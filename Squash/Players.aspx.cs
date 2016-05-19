using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
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
using System.Data;

namespace Squash
{
    public partial class Players : System.Web.UI.Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();
        DataTable dt = new DataTable();
        LoggedInPerson lip;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["lip"] != null)
            {
                lip = (LoggedInPerson)Session["lip"];
            }
            else
            {
                //Response.Write("<script>alert('" + "Du är inte inloggad." + "')</script>");
            }

            int monthNumber = DateTime.Now.Month;

            for (int i = 0; i < 5; i++)
            {
                DateTime queryMonth = DateTime.Now.AddMonths(-i);
                DateTime toMonth = queryMonth.AddMonths(1);

                DataView dv = method.PlayerStats(queryMonth, toMonth).DefaultView;

                chPlayers.Series["Series1"].Points.DataBindXY(dv, "name", dv, "NoOfReservations");
            }






            //Chart chPlayers = new Chart();

            //MySqlDataReader dr = method.myReader(query, conn);
            
                
            //Series s = new Series("mySeries");
            //chPlayers.Series.Add(s);

            //while (dr.Read())
            //{
            //    PlayerStats PS = new PlayerStats();
            //    PS.fullName = dr["name"].ToString();
            //    PS.resCount = Convert.ToInt16(dr["NoOfReservations"]);
            //    PSList.Add(PS);



            //    chPlayers.Series["mySeries"].Points.AddXY(dr["name"].ToString(), dr["NoOfReservations"].ToString());

                    
            //}

            //chPlayers.DataSource = PSList;
            //Series s = new Series("mySeries");

            //chPlayers.Series.Add(s);
            //chPlayers.Series["mySeries"].XValueMember = "fullName";
            //chPlayers.Series["mySeries"].YValueMembers = "resCount";
            //chPlayers.DataBind();

            chartDiv.Controls.Add(chPlayers);

                
        }
    }
}