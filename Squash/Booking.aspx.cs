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
            //"8" är antalet dagar som metoden ska hämta, dynamiskt och kan ändras. 
            BuildSchedule(GetDayList(),8);
            
        }
        public void BuildSchedule(List<Days> DayList, int noOfDays)
        {
            string todayIs = DateTime.Now.DayOfWeek.ToString();
            int todayNo = 0;
            foreach (Days D in DayList)
            {
                if (D.Description == todayIs)
                {
                    todayNo = D.DayId;
                }
            }

            
            int counter = 0;
            bool drawtimes = true;
            for (int i = todayNo; i <= noOfDays; i++)
            {
                counter++;
                if (counter > noOfDays)
                {
                    break;
                }
                if (counter <= noOfDays)
                {
                    if(i == todayNo && drawtimes == true)
                    {
                        drawtimes = false;
       
                        HtmlGenericControl staticTimesDiv = new HtmlGenericControl("div");
                        staticTimesDiv.Attributes.Add("id", "staticTimesDiv");
                        string width = ((740 / (noOfDays + 1)) - 1).ToString() + "px";
                        staticTimesDiv.Style.Add("width", width);
                        staticTimesDiv.Style.Add("float", "left");
                        staticTimesDiv.Style.Add("border", "1px solid black");

                        foreach (CourtTimes CT in DayList[1].CourtTimes)
                        {
                            HtmlGenericControl staticHourDiv = new HtmlGenericControl("div");
                            if(CT.StartHour < 10)
                            {
                                staticHourDiv.InnerHtml = "0" + CT.StartHour +":00";
                            }
                            else
                            {
                                staticHourDiv.InnerHtml = CT.StartHour +":00";
                            }


                            staticTimesDiv.Controls.Add(staticHourDiv);
                        }
                        scheduleDiv.Controls.Add(staticTimesDiv);
                        }


                    }

                    foreach (Days D in DayList)
                    {
                        if (D.DayId == i)
                        {
                            HtmlGenericControl dayDiv = new HtmlGenericControl("div");
                            dayDiv.Attributes.Add("id", "day" + D.Description);
                            string width = ((740/(noOfDays +1)) - 1).ToString() +"px";
                            dayDiv.Style.Add("width", width);
                            dayDiv.Style.Add("float", "left");
                            dayDiv.Style.Add("border", "1px solid black");

                            HtmlGenericControl staticDayDiv = new HtmlGenericControl("div");
                            staticDayDiv.Style.Add("margin", "5px");
                            staticDayDiv.InnerHtml = D.Description;

                            dayDiv.Controls.Add(staticDayDiv);

                            foreach (CourtTimes CT in D.CourtTimes)
                            {
                                HtmlGenericControl hourDiv = new HtmlGenericControl("div");

                                foreach (Courts C in D.Courts)
                                {
                                    HtmlGenericControl courtDiv = new HtmlGenericControl("div");
                                    courtDiv.Style.Add("background-color", "green");
                                    courtDiv.Style.Add("margin", "5px");
                                    courtDiv.InnerHtml = "testarn";
                                    hourDiv.Controls.Add(courtDiv);
                                }
                                dayDiv.Controls.Add(hourDiv);
                            }
                            scheduleDiv.Controls.Add(dayDiv);
                        }
                    }
                
                if (i == 7)
                {
                    i = 0;
                }
            }
    





            //foreach (Days D in DayList)
            //{
            //    HtmlGenericControl dayDiv= new HtmlGenericControl("div");
            //    dayDiv.Attributes.Add("id", "day" + D.Description);

            //    foreach (CourtTimes CT in D.CourtTimes)
            //    {
            //        HtmlGenericControl hourDiv = new HtmlGenericControl("div");
            //        hourDiv.Style.Add("background-color", "blue");
            //        hourDiv.Style.Add("padding", "10px");

            //        foreach(Courts C in D.Courts)
            //        {
            //            HtmlGenericControl courtDiv = new HtmlGenericControl("div");
            //            courtDiv.Style.Add("background-color", "red");
            //            courtDiv.Style.Add("margin", "5px");
            //            courtDiv.InnerHtml = "testarn";
            //            hourDiv.Controls.Add(courtDiv); 
            //        }
            //        dayDiv.Controls.Add(hourDiv);
            //    }
            //    scheduleDiv.Controls.Add(dayDiv);
                
            //}
        }
        public List<Days> GetDayList()
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
            query = "SELECT * FROM courttimes WHERE Active = 1";
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
            return DayList;
        }
    }
}