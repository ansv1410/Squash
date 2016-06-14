using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using Squash.Classes;
using System.Diagnostics;
using System.Web.UI.HtmlControls;



namespace Squash
{
    public partial class SiteMaster : MasterPage
    {
        LoggedInPerson lip;
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();


        protected void Page_Init(object sender, EventArgs e)
        {
            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (Session["lip"] != null) //Inloggad person lagras i en Session av klassen LoggedInPerson
            {
                lip = (LoggedInPerson)Session["lip"];
                pinDiv.Visible = ShowPinDiv();

            }
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["lip"] != null)
            {
                lip = (LoggedInPerson)Session["lip"];
                privAcc.Visible = true;
                postitnote.Visible = false;


                int companyNameLength = 0;
                int firstNameLength = 0;
                int lastNameLength = 0;

                if((lip.member.MemberType == 2 || lip.member.MemberType == 3) && lip.company.Name != null) //Om memberType är 2, 3 eller om lips företagsnamn skiljer sig från null
                {
                    companyNameLength = lip.company.Name.Length;

                    string companyName = lip.company.Name;
                    string shortCompanyName = "";


                    if(companyNameLength <= 14) //Om företagsnamnet är mindre eller lika med 14 tecken så står det i headern till vänster om "Logga ut".
                    {
                        myPageLink.InnerText = lip.company.Name;
                    }
                    else
                    {
                        foreach (char c in companyName)
                        {
                            if(c.ToString() != " ")
                            {
                                shortCompanyName += c;
                            }
                            else if (c.ToString() == " ")
                            {
                                break;
                            }
                        }
                        
                        if(shortCompanyName.Length < 14)
                        {
                            myPageLink.InnerText = shortCompanyName;
                        }
                        else
                        {
                            myPageLink.InnerText = shortCompanyName.Substring(0, 14);
                        }
                    }



                    
                }
                else
                {
                    firstNameLength = lip.user.FirstName.Length;
                    lastNameLength = lip.user.SurName.Length;

                    if(firstNameLength + lastNameLength <= 14)
                    {
                        myPageLink.InnerText = lip.user.FirstName + " " + lip.user.SurName;  //förnamn + efternamn i headern
                    }
                    else if(firstNameLength <= 14)
                    {
                        myPageLink.InnerText = lip.user.FirstName; //Bara förnamn i headern
                    }
                    else if(firstNameLength > 14)
                    {
                        string firstName = lip.user.FirstName;

                        string firstNameToShow = firstName.Substring(0, 14);

                        myPageLink.InnerText = firstNameToShow; //Förnamnets 14 första tecken
                    }


                }

            }
            else
            {
                
                if (Session["showPostit"] != null)
                {
                    string showPostit = Session["showPostit"].ToString();
                    postitnote.Visible = false;
                }
                pinDiv.Visible = false;
                pubAcc.Visible = true;
            }

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void lbtnLogin_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Default");
        }

