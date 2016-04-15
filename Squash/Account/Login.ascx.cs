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
using System.Text;


namespace Squash_Template.Account
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
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ((HiddenField)Page.Master.FindControl("hfShowLogin")).Value = "0";

                //hfShowLogin.Value = "0";
                string email = tbLogInEmail.Text;
                string password = tbLogInPassword.Text;

                string queryEmail = "SELECT * FROM users WHERE EMail = '" + email + "'";
                string queryPw = "";
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


                    //Behövs nog inte göras en ny DataReader. Har som test nu bara.
                    //queryPw = "SELECT * FROM users WHERE EMail = '" + email + "' AND UserId = '" + u.UserId + "'";
                    //MySqlDataReader dr2 = method.myReader(queryPw, conn);


                    //while(dr2.Read())
                    //{
                    //Users uPw = new Users();
                    //uPw.Password = dr2["Password"].ToString();
                    //uPw.UserId = Convert.ToInt32(dr2["UserId"].ToString());

                    if (u.Password == method.Hashify(password))
                    //if(dr2.HasRows)
                    {
                        LoggedInUser.Text = "Välkommen " + u.FirstName + " " + u.SurName + ".";
                        conn.Close();

                        queryMemberId = "SELECT * FROM members WHERE UserId = '" + u.UserId + "'";
                        MySqlDataReader dr3 = method.myReader(queryMemberId, conn);

                        while (dr3.Read())
                        {
                            Members m = new Members();
                            m.MemberId = Convert.ToInt32(dr3["MemberId"].ToString());
                            m.UserId = Convert.ToInt32(dr3["UserId"].ToString());
                            m.StatusId = Convert.ToInt32(dr3["StatusId"].ToString());
                            m.ApprovedBy = Convert.ToInt32(dr3["ApprovedBy"].ToString());
                            m.SessionId = dr3["SessionId"].ToString();
                            m.MemberType = Convert.ToInt32(dr3["MemberType"].ToString());

                            LoggedInMember.Text = "MedlemsId: " + m.MemberId;
                            int memberId = m.MemberId;
                            conn.Close();


                            //EN INSERT TILL logins-tabellen.
                            queryInsertLogins = "INSERT INTO logins (MemberId, LoggedIn, IPAddress) VALUES (@mid, @li, @ip)";
                            MySqlCommand cmdInsertLogins = new MySqlCommand(queryInsertLogins, conn);
                            cmdInsertLogins.Parameters.AddWithValue("mid", memberId);
                            cmdInsertLogins.Parameters.AddWithValue("li", DateTime.Now);
                            cmdInsertLogins.Parameters.AddWithValue("ip", "123.456.789.1011");
                            conn.Open();
                            cmdInsertLogins.ExecuteNonQuery();
                            conn.Close();
                            //FORM.REDIRECT(Nyhetssida).

                        }
                    }
                    else
                    {
                        LoggedInUser.Text = "Fel lösenord.";
                        LoggedInMember.Text = "";
                    }
                }
                //}
                //else
                //{
                //    LoggedInUser.Text = "Fel Email.";
                //}
                //}
                else
                {
                    LoggedInUser.Text = "Fel Email.";
                    LoggedInMember.Text = "";
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
                //hfShowLogin.Value = "1";

                //StringBuilder SB = new StringBuilder();
                //SB.Append("<script type='text/javascript'>function OpenOverlay() {");
                //SB.Append("$('.overlay-container').fadeIn('slow');");
                //SB.Append("return false;");
                //SB.Append("}</script>");

                //if (!Page.ClientScript.IsClientScriptBlockRegistered("JSScriptBlock"))
                //{
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "JSScriptBlock", SB.ToString());
                //}

                //string funcCall = "<script language='javascript'>OpenOverlay();</script>";

                //if (!Page.ClientScript.IsStartupScriptRegistered("JSScript"))
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "JSScript", funcCall);
                //}

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