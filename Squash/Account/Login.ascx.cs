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
                    //if(dr.HasRows)
                    //{
                    Users u = new Users();
                    u.Id = Convert.ToInt32(dr["Id"].ToString());
                    u.UserId = Convert.ToInt32(dr["UserId"].ToString());
                    u.FirstName = dr["Firstname"].ToString();
                    u.SurName = dr["Surname"].ToString();
                    u.Phone = dr["Phone"].ToString();
                    u.EMail = dr["EMail"].ToString();
                    u.StreatAddress = dr["StreetAddress"].ToString();
                    u.ZipCode = dr["ZipCode"].ToString();
                    u.City = dr["City"].ToString();
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
                                string query = "SELECT * FROM companies c "
                                               + "INNER JOIN membercompany mc ON c.ID = mc.CompanyId "
                                               + "INNER JOIN members m ON mc.MemberId = m.MemberId "
                                               + "WHERE m.MemberId = "+m.MemberId+";";

                                MySqlDataReader dr4 = method.myReader(query, conn);

                                if(dr4.Read())
                                {
                                    c.Id = Convert.ToInt16(dr4["Id"]);
                                    c.Name = dr4["Name"].ToString();
                                }
                            }
                            
                            
                            int memberId = m.MemberId;
                            conn.Close();

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
                            conn.Open();
                            cmdInsertLogins.ExecuteNonQuery();
                            
                            conn.Close();




                            LoggedInPerson lip = new LoggedInPerson();
                            lip.user = u;
                            lip.member = m;
                            lip.logins = l;
                            lip.company = c;

                            bool showBookingMessage = false;
                            string bookingMessage = "";

                            Session["lip"] = lip;
                            Session["showBookingMessage"] = showBookingMessage;
                            Session["bookingMessage"] = bookingMessage;

                            ((HiddenField)Page.Master.FindControl("hfShowLogin")).Value = "0";

                            FormsAuthentication.RedirectFromLoginPage(m.MemberId.ToString(), false);
                            Response.Redirect("~/Default.aspx");

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


            #region ASPs KOD SOM FÖLJDE MED
            //if (IsValid)
            //{
            //    // Validate the user password
            //    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            //    // This doen't count login failures towards account lockout
            //    // To enable password failures to trigger lockout, change to shouldLockout: true
            //    var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

            //    switch (result)
            //    {
            //        case SignInStatus.Success:
            //            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            //            break;
            //        case SignInStatus.LockedOut:
            //            Response.Redirect("/Account/Lockout");
            //            break;
            //        case SignInStatus.RequiresVerification:
            //            Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
            //                                            Request.QueryString["ReturnUrl"],
            //                                            RememberMe.Checked),
            //                              true);
            //            break;
            //        case SignInStatus.Failure:
            //        default:
            //            FailureText.Text = "Invalid login attempt";
            //            ErrorMessage.Visible = true;
            //            break;
            //    }
            //}
            #endregion
        }
    }
}