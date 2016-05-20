using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
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
using System.Drawing;

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

            for (int i = 1; i < 7; i++)
            {
                DateTime queryMonth = DateTime.Now.AddMonths(1-i);
                DateTime toMonth = queryMonth.AddMonths(1);


                HtmlGenericControl chartDiv = new HtmlGenericControl("div");
                chartDiv.Attributes.Add("id", "chartDiv" + i);
                if (i == 1)
                {
                    chartDiv.Attributes.Add("class", "chartDivs chartVisible");

                }
                else
                {
                    chartDiv.Attributes.Add("class", "chartDivs");

                }

                string titleMonth = method.FixName(queryMonth.ToString("MMMM", new CultureInfo("sv-SE")));
                string titleYear = queryMonth.ToString("yyyy", new CultureInfo("sv-SE"));


                Chart chart = new Chart();
                chart.Titles.Add(new Title(titleMonth + " - " + titleYear, Docking.Top, new Font("Tahoma", 20f, FontStyle.Bold), Color.Black));

                chart.Series.Add("Series1");
                chart.Attributes.Add("class", "monthChart");

                ChartArea ChartArea1 = new ChartArea("ChartArea1");
                //chart.ChartAreas.Add("ChartArea1");


                //System.Web.UI.DataVisualization.Charting.Title bokos = new System.Web.UI.DataVisualization.Charting.Title("Los Bokos", Docking.Top, new Font("Verdana", 12f, FontStyle.Bold), Color.Black);
                ChartArea1.AxisY = new Axis { LabelStyle = new LabelStyle() { Font = new Font("Tahoma", 15.5f) } };
                ChartArea1.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;

                ChartArea1.AxisY.Title = "Bokningar";
                ChartArea1.AxisY.TitleFont = new Font("Tahoma", 15f);

                ChartArea1.AxisX = new Axis { LabelStyle = new LabelStyle() { Font = new Font("Tahoma", 15.5f) } };
                ChartArea1.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;


                

                chart.ChartAreas.Add(ChartArea1);
                chart.Width = 1000;
                chart.Height = 500;
                chart.Palette = ChartColorPalette.BrightPastel;
                


                DataView dv = method.PlayerStats(queryMonth, toMonth).DefaultView;

                chart.Series["Series1"].Points.DataBindXY(dv, "name", dv, "NoOfReservations");


                chartDiv.Controls.Add(chart);
                statsDiv.Controls.Add(chartDiv);
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

            //chartDiv.Controls.Add(chPlayers);

                
        }
    }
}