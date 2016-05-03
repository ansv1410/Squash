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
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();

        //public string ShowOrNot
        //{
        //    get
        //    {
        //        return hfShowLogin.Value;
        //    }
        //    set
        //    {
        //        hfShowLogin.Value = value;
        //    }
        //}

        protected void Page_Init(object sender, EventArgs e)
        {
            //string testarn = hfShowLogin.Value;
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
                    
                if (Session == null)
                {
                    
                    //(this.FindControl("pubAcc")).Visible = true;

                    //((HiddenField)Page.Master.FindControl("hfShowLogin")).Value = "0";
                }
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["lip"] != null)
            {
                LoggedInPerson lip = (LoggedInPerson)Session["lip"];
                privAcc.Visible = true;
                
                if((lip.member.MemberType == 2 || lip.member.MemberType == 3) && lip.company.Name != null)
                {
                    myPageLink.InnerText = lip.company.Name;
                }
                else
                {
                    myPageLink.InnerText = lip.user.FirstName + " " + lip.user.SurName; 
                }

            }
            else
            {
                pubAcc.Visible = true;
            }

            //(HtmlGenericControl)Page.Master.FindControl("pubAcc").
            //HtmlGenericControl accMenu = (HtmlGenericControl)m.FindControl("pubAcc");
            //accMenu.Attributes.Remove("hidden");
            
            
            

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


    }
}