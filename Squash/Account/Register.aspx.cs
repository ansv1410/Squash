using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Squash.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using Squash.Classes;
using System.Net;
using System.IO;
using System.Globalization;


namespace Squash.Account
{
    public partial class Register : Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string telNum = tbTelephone.Text;
                string newNum = method.FixNumber(telNum);
                string corrNum = "";


                //foreach (char c in telNum)
                //{
                //    if (Char.IsLetter(c))
                //    {
                //        newNum += "";
                //    }
                //    else if (Char.IsDigit(c))
                //    {
                //        newNum += c;
                //    }
                //    else
                //    {
                //        newNum += "";
                //    }
                //}

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

                string email = tbEmail.Text;
                string queryEmailExist = "SELECT * FROM users WHERE EMail = '" + email + "'";

                MySqlDataReader dr = method.myReader(queryEmailExist, conn);

                string pw = tbConfirmPassword.Text;
                int agreed = 0;


                if(rblAgreement.SelectedValue == "Agree")
                {
                    agreed = 1;
                }


                //string url = "http://checkip.dyndns.org";
                //WebRequest request = WebRequest.Create(url);
                //WebResponse resp = request.GetResponse();
                //StreamReader sr = new StreamReader(resp.GetResponseStream());
                //string response = sr.ReadToEnd().Trim();
                //string[] a = response.Split(':');
                //string a2 = a[1].Substring(1);
                //string[] a3 = a2.Split('<');
                //string ipAddress = a3[0];

                string ipAddress = ((HiddenField)Page.Master.FindControl("hfLoggedInIP")).Value;

                string queryInsToUser = "";
                string queryInsToMember = "";

                try
                {
                    if(!dr.HasRows)
                    {
                        //Gör om alla strängar med text [0].To.Upper, SubString.ToLower.
                        //string tbFN = tbFirstName.Text;
                        //string firstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbFN);
                        string firstName = method.FixName(tbFirstName.Text);

                        //string tbSN = tbSurName.Text;
                        //string surName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbSN);
                        string surName = method.FixName(tbSurName.Text);

                        //string tbSA = tbStreetAddress.Text;
                        //string streetAddress = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbSA);
                        string streetAddress = method.FixName(tbStreetAddress.Text);

                        //string tbC = tbCity.Text;
                        //string city = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbC);
                        string city = method.FixName(tbCity.Text);


                        Users createUser = new Users()
                        {
                            FirstName = firstName,
                            SurName = surName,
                            Phone = newNum,
                            EMail = tbEmail.Text,
                            StreatAddress = streetAddress,
                            ZipCode = tbPostalCode.Text,
                            City = city,
                            IPAddress = ipAddress,
                            Password = method.Hashify(pw),
                            PublicAddres = agreed
                        };
                        conn.Close();


                        queryInsToUser = "INSERT INTO users (Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, PublicAddress) VALUES (@fn, @sn, @p, @e, @sa, @zc, @c, @ip, @pw, @pa)";
                        MySqlCommand cmd = new MySqlCommand(queryInsToUser, conn);
                        cmd.Parameters.AddWithValue("fn", createUser.FirstName);
                        cmd.Parameters.AddWithValue("sn", createUser.SurName);
                        cmd.Parameters.AddWithValue("p", createUser.Phone);
                        cmd.Parameters.AddWithValue("e", createUser.EMail);
                        cmd.Parameters.AddWithValue("sa", createUser.StreatAddress);
                        cmd.Parameters.AddWithValue("zc", createUser.ZipCode);
                        cmd.Parameters.AddWithValue("c", createUser.City);
                        cmd.Parameters.AddWithValue("ip", createUser.IPAddress);
                        cmd.Parameters.AddWithValue("pw", createUser.Password);
                        cmd.Parameters.AddWithValue("pa", createUser.PublicAddres);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        //cmd.ExecuteScalar();

                        //Möjligtvis använda returning ID för att göra en insert till members.
                        //Alternativt typ: int id = conn.LastInsertedId;
                    }
                    else
                    {
                        //E-posten användaren angav finns redan i databasen, vänligen ange en ny.
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

                #region ONÖDIGA SAKER
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                //var user = new ApplicationUser() { UserName = tbEmail.Text, Email = tbEmail.Text };
                //IdentityResult result = manager.Create(user, tbPassword.Text);
                //if (result.Succeeded)
                //{
                //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //    //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //    //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                //    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                //}
                //else
                //{
                //    ErrorMessage.Text = result.Errors.FirstOrDefault();
                //}
                #endregion

            }

            else
            {
                //ErrorMessage.Text = "Det funkar inte det här.";
            }


            


        }


    }
}