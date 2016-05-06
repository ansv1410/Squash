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

            if(Session["lip"] != null)
            {
                ShowMyReservations();
            }
            
            BuildSchedule(GetDayList(), 8);


        }
        public void BuildSchedule(List<Days> DayList, int noOfDays)
        {
            DateTime bookingDate = DateTime.Now.Date;
            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));

            List<Subscriptions> allSubscriptions = GetSubscriptionList();
            List<Reservations> allReservations = GetReservationsList();
            List<Companies> allCompanies = GetCompanyList();
            List<MemberCompany> allMemberCompany = GetMemberCompanyList();

            List<HtmlGenericControl> daySelectorList = new List<HtmlGenericControl>();

            int counter = 0;
            bool drawtimes = true;
            for (int i = todayNo; i <= noOfDays; i++)
            {
                counter++;
                HtmlGenericControl daySelector = new HtmlGenericControl("div");
                daySelector.Attributes.Add("id", counter.ToString() + "_daySelector");
                daySelector.Attributes.Add("class", "daySelector");
                daySelector.Attributes.Add("onclick", "ShowMobileDayDiv('" + counter.ToString() + "_day')");
                
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
                        dayDiv.Attributes.Add("id", counter.ToString() +"_day");
                        dayDiv.Attributes.Add("class", "dayDiv");
                        if (counter == noOfDays)
                        {
                            dayDiv.Style.Add("border-right", "1px solid #A0A0A0");
                        }
                        double dn = d / n;
                        double dn2 = 100 / n;

                        string wid = dn.ToString() + "%";
                        string wid2 = dn2.ToString() + "%";
                        string width = "";
                        string width2 = "";
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
                        foreach (Char c in wid2)
                        {
                            if (c.ToString() == ",")
                            {
                                width2 += ".";
                            }
                            else
                            {
                                width2 += c;
                            }
                        }
                        hfWidthOfDaySelectors.Value = width2;
                        hfWidthOfDayDivs.Value = width;

                        //dayDiv.Style.Add("width", width);
                        //dayDiv.Style.Add("width", "@media screen and (min-width: 768px){width:'" + width + "'%;};");
                        //dayDiv.Attributes["media"] = "screen and(min-width: 768px){width:'" + width + "'%;};";
                        //dayDiv.Style.Add("media", "screen and(min-width: 768px){width:"+width+")");
                        //dayDiv.Style.Add("width", "@media screen and (min-width: 768px){width:"+width+";}");


                        HtmlGenericControl staticDayDiv = new HtmlGenericControl("div");
                        staticDayDiv.Attributes.Add("class", "staticDayDiv");

                        string thisDayIs = method.FixName(DateTime.Now.AddDays(counter - 1).ToString("dddd", new CultureInfo("sv-SE")));
                        string thisDayIsDate = method.FixName(DateTime.Now.AddDays(counter - 1).ToString("%d", new CultureInfo("sv-SE")));
                        string thisDayIsMonth = DateTime.Now.AddDays(counter - 1).ToString("%M", new CultureInfo("sv-SE"));
                        //daySelector.InnerHtml = thisDayIs + "<br />" + thisDayIsDate + "/" + thisDayIsMonth;
                        daySelector.InnerHtml = thisDayIsDate + "/" + thisDayIsMonth;
                        staticDayDiv.InnerHtml = thisDayIs + "<br />" + thisDayIsDate + "/" + thisDayIsMonth;
                        string thisDayIsFullDate = DateTime.Now.AddDays(counter - 1).ToString("yyyy-MM-dd", new CultureInfo("sv-SE"));

                        selectorDiv.Controls.Add(daySelector);
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

                                        foreach (MemberCompany mc in allMemberCompany)
                                        {
                                            if (mc.MemberId == sub.MemberId)
                                            {
                                                foreach (Companies c in allCompanies)
                                                {
                                                    if (mc.CompanyId == c.Id)
                                                    {
                                                        courtDiv.Attributes.Add("title", "Redan bokad av " + c.Name);
                                                        courtDiv.InnerHtml = c.Name;

                                                        pBookedBy.InnerHtml = "Bokad av " + c.Name;
                                                    }
                                                }
                                            }
                                        }
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
                                        
                                        foreach(MemberCompany mc in allMemberCompany)
                                        {
                                            if(mc.MemberId == res.MemberId)
                                            {
                                                foreach(Companies c in allCompanies)
                                                {
                                                    if(mc.CompanyId == c.Id)
                                                    {
                                                        courtDiv.Attributes.Add("title", "Redan bokad av " + c.Name);
                                                        courtDiv.InnerHtml = c.Name;

                                                        pBookedBy.InnerHtml = "Bokad av " + c.Name;
                                                    }
                                                }
                                            }
                                        }


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

                                    courtImgDiv.InnerHtml = "<img class='courtImg' src='Images/squashB" + C.CourtId.ToString() + "lightgreen.svg' />";
                                    
                                    
                                    

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
                                        courtImgDiv.InnerHtml = "<img class='courtImg CourtImgGray' src='Images/squashB" + C.CourtId.ToString() + "lightcoral.svg' />";
                                        bookCourtDiv.Controls.Add(pBookedBy);
                                    }

                                    else if (booked == false && reserved == true)
                                    {
                                        courtImgDiv.Attributes.Add("class", "courtImgDivReserved");
                                        courtImgDiv.InnerHtml = "<img class='courtImg CourtImgGray' src='Images/squashB" + C.CourtId.ToString() + "lightblue.svg' />";
                                        bookCourtDiv.Controls.Add(pBookedBy);
                                    }


                                    bookCourtDiv.Controls.Add(courtImgDiv);

                                    bookingDiv.Controls.Add(bookCourtDiv);
                                    hourBookingDiv.Controls.Add(bookingDiv);

                                }
                                else if (booked == false && reserved == false)
                                {
                                    courtDiv.Attributes.Add("class", "courtDivs freeCourt masterTiptool");
                                    courtDiv.Attributes.Add("title", "Logga in eller bli medlem för att boka.");

                                }

                                //courtDiv.InnerHtml = D.DayId + "-" + CT.CourtTimeId + "-" + C.CourtId + " " + thisDayIsFullDate + " " + thisDayIsFullTime;
                                int thisHour = Convert.ToInt16(DateTime.Now.Hour);
                                string nowDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("sv-SE"));

                                if (thisDayIsFullDate != nowDate)
                                {
                                    hourDiv.Controls.Add(courtDiv);
                                }
                                else
                                {
                                    if (thisHour <= CT.StartHour)
                                    {
                                        hourDiv.Controls.Add(courtDiv);
                                    }
                                    else
                                    {
                                        hourDiv.Attributes.Add("class", "greyDiv");
                                    }
                                }
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


        //HÄMTAR ALLA DAGAR OCH LÄGRAR DESSA I EN LISTA INNEHÅLLANDE DAGID, DAGENS NAMN, EN LISTA AV BANOR, EN LISTA AV BANTIDER
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

        //HÄMTAR ALLA ABONNEMANGSBOKNINGAR OCH LAGRAR DESSA I EN LISTA
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

        //HÄMTAR ALLA RESERVATIONER OCH LAGRAR DESSA I EN LISTA
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

        //HÄMTAR ALLA FÖRETAG OCH LAGRAR DESSA I EN LISTA.
        public List<Companies> GetCompanyList()
        {
            List<Companies> companiesList = new List<Companies>();
            string query = "SELECT Id, Name FROM companies";

            MySqlDataReader dr = method.myReader(query, conn);

            while(dr.Read())
            {
                Companies c = new Companies();
                c.Id = Convert.ToInt16(dr["Id"]);
                c.Name = dr["Name"].ToString();

                companiesList.Add(c);
            }
            conn.Close();

            return companiesList;

        }
        
        //HÄMTAR ALLA RADER FRÅN MEMBERCOMPANY OCH LAGRAR DESSA I EN LISTA.
        public List<MemberCompany> GetMemberCompanyList()
        {
            List<MemberCompany> memberCompanyList = new List<MemberCompany>();
            string query = "SELECT mc.MemberId, mc.CompanyId FROM membercompany mc";

            MySqlDataReader dr = method.myReader(query, conn);

            while(dr.Read())
            {
                MemberCompany mc = new MemberCompany();
                mc.MemberId = Convert.ToInt16(dr["MemberId"]);
                mc.CompanyId = Convert.ToInt16(dr["CompanyId"]);
                memberCompanyList.Add(mc);
            }
            conn.Close();

            return memberCompanyList;
        }


        public void ShowMyReservations()
        {
            List<Tuple<Reservations, Courts, ReservationTypes>> bookingInfoList = new List<Tuple<Reservations, Courts, ReservationTypes>>();

            string query = "SELECT r.StartDate, c.Description AS courtName, c.CourtId, rt.Description AS resType FROM reservations r "
                           + "INNER JOIN courts c ON c.CourtId = r.CourtId "
                           + "INNER JOIN reservationtypes rt ON rt.ReservationTypeId = r.ReservationType "
                           + "WHERE r.MemberId = '" + lip.member.MemberId + "' AND StartDate > NOW() ORDER BY StartDate;";

            MySqlDataReader dr = method.myReader(query, conn);

            while (dr.Read())
            {
                Reservations r = new Reservations();
                r.StartDate = Convert.ToDateTime(dr["StartDate"]);
                //if (dr["ReservationType"] != DBNull.Value)
                //{
                //    r.ReservationType = Convert.ToInt16(dr["ReservationType"]);
                //}
                //else
                //{
                //    r.ReservationType = 0;
                //}

                Courts c = new Courts();
                c.CourtId = Convert.ToInt16(dr["CourtId"]);
                c.Description = dr["courtName"].ToString();


                ReservationTypes rt = new ReservationTypes();
                rt.Description = dr["resType"].ToString();

                Tuple<Reservations, Courts, ReservationTypes> tupleList = new Tuple<Reservations, Courts, ReservationTypes>(r, c, rt);
                bookingInfoList.Add(tupleList);

                //loggedInReservationsList.Add(r);
            }

            //List<HtmlGenericControl> trList = new List<HtmlGenericControl>();
            List<HtmlTableRow> trList = new List<HtmlTableRow>();
            //foreach (HtmlGenericControl t in trList)
            //{
            //}
            //for (int x = 0; x < 5; x++)
            //{
            //    HtmlGenericControl td = new HtmlGenericControl("td");
            //    td.Attributes.Add("class", "myBookingsTD");
            //    td.InnerHtml = "TD";

            //}
            //foreach (Reservations r in loggedInReservationsList)
            //{
            //    HtmlTableRow tr = new HtmlTableRow();
            //    tr.Attributes.Add("class", "myBookingsTR");
            //    trList.Add(tr);

            //    foreach (HtmlTableRow t in trList)
            //    {
            //        HtmlTableCell td = new HtmlTableCell();
            //        td.InnerText = "HEJ";
            //    }

            //    //for (int x = 0; x < 5; x++)
            //    //{

            //    //}

            //    bookingsTable.Rows.Add(tr);
            //}

            foreach (Tuple<Reservations, Courts, ReservationTypes> t in bookingInfoList)
            {
                HtmlTableRow tr = new HtmlTableRow();
                tr.Attributes.Add("class", "myBookingsTR");
                trList.Add(tr);

                for (int x = 0; x < 5; x++)
                {
                    HtmlTableCell td = new HtmlTableCell();
                    td.Attributes.Add("class", "myBookingsTD");
                    if (x == 0)
                    {
                        td.InnerText = t.Item1.StartDate.ToShortDateString();
                    }
                    if (x == 1)
                    {
                        td.InnerText = t.Item1.StartDate.ToShortTimeString();
                    }
                    if (x == 2)
                    {
                        td.InnerText = t.Item2.Description.ToString();
                    }
                    if (x == 3)
                    {
                        td.InnerText = t.Item3.Description.ToString();
                    }
                    if (x == 4)
                    {
                        td.InnerText = "1234";
                    }

                    tr.Controls.Add(td);
                }
                bookingsTable.Rows.Add(tr);
                myBookingsDiv.Visible = true;
            }


            //foreach (Reservations r in loggedInReservationsList)
            //{
            //    HtmlTableRow tr = new HtmlTableRow();
            //    tr.Attributes.Add("class", "myBookingsTR");
            //    trList.Add(tr);

            //    for (int x = 0; x < 5; x++)
            //    {
            //        HtmlTableCell td = new HtmlTableCell();
            //        td.Attributes.Add("class", "myBookingsTD");
            //        if (x == 0)
            //        {
            //            td.InnerText = r.StartDate.Date.ToString();
            //        }
            //        if (x == 1)
            //        {
            //            td.InnerText = r.StartDate.TimeOfDay.ToString();
            //        }
            //        if (x == 2)
            //        {
            //            td.InnerText = r.CourtId.ToString();
            //        }
            //        if (x == 3)
            //        {
            //            td.InnerText = "Handled By";
            //        }
            //        if (x == 4)
            //        {
            //            td.InnerText = r.ReservationType.ToString();
            //        }

            //        tr.Controls.Add(td);
            //    }
            //    bookingsTable.Rows.Add(tr);

            //}


            /*foreach (Reservations r in loggedInReservationsList)
                Add new row to table                                     
                foreach row add 5 cells
                for int i = 0; i <5; i++
             * if i == 0 add courtID
             * if i == 1 add memberId
             * if i == 2 add date
             * typ så.
             */






            //HtmlTableRow row = new HtmlTableRow();
            //HtmlTableCell cell = new HtmlTableCell();
            //cell.InnerText = "Datum";
            //row.Cells.Add(cell);
            //bookingsTable.Rows.Add(row);






            //HtmlGenericControl table = new HtmlGenericControl("table");

            //HtmlGenericControl th = new HtmlGenericControl("th");
            //th.Attributes.Add("class", "myBookingsTH");
            //th.InnerHtml = "Datum";
            //table.Controls.Add(th);

            //HtmlGenericControl th2 = new HtmlGenericControl("th");
            //th2.Attributes.Add("class", "myBookingsTH");
            //th2.InnerHtml = "Tid";
            //table.Controls.Add(th2);

            //HtmlGenericControl th3 = new HtmlGenericControl("th");
            //th3.Attributes.Add("class", "myBookingsTH");
            //th3.InnerHtml = "Bana";
            //table.Controls.Add(th3);

            //HtmlGenericControl th4 = new HtmlGenericControl("th");
            //th4.Attributes.Add("class", "myBookingsTH");
            //th4.InnerHtml = "Pris";
            //table.Controls.Add(th4);

            //HtmlGenericControl th5 = new HtmlGenericControl("th");
            //th5.Attributes.Add("class", "myBookingsTH");
            //th5.InnerHtml = "PIN-kod";
            //table.Controls.Add(th5);


            //myBookingsDiv.Controls.Add(table);

            //foreach(Reservations r in loggedInReservationsList)
            //{
            //    HtmlGenericControl tr = new HtmlGenericControl("tr");
            //    tr.Attributes.Add("class", "myBookingsTR");

            //    //foreach row. add 5 tds
            //}
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
                //IF MEMBERTYPE == 3
                //AND IF lip.member.MemberId INTE HAR NÅGON TID DENNA VECKA
                //RESERVATIONTYPE = 3

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