using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Squash.Classes;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;

namespace Squash
{
    public partial class _Default : Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string query = "SELECT * FROM users WHERE UserId = 1";
                MySqlDataReader dr = method.myReader(query, conn);
                try
                {
                    while (dr.Read())
                    {
                        testarn.Text = dr["Firstname"].ToString();
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

                Messages m = new Messages();

            }
        }
    }
}