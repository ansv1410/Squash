using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using System.Configuration;
using Squash.Classes;




using System.Linq;
using System.Globalization;


namespace Squash
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["squash"].ConnectionString);

            string sql = "SELECT * FROM users WHERE UserId = 1";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                testarn.Text = dr["Firstname"].ToString();
   
            }
            conn.Close();
           

        }
    }
}