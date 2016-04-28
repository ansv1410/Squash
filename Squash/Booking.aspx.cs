﻿using System;
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
using System.Globalization;
using System.Threading;

namespace Squash
{
    public partial class Booking : System.Web.UI.Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();
        LoggedInPerson lip;
        protected void Page_Load(object sender, EventArgs e)
        {
            lip = (LoggedInPerson)Session["lip"];
            
            //"8" är antalet dagar som metoden ska hämta, dynamiskt och kan ändras. 
            BuildSchedule(GetDayList(),8);


        }
        public void BuildSchedule(List<Days> DayList, int noOfDays)
        {
            DateTime bookingDate = DateTime.Now.Date;
            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));

            List<Subscriptions> allSubscriptions = GetSubscriptionList();



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

                        foreach (CourtTimes CT in DayList[1].CourtTimes)
                        {
                            HtmlGenericControl staticHourDiv = new HtmlGenericControl("div");
                            if(CT.StartHour < 10)
                            {
                                staticHourDiv.Attributes.Add("class", "staticHourDiv");
                                staticHourDiv.InnerHtml = "0" + CT.StartHour +":00";
                            }
                            else
                            {
                                staticHourDiv.Attributes.Add("class", "staticHourDiv");
                                staticHourDiv.InnerHtml = CT.StartHour +":00";
                            }


                            staticTimesDiv.Controls.Add(staticHourDiv);
                        }
                        scheduleDiv.Controls.Add(staticTimesDiv);
                        }


                    }
                double d = 94;
                double n = Convert.ToDouble(noOfDays);
                    foreach (Days D in DayList)
                    {
                        if (D.DayId == i)
                        {
                            

                            HtmlGenericControl dayDiv = new HtmlGenericControl("div");
                            dayDiv.Attributes.Add("id", "day" + D.Description);
                            dayDiv.Attributes.Add("class", "dayDiv");
                            if (counter == noOfDays)
                            {
                                dayDiv.Style.Add("border-right", "1px solid black");
                            }
                            double dn = d / n;

                            string wid = dn.ToString() + "%";
                            string width = "";
                            foreach (Char c in wid)
                            {
                                if (c.ToString() == ",")
                                {
                                    width += ".";
                                }
                                else
                                {
                                    width += c;
                                }
                            }

                            dayDiv.Style.Add("width", width);

                            HtmlGenericControl staticDayDiv = new HtmlGenericControl("div");
                            staticDayDiv.Attributes.Add("class", "staticDayDiv");

                            string thisDayIs = method.FixName(DateTime.Now.AddDays(counter - 1).ToString("dddd", new CultureInfo("sv-SE")));
                            string thisDayIsDate = method.FixName(DateTime.Now.AddDays(counter - 1).ToString("%d", new CultureInfo("sv-SE")));
                            string thisDayIsMonth = DateTime.Now.AddDays(counter - 1).ToString("%M", new CultureInfo("sv-SE"));
                            staticDayDiv.InnerHtml = thisDayIs + "<br />" + thisDayIsDate + "/" + thisDayIsMonth;
                            string thisDayIsFullDate = DateTime.Now.AddDays(counter - 1).ToString("yyyy-MM-dd", new CultureInfo("sv-SE"));

                            dayDiv.Controls.Add(staticDayDiv);

                            foreach (CourtTimes CT in D.CourtTimes)
                            {
                                HtmlGenericControl hourDiv = new HtmlGenericControl("div");
                                hourDiv.Attributes.Add("id", "" + D.DayId + CT.CourtTimeId +"");
                                hourDiv.Attributes.Add("class", "hourDivs");

                                foreach (Courts C in D.Courts)
                                {
                                    HtmlGenericControl courtDiv = new HtmlGenericControl("div");
                                    courtDiv.Attributes.Add("id", D.DayId + "-" + CT.CourtTimeId + "-" + C.CourtId + "-" + thisDayIsFullDate);
                                    courtDiv.Attributes.Add("class", "courtDivs");
                                    string thisDayIsFullTime = "";

                                    if (CT.StartHour < 10)
                                    {
                                        thisDayIsFullTime = "0" + CT.StartHour + ":00:00";

                                    }
                                    else
                                    {
                                        thisDayIsFullTime = CT.StartHour + ":00:00";
                                    }


                                    bool booked = false;
                                    foreach (Subscriptions sub in allSubscriptions)
                                    {
                                        if(sub.CourtId == C.CourtId && sub.CourtTimeId == CT.CourtTimeId && sub.DayId == D.DayId)
                                        {
                                            booked = true;
                                            courtDiv.Attributes.Add("class", "courtDivs subscribedCourt");
                                            courtDiv.InnerHtml = sub.FullMemberName;
                                        }
                                    }

                                    if(Session["lip"] != null && booked == false)
                                    {
                                        courtDiv.Attributes.Add("onclick", "confirm_clicked('" + C.CourtId + "','" + lip.member.MemberId + "','" + thisDayIsFullDate + " " + thisDayIsFullTime + "')");
                                    }

                                    //courtDiv.InnerHtml = D.DayId + "-" + CT.CourtTimeId + "-" + C.CourtId + " " + thisDayIsFullDate + " " + thisDayIsFullTime;
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
            conn.Close();

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
            conn.Close();
            return DayList;
        }

        public List<Subscriptions> GetSubscriptionList()
        {
            List<Subscriptions> subscriptionList = new List<Subscriptions>();

            string query = "SELECT s.CourtID, s.CourtTimeId, s.DayId, s.MemberId, u.Firstname, u.Surname FROM  subscriptions s "
                         + "JOIN members m "
                         + "ON m.MemberId = s.MemberId "
                         + "JOIN users u "
                         + "ON u.UserID = m.UserID;";

            MySqlDataReader dr = method.myReader(query, conn);

            while (dr.Read())
            {
                Subscriptions s = new Subscriptions();
                s.CourtId = Convert.ToInt16(dr["CourtId"]);
                s.CourtTimeId = Convert.ToInt16(dr["CourtTimeId"]);
                s.DayId = Convert.ToInt16(dr["DayId"]);
                s.MemberId = Convert.ToInt16(dr["MemberId"]);
                s.FullMemberName = method.FixName(dr["Firstname"].ToString() + " " + dr["Surname"].ToString());
                subscriptionList.Add(s);
            }

            return subscriptionList;
        }

    }
}