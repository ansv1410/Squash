using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Squash.Models;
using System.Collections.Generic;

namespace Squash.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string telNum = tbTelephone.Text;
                string newNum = "";
                string corrNum = "";


                foreach (char c in telNum)
                {
                    if (Char.IsLetter(c))
                    {
                        newNum += "";
                    }
                    //else if(numbers.Contains(c))
                    else if (Char.IsDigit(c))
                    {
                        newNum += c;
                    }
                    else
                    {
                        newNum += "";
                    }
                }

                //Börja med 0a, minst 8 siffror, max 15
                if (newNum[0] == '0' && newNum.Length >= 8 && newNum.Length <= 15)
                {
                    //corrNum = newNum.Insert(3, "-");
                    lblMessage.Text = newNum;
                }
                else
                {
                    lblMessage.Text = "Ogiltigt telefonnummer";
                }




                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                var user = new ApplicationUser() { UserName = tbEmail.Text, Email = tbEmail.Text };
                IdentityResult result = manager.Create(user, tbPassword.Text);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //string code = manager.GenerateEmailConfirmationToken(user.Id);
                    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    //ErrorMessage.Text = result.Errors.FirstOrDefault();
                }

            }

            else
            {
                //ErrorMessage.Text = "Det funkar inte det här.";
            }


            


        }
    }
}