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
            //hfChosenCourts.Value = "0";
            //"8" är antalet dagar som metoden ska hämta, dynamiskt och kan ändras. 
            
            
            BuildSchedule(GetDayList(), 8);


        }
        public void BuildSchedule(List<Days> DayList, int noOfDays)
        {
            DateTime bookingDate = DateTime.Now.Date;
            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));

            List<Subscriptions> allSubscriptions = GetSubscriptionList();
            List<Reservations> allReservations = GetReservationsList();

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
                    if (i == todayNo && drawtimes == true)
                    {
                        drawtimes = false;

                        HtmlGenericControl staticTimesDiv = new HtmlGenericControl("div");
                        staticTimesDiv.Attributes.Add("id", "staticTimesDiv");

                        foreach (CourtTimes CT in DayList[1].CourtTimes)
                        {
                            HtmlGenericControl staticHourDiv = new HtmlGenericControl("div");
                            if (CT.StartHour < 10)
                            {
                                staticHourDiv.Attributes.Add("class", "staticHourDiv");
                                staticHourDiv.InnerHtml = "0" + CT.StartHour + ":00";
                            }
                            else
                            {
                                staticHourDiv.Attributes.Add("class", "staticHourDiv");
                                staticHourDiv.InnerHtml = CT.StartHour + ":00";
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
                            dayDiv.Style.Add("border-right", "1px solid #A0A0A0");
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
                            hourDiv.Attributes.Add("id", "" + D.DayId + CT.CourtTimeId + "");
                            hourDiv.Attributes.Add("class", "hourDivs");

                            string thisDayIsFullTime = "";

                            if (CT.StartHour < 10)
                            {
                                thisDayIsFullTime = "0" + CT.StartHour + ":00:00";

                            }
                            else
                            {
                                thisDayIsFullTime = CT.StartHour + ":00:00";
                            }
                            string shortTime = thisDayIsFullTime[0].ToString() + thisDayIsFullTime[1].ToString();
                            string hourBookingDivId = thisDayIsFullDate + "_" + shortTime;


                            HtmlGenericControl hourBookingDiv = new HtmlGenericControl("div");
                            hourBookingDiv.Attributes.Add("class", "hourBookingDiv");
                            hourBookingDiv.Attributes.Add("id", hourBookingDivId);
                            hourBookingDiv.InnerHtml = "<h3>Boka - " + thisDayIsFullDate + " "+shortTime+":00</h3><p>Klicka på önskade banor.</p><hr />";

                            foreach (Courts C in D.Courts)
                            {
                                HtmlGenericControl courtDiv = new HtmlGenericControl("div");
                                courtDiv.Attributes.Add("id", D.DayId + "-" + CT.CourtTimeId + "-" + C.CourtId + "-" + thisDayIsFullDate);
                                courtDiv.Attributes.Add("class", "courtDivs");

                                HtmlGenericControl pBookedBy = new HtmlGenericControl("p");
                                pBookedBy.InnerHtml = "Ledig tid";

                                bool booked = false;
                                bool reserved = false;

                                foreach (Subscriptions sub in allSubscriptions)
                                {
                                    if (sub.CourtId == C.CourtId && sub.CourtTimeId == CT.CourtTimeId && sub.DayId == D.DayId)
                                    {
                                        booked = true;
                                        if (Session["lip"] != null)
                                        {
                                            if (sub.MemberId == lip.member.MemberId)
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs subscribedCourt mySubscribedCourt masterTiptool ");
                                            }
                                            else
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs subscribedCourt masterTiptool");
                                            }
                                        }
                                        else
                                        {
                                            courtDiv.Attributes.Add("class", "courtDivs subscribedCourt masterTiptool");

                                        }
                                        courtDiv.InnerHtml = sub.FullMemberName;

                                        courtDiv.Attributes.Add("title", "Redan bokad av " + sub.FullMemberName);
                                        pBookedBy.InnerHtml = "Bokad av " + sub.FullMemberName;
                                    }
                                }

                                foreach (Reservations res in allReservations)
                                {
                                    if (res.CourtId == C.CourtId && res.StartDate == Convert.ToDateTime((thisDayIsFullDate + " " + thisDayIsFullTime)))
                                    {
                                        reserved = true;
                                        if (Session["lip"] != null)
                                        {
                                            if (res.MemberId == lip.member.MemberId)
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs reservedCourt myReservedCourt masterTiptool");
                                            }
                                            else
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs reservedCourt masterTiptool");
                                            }
                                        }
                                        else
                                        {
                                            courtDiv.Attributes.Add("class", "courtDivs reservedCourt masterTiptool");

                                        }
                                        courtDiv.Attributes.Add("title", "Redan bokad av " + res.FullMemberName);
                                        courtDiv.InnerHtml = res.FullMemberName;

                                        pBookedBy.InnerHtml = "Bokad av " + res.FullMemberName;
                                    }
                                }


                                if (Session["lip"] != null)
                                {
                                    string bookingDivId = thisDayIsFullDate + "_" + shortTime + "_" + C.CourtId.ToString();

                                    HiddenFieldWithClass hf = new HiddenFieldWithClass();
                                    hf.ID = "hf" + bookingDivId;
                                    hf.CssClass = "BookingHf";
                                    hf.Value = "0";

                                    HtmlGenericControl bookingDiv = new HtmlGenericControl("div");
                                    bookingDiv.Attributes.Add("id", bookingDivId);
                                    bookingDiv.Attributes.Add("class", "bookingDiv");
                                    bookingPageDivcc.Controls.Add(hf);

                                    HtmlGenericControl bookCourtDiv = new HtmlGenericControl("div");
                                    HtmlGenericControl courtImgDiv = new HtmlGenericControl("div");

                                    courtImgDiv.InnerHtml = "<img class='courtImg' src='Images/squashB" + C.CourtId.ToString() + "NoBackgroundFlor.svg' />";
                                    
                                    
                                    

                                    //courtDiv.Attributes.Add("onclick", "confirm_clicked('" + C.CourtId + "','" + lip.member.MemberId + "','" + thisDayIsFullDate + " " + thisDayIsFullTime + "','" + bookingDivId + ")");
                                    if (booked == false && reserved == false)
                                    {
                                        courtDiv.Attributes.Add("onclick", "OpenBookingOverlay('" + hourBookingDivId + "')");
                                        courtImgDiv.Attributes.Add("class", "courtImgDivFree");
                                        courtImgDiv.Attributes.Add("onclick", "chosenCourt('" + "hf" + bookingDivId + "','" + C.CourtId.ToString() + "','" + bookingDivId + "')");
                                        courtDiv.Attributes.Add("class", "courtDivs freeCourt masterTiptool");
                                        courtDiv.Attributes.Add("title", "Klicka för att boka Bana " + C.CourtId.ToString() + ", " + thisDayIs + " " + thisDayIsDate + "/" + thisDayIsMonth);
                                        bookCourtDiv.Controls.Add(pBookedBy);

                                    }
                                    else if (booked == true && reserved == false)
                                    {
                                        courtImgDiv.Attributes.Add("class", "courtImgDivBooked");
                                        courtImgDiv.InnerHtml = "<img class='courtImg CourtImgGray' src='Images/squashB" + C.CourtId.ToString() + "NoBackgroundFlor.svg' />";
                                        bookCourtDiv.Controls.Add(pBookedBy);
                                    }

                                    else if (booked == false && reserved == true)
                                    {
                                        courtImgDiv.Attributes.Add("class", "courtImgDivReserved");
                                        courtImgDiv.InnerHtml = "<img class='courtImg CourtImgGray' src='Images/squashB" + C.CourtId.ToString() + "NoBackgroundFlor.svg' />";
                                        bookCourtDiv.Controls.Add(pBookedBy);
                                    }


                                    bookCourtDiv.Controls.Add(courtImgDiv);

                                    bookingDiv.Controls.Add(bookCourtDiv);
                                    hourBookingDiv.Controls.Add(bookingDiv);

                                }
                                else if (booked == false && reserved == false)
                                {
                                    courtDiv.Attributes.Add("class", "courtDivs freeCourt");

                                }

                                //courtDiv.InnerHtml = D.DayId + "-" + CT.CourtTimeId + "-" + C.CourtId + " " + thisDayIsFullDate + " " + thisDayIsFullTime;
                                hourDiv.Controls.Add(courtDiv);
                            }
                            
                            Button btnBook = new Button();
                            btnBook.Text = "Boka";
                            btnBook.Attributes.Add("class", "btn btn-default book-btn");
                            btnBook.CommandArgument = hourBookingDivId;
                            btnBook.Click += btnBook_Click;

                            hourBookingDiv.Controls.Add(btnBook);

                            bookingOverlayMessage.Controls.Add(hourBookingDiv);
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

        public List<Reservations> GetReservationsList()
        {
            List<Reservations> reservationsList = new List<Reservations>();

            string query = "SELECT r.CourtId, r.MemberId, r.StartDate, r.HandledBy, r.ReservationType, u.Firstname, u.Surname FROM reservations r "
                         + "INNER JOIN members m ON m.MemberId = r.MemberId "
                         + "INNER JOIN users u ON u.UserId = m.UserId;";

            MySqlDataReader dr = method.myReader(query, conn);

            while (dr.Read())
            {
                Reservations r = new Reservations();
                r.CourtId = Convert.ToInt16(dr["CourtId"]);
                r.MemberId = Convert.ToInt16(dr["MemberId"]);
                r.StartDate = Convert.ToDateTime(dr["StartDate"]);
                //if (dr["HandledBy"] != DBNull.Value)
                //{
                //    r.HandledBy = Convert.ToInt16(dr["HandledBy"]);
                //}
                //else
                //{
                //    r.HandledBy = 0;
                //}
                if (dr["ReservationType"] != DBNull.Value)
                {
                    r.ReservationType = Convert.ToInt16(dr["ReservationType"]);
                }
                else
                {
                    r.ReservationType = 0;
                }
                r.FullMemberName = method.FixName(dr["Firstname"].ToString() + " " + dr["Surname"].ToString());
                reservationsList.Add(r);
            }
            return reservationsList;
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            List<int> courtIdList = new List<int>(); 

            Response.Write("<script>alert('" + btn.CommandArgument.ToString() +"') </script>");
            string startTime = btn.CommandArgument.ToString();
            string corrStartTime = "";

            foreach (char c in startTime)
            {
                if(c.ToString() == "_")
                {
                    corrStartTime += " ";
                }
                else
                {
                    corrStartTime += c;
                }
            }
            corrStartTime += ":00:00";

            //HiddenFieldWithClass a = new HiddenFieldWithClass();

            string query = "INSERT INTO reservations VALUES ";

            List<Control> controlList = new List<Control>();
            List<HiddenFieldWithClass> hfwcList = new List<HiddenFieldWithClass>();
            //foreach (HiddenFieldWithClass hf in this.Page.Controls)
            foreach(Control c in this.Page.Controls)
            {
                //controlList.Add(c);
                foreach(Control c2 in c.Controls)
                {
                    //controlList.Add(c2);
                    foreach(Control c3 in c2.Controls)
                    {
                        //controlList.Add(c3);
                        foreach(Control c4 in c3.Controls)
                        {
                            //controlList.Add(c4);
                            foreach (Control c5 in c4.Controls)
                            {
                                if(c5 is HiddenFieldWithClass)
                                {
                                    HiddenFieldWithClass a = (HiddenFieldWithClass)c5;
                                    if(a.Value != "0")
                                    {
                                        hfwcList.Add(a);
                                    }
                                }
                            }
                        }
                        
                        

                    }
                }

            }

            
            foreach(HiddenFieldWithClass hf in hfwcList)
            {
                query += "(" + Convert.ToInt16(hf.Value) + ", " + lip.member.MemberId + ", '" + Convert.ToDateTime(corrStartTime) + "', NULL, 1),";
            }



            char[] s = { ',' };
            string finalQuery = query.TrimEnd(s);
            

            MySqlCommand cmdInsertRes = new MySqlCommand(finalQuery + ";", conn);
            
            conn.Close();
            conn.Open();
            cmdInsertRes.ExecuteNonQuery();
            conn.Close();


            //TRY CATCH FINALLY
            Response.Redirect("Booking.aspx");
        }

    }
}