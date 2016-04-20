using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Squash.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using Squash.Classes;
using System.Net;
using System.IO;
using System.Globalization;

namespace Squash.Account
{
    public partial class EditUserInfo : System.Web.UI.UserControl
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();
        LoggedInPerson lip;
        protected void Page_Load(object sender, EventArgs e)
        {
            lip = (LoggedInPerson)Session["lip"];

            if (!Page.IsPostBack)
            {
                //Response.Write("<script>alert('" + lip.user.FirstName + " " + lip.member.MemberId + " " + lip.logins.IPAddress + "')</script>");

                tbMPFirstName.Text = lip.user.FirstName;
                tbMPSurName.Text = lip.user.SurName;
                tbMPStreetAddress.Text = lip.user.StreatAddress;
                tbMPPostalCode.Text = lip.user.ZipCode;
                tbMPCity.Text = lip.user.City;
                tbMPTelephone.Text = lip.user.Phone;
                tbMPEmail.Text = lip.user.EMail;
            
                if(lip.user.PublicAddres == 1)
                {
                    rblMPAgreement.SelectedValue = "Agree";
                }
                else if(lip.user.PublicAddres == 0)
                {
                    rblMPAgreement.SelectedValue = "Disagree";
                }
            }
        }

        protected void UpdateInfo_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //Response.Write("<script>alert('" + "hej" + "')</script>");

                //string telNum = tbMPTelephone.Text;
                //string newNum = method.FixNumber(telNum);


                //if (newNum[0] == '0' && newNum.Length >= 8 && newNum.Length <= 15)
                //{
                //    //corrNum = newNum.Insert(3, "-");
                //    lblMPMessage.Text = newNum;
                //}
                //else
                //{
                //    lblMPMessage.Text = "Ogiltigt telefonnummer";
                //}

                //string postalCode = tbMPPostalCode.Text;


                //string ipAddress = ((HiddenField)Page.Master.FindControl("hfLoggedInIP")).Value;
                //string pw = tbMPConfirmPassword.Text;


                //int agreed = 0;

                //if (rblMPAgreement.SelectedValue == "Agree")
                //{
                //    agreed = 1;
                //}

                try
                {
                    if (!method.EmailExist(tbMPEmail.Text))
                    {
                        UpdateUser();

                        //string firstName = method.FixName(tbMPFirstName.Text);
                        //string surName = method.FixName(tbMPSurName.Text);
                        //string streetAddress = method.FixName(tbMPStreetAddress.Text);
                        //string city = method.FixName(tbMPCity.Text);

                        //int userId = lip.user.UserId;

                        //Users UpdateUser = new Users()
                        //{
                        //    FirstName = firstName,
                        //    SurName = surName,
                        //    Phone = newNum,
                        //    EMail = email,
                        //    StreatAddress = streetAddress,
                        //    ZipCode = postalCode,
                        //    City = city,
                        //    IPAddress = ipAddress,
                        //    Password = method.Hashify(pw),
                        //    PublicAddres = agreed
                        //};
                        //conn.Close();

                        //queryUpdateUser = "UPDATE users "
                        //    + "SET Firstname = @fn, Surname = @sn, Phone = @p, EMail = @e, "
                        //    + "StreetAddress = @sa, ZipCode = @zc, City = @c, IPAddress = @ip, Password = @pw, PublicAddress = @pa "
                        //    + "WHERE UserId = '" + userId + "'";

                        //MySqlCommand cmd = new MySqlCommand(queryUpdateUser, conn);
                        //cmd.Parameters.AddWithValue("fn", UpdateUser.FirstName);
                        //cmd.Parameters.AddWithValue("sn", UpdateUser.SurName);
                        //cmd.Parameters.AddWithValue("p", UpdateUser.Phone);
                        //cmd.Parameters.AddWithValue("e", UpdateUser.EMail);
                        //cmd.Parameters.AddWithValue("sa", UpdateUser.StreatAddress);
                        //cmd.Parameters.AddWithValue("zc", UpdateUser.ZipCode);
                        //cmd.Parameters.AddWithValue("c", UpdateUser.City);
                        //cmd.Parameters.AddWithValue("ip", UpdateUser.IPAddress);
                        //cmd.Parameters.AddWithValue("pw", UpdateUser.Password);
                        //cmd.Parameters.AddWithValue("pa", UpdateUser.PublicAddres);

                        //conn.Open();
                        //cmd.ExecuteNonQuery();

                    }

                    else if (lip.user.EMail == tbMPEmail.Text)
                    {
                        UpdateUser();
                    }

                    else
                    {
                        Response.Write("<script>alert('" + "E-post finns redan i databasen, vänligen ange en ny." + "')</script>");
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

            }


        }


        public void UpdateUser()
        {
            string firstName = method.FixName(tbMPFirstName.Text);
            string surName = method.FixName(tbMPSurName.Text);
            string streetAddress = method.FixName(tbMPStreetAddress.Text);
            string city = method.FixName(tbMPCity.Text);
            string telNum = tbMPTelephone.Text;
            string newNum = method.FixNumber(telNum);
            string postalCode = tbMPPostalCode.Text;
            string email = tbMPEmail.Text;
            string ipAddress = ((HiddenField)Page.Master.FindControl("hfLoggedInIP")).Value;
            string pw = tbMPConfirmPassword.Text;
            int agreed = 0;
            if (rblMPAgreement.SelectedValue == "Agree")
            {
                agreed = 1;
            }

            int userId = lip.user.UserId;

            Users UpdateUser = new Users()
            {
                UserId = userId,
                FirstName = firstName,
                SurName = surName,
                Phone = newNum,
                EMail = email,
                StreatAddress = streetAddress,
                ZipCode = postalCode,
                City = city,
                IPAddress = ipAddress,
                Password = method.Hashify(pw),
                PublicAddres = agreed
            };
            conn.Close();

            string queryUpdateUser = "UPDATE users_update "
                + "SET Firstname = @fn, Surname = @sn, Phone = @p, EMail = @e, "
                + "StreetAddress = @sa, ZipCode = @zc, City = @c, IPAddress = @ip, Password = @pw, PublicAddress = @pa "
                + "WHERE UserId = '" + userId + "'";

            MySqlCommand cmd = new MySqlCommand(queryUpdateUser, conn);
            cmd.Parameters.AddWithValue("fn", UpdateUser.FirstName);
            cmd.Parameters.AddWithValue("sn", UpdateUser.SurName);
            cmd.Parameters.AddWithValue("p", UpdateUser.Phone);
            cmd.Parameters.AddWithValue("e", UpdateUser.EMail);
            cmd.Parameters.AddWithValue("sa", UpdateUser.StreatAddress);
            cmd.Parameters.AddWithValue("zc", UpdateUser.ZipCode);
            cmd.Parameters.AddWithValue("c", UpdateUser.City);
            cmd.Parameters.AddWithValue("ip", UpdateUser.IPAddress);
            cmd.Parameters.AddWithValue("pw", UpdateUser.Password);
            cmd.Parameters.AddWithValue("pa", UpdateUser.PublicAddres);

            conn.Open();
            cmd.ExecuteNonQuery();

            lip.user = UpdateUser;
            Session["lip"] = lip;

            Response.Redirect("MyPage.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Players.aspx");
        }
    }
}