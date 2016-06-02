using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using Squash.Models;
using Squash.Classes;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Text;
using System.Web.UI.HtmlControls;


namespace Squash.Account
{
    public partial class Login : System.Web.UI.UserControl
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            //hfShowLogin.Value = "1";
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
            //(HtmlGenericControl)Page.Master.FindControl("pubAcc").A
            //((PlaceHolder)Page.Master.FindControl("pubAcc")).Visible = true;

            //PlaceHolder ph = (PlaceHolder)Page.Master.FindControl("pubAcc");
            //ph.Visible = true;
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                

                //hfShowLogin.Value = "0";
            string email = tbLogInEmail.Text;
            string password = tbLogInPassword.Text;

            string queryEmail = "SELECT * FROM users_updated WHERE EMail = '" + email + "'";
            string queryMemberId = "";
            string queryInsertLogins = "";

            MySqlDataReader dr = method.myReader(queryEmail, conn);


            try
            {
                if (dr.Read())
                {
                    
                    Users u = new Users();
                    u.Id = Convert.ToInt32(dr["Id"].ToString());
                    u.UserId = Convert.ToInt32(dr["UserId"].ToString());
                    u.FirstName = method.FixName(dr["Firstname"].ToString());
                    u.SurName = method.FixName(dr["Surname"].ToString());
                    u.Phone = method.FixNumber(dr["Phone"].ToString());
                    u.EMail = dr["EMail"].ToString();
                    u.StreatAddress = method.FixName(dr["StreetAddress"].ToString());
                    u.ZipCode = method.FixNumber(dr["ZipCode"].ToString());
                    u.City = method.FixName(dr["City"].ToString());
                    u.IPAddress = dr["IPAddress"].ToString();
                    u.Password = dr["Password"].ToString();
                    u.Cellular = dr["Cellular"].ToString();
                    u.PublicAddres = Convert.ToInt32(dr["PublicAddress"].ToString());

                    conn.Close();


                    string ipAddress = ((HiddenField)Page.Master.FindControl("hfLoggedInIP")).Value;

                    if (u.Password == method.Hashify(password))
                    {
                        queryMemberId = "SELECT * FROM members WHERE UserId = '" + u.UserId + "'";
                        MySqlDataReader dr3 = method.myReader(queryMemberId, conn);

                        if (dr3.Read())
                        {
                            Members m = new Members();
                            m.MemberId = Convert.ToInt32(dr3["MemberId"].ToString());
                            m.UserId = Convert.ToInt32(dr3["UserId"].ToString());
                            m.StatusId = Convert.ToInt32(dr3["StatusId"].ToString());
                            m.ApprovedBy = Convert.ToInt32(dr3["ApprovedBy"].ToString());
                            m.SessionId = dr3["SessionId"].ToString();
                            m.MemberType = Convert.ToInt32(dr3["MemberType"].ToString());


                            Companies c = new Companies();
                            if(m.MemberType == 2 || m.MemberType == 3)
                            {
                                string query = "SELECT * FROM Companies c "
                                               + "INNER JOIN MemberCompany mc ON c.ID = mc.CompanyId "
                                               + "INNER JOIN members m ON mc.MemberId = m.MemberId "
                                               + "WHERE m.MemberId = "+m.MemberId+";";

                                MySqlDataReader dr4 = method.myReader(query, conn);

                                if(dr4.Read())
                                {
                                    c.Id = Convert.ToInt16(dr4["Id"]);
                                    c.Name = dr4["Name"].ToString();
                                }
                            }
                            
                            conn.Close();
                            
                            int memberId = m.MemberId;

                            Logins l = new Logins();
                            l.MemberId = memberId;
                            l.LoggedIn = DateTime.Now;
                            l.IPAddress = ipAddress;

                            //EN INSERT TILL logins-tabellen.
                            queryInsertLogins = "INSERT INTO logins (MemberId, LoggedIn, IPAddress) VALUES (@mid, @li, @ip)";
                            MySqlCommand cmdInsertLogins = new MySqlCommand(queryInsertLogins, conn);
                            cmdInsertLogins.Parameters.AddWithValue("mid", l.MemberId);
                            cmdInsertLogins.Parameters.AddWithValue("li", l.LoggedIn);
                            cmdInsertLogins.Parameters.AddWithValue("ip", l.IPAddress);

                            try
                            {
                            conn.Open();
                            cmdInsertLogins.ExecuteNonQuery();
                            
                            conn.Close();
                            }
                            catch(MySqlException ex)
                            {
                                Debug.WriteLine(ex.Message);
                                conn.Close();
                            }




                            LoggedInPerson lip = new LoggedInPerson();
                            lip.user = u;
                            lip.member = m;
                            lip.logins = l;
                            lip.company = c;
                            if (lip.member.MemberType == 3)
                            {
                                lip.memberfloatable = lip.IsMF();
                            }
                            bool showBookingMessage = false;
                            string bookingMessage = "";

                            Session["lip"] = lip;
                            Session["showBookingMessage"] = showBookingMessage;
                            Session["bookingMessage"] = bookingMessage;

                            string rawUrl = Request.RawUrl;
                            string rawUrlSub = "";
                            if (rawUrl.Length >= 7)
                            {
                                rawUrlSub = rawUrl.Substring(1, 7);
                            }

                            ((HiddenField)Page.Master.FindControl("hfShowLogin")).Value = "0";

                            FormsAuthentication.RedirectFromLoginPage(m.MemberId.ToString(), false);
                            if (rawUrlSub == "Account")
                            {
                                Response.Redirect("~/Default");
                            }
                            else
                            {
                                Response.Redirect(rawUrl);
                            }

                        }
                        else
                        {
                            LoginFail.Text = "Administratör har inte godkänt dig än.";
                            
                        }

                    }
                    else
                    {
                        LoginFail.Text = "Fel e-post eller lösenord.";
                        
                    }
                }

                else
                {
                    LoginFail.Text = "Fel e-post eller lösenord.";
                    
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            }
            else
            {

            }

        }
    }
}