        /// <summary>
        /// Visar PIN-koden i headern om man har en reservation som är inom en timme idag eller om man har valt att se dagens PIN.
        /// </summary>
        /// <returns>Returnerar en bool(true) om man har valt att se PIN-koden eller om man har en reservation inom en timme från nu</returns>
        public bool ShowPinDiv()
        {
            List<Tuple<Reservations, Courts, ReservationTypes>> resList = method.GetResTuples(lip);
            
            List<CodeLock> codeLockList = method.GetCodeLocks();
            
            if (!ShowSubPin(codeLockList))
            {
                foreach (Tuple<Reservations, Courts, ReservationTypes> t in resList)
                {
                    if (t.Item1.StartDate.Date == DateTime.Now.Date)
                    {
                        foreach (CodeLock codelock in codeLockList)
                        {
                            if (codelock.DateOfChange > t.Item1.StartDate == false)
                            {
                                if (DateTime.Now.AddHours(1) > t.Item1.StartDate)
                                {
                                    todaysPin.InnerHtml = "Dagens PIN<br />" + "<p class='pinNumber'> "+codelock.Code+"</p>";
                                    todaysPin.Visible = true;
                                    if (!method.HasCLRequest(lip))
                                    {
                                        string query = "INSERT INTO codelockrequests (MemberId, DateOfRequest) VALUES(@mid, @date)";
                                        MySqlCommand cmdInsertToCLR = new MySqlCommand(query, conn);
                                        cmdInsertToCLR.Parameters.AddWithValue("mid", lip.member.MemberId);
                                        cmdInsertToCLR.Parameters.AddWithValue("date", DateTime.Now);

                                        try
                                        {
                                        conn.Open();
                                        cmdInsertToCLR.ExecuteNonQuery();
                                        conn.Close();
                                        }
                                        catch(MySqlException ex)
                                        {
                                            Debug.WriteLine(ex.Message);
                                            conn.Close();
                                        }

                                    }
                                   
                                }
                                else
                                {
                                    if (method.HasCLRequest(lip))
                                    {
                                        todaysPin.InnerHtml = "Dagens PIN<br />" + codelock.Code;
                                        todaysPin.Visible = true;
                                    }
                                    else
                                    {
                                        todaysPin.InnerHtml = "Dagens PIN<br />" + codelock.Code;
                                        showPin.Visible = true;
                                    }
                                    conn.Close();
                                }
                                break;
                            }


                        }

                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            
            return false;

        }
        
        /// <summary>
        /// Visar PIN-kod i headern om inloggad person är ett företag och har en abonnemangstid på den aktuella dagen
        /// </summary>
        /// <param name="codeLockList"></param>
        /// <returns>Sant om ovan uppfylls, annars falskt</returns>
        public bool ShowSubPin(List<CodeLock> codeLockList)
        {
            List<Tuple<Subscriptions, CourtTimes, Days>> subList = method.GetSubTuples(lip);

            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));
            if (todayNo == 0)
            {
                todayNo = 7;
            }

            foreach (Tuple<Subscriptions, CourtTimes, Days> t in subList)
            {
                if (todayNo == t.Item3.DayId)
                {
                    if (!method.HasCLRequest(lip))
                    {
                        string query = "INSERT INTO codelockrequests (MemberId, DateOfRequest) VALUES(@mid, @date)";
                        MySqlCommand cmdInsertToCLR = new MySqlCommand(query, conn);
                        cmdInsertToCLR.Parameters.AddWithValue("mid", lip.member.MemberId);
                        cmdInsertToCLR.Parameters.AddWithValue("date", DateTime.Now);

                        try
                        {
                        conn.Open();
                        cmdInsertToCLR.ExecuteNonQuery();
                        conn.Close();
                        }
                        catch (MySqlException ex)
                        {
                            Debug.WriteLine(ex.Message);
                            conn.Close();
                        }

                    }

                    foreach (CodeLock codelock in codeLockList)
                    {

                        if (DateTime.Now >= codelock.DateOfChange)
                        {
                            todaysPin.InnerHtml = "Dagens PIN<br />" + codelock.Code;
                            todaysPin.Visible = true;
                            return true;
                        }

                    }
                }
            }




            return false;
        }

        protected void lbtnShowPin_Click(object sender, EventArgs e)
        {
            //INSERT till codelockrequest
            string query = "INSERT INTO codelockrequests (MemberId, DateOfRequest) VALUES(@mid, @date)";
            MySqlCommand cmdInsertToCLR = new MySqlCommand(query, conn);
            cmdInsertToCLR.Parameters.AddWithValue("mid", lip.member.MemberId);
            cmdInsertToCLR.Parameters.AddWithValue("date", DateTime.Now);

            try
            {
            conn.Open();
            cmdInsertToCLR.ExecuteNonQuery();
            conn.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                conn.Close();
            }

            ShowPinDiv();

            //string url = HttpContext.Current.Request.RawUrl;
            Response.Redirect(Request.RawUrl);
            
        }


    }
}