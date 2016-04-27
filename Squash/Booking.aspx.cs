using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using Squash.Classes;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Squash
{
    public partial class Booking : System.Web.UI.Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Days> DayList = new List<Days>();

            string query = "SELECT * FROM days";

            MySqlDataReader dr = method.myReader(query, conn);

            while (dr.Read())
            {
                Days day = new Days();
                day.DayId = Convert.ToInt16(dr["DayId"]);
                day.Description = dr["Description"].ToString();
                day.Courts = new List<Courts>();
                day.CourtTimes = new List<CourtTimes>();
                DayList.Add(day);
            }
            conn.Close();
            
            query = "SELECT * FROM courts";
            dr = method.myReader(query, conn);
            while (dr.Read())
            {
                foreach (Days D in DayList)
                {
                    Courts court = new Courts();
                    court.CourtId = Convert.ToInt16(dr["CourtId"]);
                    court.Description = dr["Description"].ToString();
                    D.Courts.Add(court);
                }

            }
            query = "SELECT * FROM courttimes";
            dr = method.myReader(query, conn);
            while (dr.Read())
            {
                foreach (Days D in DayList)
                {
                    CourtTimes courtTime = new CourtTimes();
                    courtTime.CourtTimeId = Convert.ToInt16(dr["CourtTimeId"]);
                    courtTime.StartHour = Convert.ToInt16(dr["StartHour"]);
                    courtTime.Active = dr.GetBoolean(dr.GetOrdinal("Active"));
                    D.CourtTimes.Add(courtTime);
                }

            }
            

            
            try
            {

            HtmlGenericControl hourDiv = new HtmlGenericControl("div");
            hourDiv.Style.Add("background-color", "blue");
            hourDiv.Style.Add("padding", "10px");
            HtmlGenericControl courtDiv = new HtmlGenericControl("div");
            courtDiv.Style.Add("background-color", "red");
            courtDiv.Style.Add("margin", "5px");
            courtDiv.InnerHtml = "testarn";

            HtmlGenericControl court2Div = new HtmlGenericControl("div");
            court2Div.Style.Add("background-color", "green");
            court2Div.Style.Add("margin", "5px");
            court2Div.InnerHtml = "testarn2";

            hourDiv.Controls.Add(courtDiv); 
            hourDiv.Controls.Add(court2Div);

            scheduleDiv.Controls.Add(hourDiv);
            scheduleDiv.Controls.Add(hourDiv);
            }
            catch (HttpException ex)
            {
                Debug.WriteLine(ex);
            }


            
        }
    }
}