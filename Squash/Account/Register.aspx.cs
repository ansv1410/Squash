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
                string errorMessage = "";


                bool correctPhone = false;
                bool correctZip = false;

                string telNum = tbTelephone.Text;
                string newNum = method.FixNumber(telNum);
                string zipCode = method.FixNumber(tbPostalCode.Text);

                string corrNum = "";

                /*FOR TELEPHONE*/
                
                if (newNum == "")
                {
                    errorMessage += "Ogiltigt telefonnummer. ";
                }

                //Börja med 0a, minst 8 siffror, max 15
                else if (newNum[0] == '0' && newNum.Length >= 8 && newNum.Length <= 15)
                {
                    correctPhone = true;
                    //corrNum = newNum.Insert(3, "-");
                    //lblMessage.Text = newNum;
                }

                else
                {
                    errorMessage += "Ogiltigt telefonnummer. ";
                }




                /*FOR ZIPCODE*/
                if(zipCode == "" || zipCode.Length != 5)
                {
                    errorMessage += "Ogiltigt postnummer. ";
                }
                else if (zipCode.Length == 5)
                {
                    correctZip = true;
                }



                if(errorMessage == "")
                {
                    lblMessage.Visible = false;
                }
                else
                {
                    lblMessage.Text = errorMessage;
                }





                string pw = tbConfirmPassword.Text;
                int agreed = 0;


                if(rblAgreement.SelectedValue == "Agree")
                {
                    agreed = 1;
                }


                string ipAddress = ((HiddenField)Page.Master.FindControl("hfLoggedInIP")).Value;

                string queryInsToUser = "";

                try
                {
                    if (correctPhone && correctZip)
                    {
                        if (!method.EmailExist(tbEmail.Text))
                        {
                            string firstName = method.FixName(tbFirstName.Text);
                            string surName = method.FixName(tbSurName.Text);
                            string streetAddress = method.FixName(tbStreetAddress.Text);
                            string city = method.FixName(tbCity.Text);


                            Users createUser = new Users()
                            {
                                FirstName = firstName,
                                SurName = surName,
                                Phone = newNum, //Möjligtvis corrNum om man vill ha med ett -.
                                EMail = tbEmail.Text,
                                StreatAddress = streetAddress,
                                ZipCode = tbPostalCode.Text,
                                City = city,
                                IPAddress = ipAddress,
                                Password = method.Hashify(pw),
                                PublicAddres = agreed
                            };
                            conn.Close();


                            queryInsToUser = "START TRANSACTION; "
                                            + "INSERT INTO users (Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress) "
                                            + "VALUES (@fn, @sn, @p, @e, @sa, @zc, @c, @ip, @pw, @cell, @pa); "
                                            + "INSERT INTO users_updated (UserId, Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress) "
                                            + "SELECT UserId, Firstname, Surname, Phone, EMail, StreetAddress, ZipCode, City, IPAddress, Password, Cellular, PublicAddress FROM users WHERE UserId = last_insert_id(); "
                                            + "COMMIT;";

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
                            cmd.Parameters.AddWithValue("cell", "");
                            cmd.Parameters.AddWithValue("pa", createUser.PublicAddres);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            lblMessage.Text = "E-posten du angav finns redan i databasen, vänligen ange en ny.";
                        }
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

    }
}