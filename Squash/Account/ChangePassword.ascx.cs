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
    public partial class ChangePassword : System.Web.UI.UserControl
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();
        LoggedInPerson lip;
        protected void Page_Load(object sender, EventArgs e)
        {
            lip = (LoggedInPerson)Session["lip"];
        }

        protected void btnUpdatePW_Click(object sender, EventArgs e)
        {

            //if (Page.IsValid)
            //{

            //    string oldPasswordInput = tbMPOldPassword.Text;
            //    string oldPasswordRead;
            //    string newPW = tbMPPassword.Text;
            //    string confirmNewPW = tbMPConfirmPassword.Text;

            //    string queryPW = "SELECT Password FROM users_updated WHERE Id = '" + lip.user.Id + "'";

            //    MySqlDataReader dr = method.myReader(queryPW, conn);

            //    if (dr.Read())
            //    {
            //        oldPasswordRead = dr["Password"].ToString();
            //        conn.Close();
            //        if (oldPasswordRead == method.Hashify(oldPasswordInput))
            //        {
            //            string queryUpdatePW = "UPDATE users_updated "
            //                                + "SET Password = @pw "
            //                                + "WHERE Id = '" + lip.user.Id + "'";

            //            MySqlCommand cmd = new MySqlCommand(queryUpdatePW, conn);
            //            string PwHash = method.Hashify(newPW);
            //            cmd.Parameters.AddWithValue("pw", PwHash);
                        
            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //            conn.Close();
            //            lip.user.Password = PwHash;
            //            Session["lip"] = lip;
            //        }
            //        else
            //        {
            //            string pwID = e.CommandArgument.ToString();
            //            Page.Response.Redirect("MyPage.aspx");
            //        }
            //    }


            //}
        }

        protected void btnUpdatePW_Command(object sender, CommandEventArgs e)
        {
            if (Page.IsValid)
            {

                string oldPasswordInput = tbMPOldPassword.Text;
                string oldPasswordRead;
                string newPW = tbMPPassword.Text;
                string confirmNewPW = tbMPConfirmPassword.Text;

                string queryPW = "SELECT Password FROM users_updated WHERE Id = '" + lip.user.Id + "'";

                MySqlDataReader dr = method.myReader(queryPW, conn);

                if (dr.Read())
                {
                    oldPasswordRead = dr["Password"].ToString();
                    conn.Close();
                    if (oldPasswordRead == method.Hashify(oldPasswordInput))
                    {
                        string queryUpdatePW = "UPDATE users_updated "
                                            + "SET Password = @pw "
                                            + "WHERE Id = '" + lip.user.Id + "'";

                        MySqlCommand cmd = new MySqlCommand(queryUpdatePW, conn);
                        string PwHash = method.Hashify(newPW);
                        cmd.Parameters.AddWithValue("pw", PwHash);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        lip.user.Password = PwHash;
                        Session["lip"] = lip;
                        Response.Redirect("MyPage.aspx");
                    }
                    else
                    {
                        string pwID = e.CommandArgument.ToString();
                        Response.Redirect("MyPage.aspx?falsePw=" + pwID);
                    }
                }


            }
        }
    }
}