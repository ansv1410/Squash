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
        bool showBookingMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            lip = (LoggedInPerson)Session["lip"];
            //hfChosenCourts.Value = "0";
            //"8" är antalet dagar som metoden ska hämta, dynamiskt och kan ändras. 
            hfNoOfClickedCourts.Value = "0";
            if(Session["lip"] != null)
            {
                HtmlTable table = method.MyBookingsTable(lip);
                if (table != null)
                {
                    bool f = false;
                    HtmlInputButton btnCancelRes = new HtmlInputButton();
                    btnCancelRes.Attributes.Add("id", "btnCancelRes");
                    btnCancelRes.Attributes.Add("type", "button");
                    btnCancelRes.Attributes.Add("onclick", "OpenCancelReservationOverlay('false')");
                    btnCancelRes.Attributes.Add("class", "btn btn-default");
                    btnCancelRes.Attributes.Add("value", "Avboka ▲");
                    btnCancelRes.Attributes.Add("disabled", "disabled");

                    myBookingsDiv.Controls.Add(table);

                    myBookingsDiv.Controls.Add(btnCancelRes);

                    myBookingsDiv.Visible = true;
                }
                //ShowMyReservations();

                //ShowMySubscriptions();
                showBookingMessage = (bool)Session["showBookingMessage"];
              
                if (showBookingMessage)
                {
                    if ((Session["bookingMessage"].ToString())[3].ToString() == "D")
                    {
                        bookingConfirmationMessage.Visible = true;
                        bookingConfirmationMessage.InnerHtml = Session["bookingMessage"].ToString();
                    }
                    else
                    {
                        bookingErrorMessage.Visible = true;
                        bookingErrorMessage.InnerHtml = Session["bookingMessage"].ToString();
                    }

                    //bookingMessage.Visible = true;
                    //bookingMessage.InnerText = (string)Session["bookingMessage"];

                    Session["showBookingMessage"] = false;
                }


            }

            BuildSchedule(8);

        }
        /// <summary>
        /// Loopar all bokningsdata data och genererar ett dynamiskt schema för det valda antalet dagar(nOfDays)
        /// </summary>
        /// <param name="noOfDays">Antalet dagar att visa</param>
        public void BuildSchedule(int noOfDays)
        {
            DateTime bookingDate = DateTime.Now.Date;
            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));

            List<Days> DayList = GetDayList();
            List<Subscriptions> allSubscriptions = GetSubscriptionList();
            List<Reservations> allReservations = GetReservationsList();
            List<Companies> allCompanies = GetCompanyList();
            List<MemberCompany> allMemberCompany = GetMemberCompanyList();

            List<HtmlGenericControl> daySelectorList = new List<HtmlGenericControl>();

            int counter = 0;
            bool drawtimes = true;

            //Startar loopen på dagens "todayNo", ex. Måndag = 1 eller Fredag = 5.
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
                    //Loopar ut den vänstra titelkolumnen för aktiva starttider som hämtats från servern.
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

                //Loopar veckans dagar enligt de dagar som specificerats i databasen skapar element för dessa.
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
                        
                        hfWidthOfDaySelectors.Value = method.DivideWidth(100, Convert.ToDouble(noOfDays));
                        hfWidthOfDayDivs.Value = method.DivideWidth(94, Convert.ToDouble(noOfDays));

                        HtmlGenericControl staticDayDiv = new HtmlGenericControl("div");
                        staticDayDiv.Attributes.Add("class", "staticDayDiv");

                        string thisDayIs = method.FixName(DateTime.Now.AddDays(counter - 1).ToString("dddd", new CultureInfo("sv-SE")));
                        string thisDayIsDate = method.FixName(DateTime.Now.AddDays(counter - 1).ToString("%d", new CultureInfo("sv-SE")));
                        string thisDayIsMonth = DateTime.Now.AddDays(counter - 1).ToString("%M", new CultureInfo("sv-SE"));
                        string shortDayIs = thisDayIs.Substring(0, 3);
                        daySelector.InnerHtml = shortDayIs + "<br />" + thisDayIsDate + "/" + thisDayIsMonth;
                        staticDayDiv.InnerHtml = thisDayIs + "<br />" + thisDayIsDate + "/" + thisDayIsMonth;
                        string thisDayIsFullDate = DateTime.Now.AddDays(counter - 1).ToString("yyyy-MM-dd", new CultureInfo("sv-SE"));

                        selectorDiv.Controls.Add(daySelector);
                        dayDiv.Controls.Add(staticDayDiv);

                        //Loopar alla starttider för den aktuella dagen och skapar element för dessa.
                        foreach (CourtTimes CT in D.CourtTimes)
                        {
                            HtmlGenericControl hourDiv = new HtmlGenericControl("div");
                            hourDiv.Attributes.Add("id", "" + D.DayId + CT.CourtTimeId + "");
                            hourDiv.Attributes.Add("class", "hourDivs");

                            string thisDayIsFullTime = method.ConvertToFullTime(CT.StartHour, false);
                            string shortTime = method.ConvertToFullTime(CT.StartHour, true);
                            string hourBookingDivId = thisDayIsFullDate + "_" + shortTime;

                            HtmlGenericControl pTime = new HtmlGenericControl("p");
                            pTime.Attributes.Add("class", "pTime");
                            pTime.InnerHtml = shortTime+":00";

                            hourDiv.Controls.Add(pTime);

                            HtmlGenericControl hourBookingDiv = new HtmlGenericControl("div");
                            hourBookingDiv.Attributes.Add("class", "hourBookingDiv");
                            hourBookingDiv.Attributes.Add("id", hourBookingDivId);
                            hourBookingDiv.InnerHtml = "<h3>Boka - " + thisDayIsFullDate + " "+shortTime+":00</h3><p>Klicka på önskade banor.</p><hr />";

                            HtmlGenericControl bookingDescriptionDiv = new HtmlGenericControl("div");
                            bookingDescriptionDiv.Attributes.Add("id", "bookingDescriptionDiv"+CT.CourtTimeId);
                            bookingDescriptionDiv.Attributes.Add("class", "bookingDescriptionDiv");
                            bookingDescriptionDiv.Attributes.Add("runat", "server");

                            hourBookingDiv.Controls.Add(bookingDescriptionDiv);

                            //Loopar alla banor för den aktuella starttiden och genererar element för dessa.
                            foreach (Courts C in D.Courts)
                            {
                                HtmlGenericControl descriptionDiv = new HtmlGenericControl("div");
                                descriptionDiv.Attributes.Add("class", "descriptionDiv");


                                HtmlGenericControl courtDiv = new HtmlGenericControl("div");
                                courtDiv.Attributes.Add("id", D.DayId + "-" + CT.CourtTimeId + "-" + C.CourtId + "-" + thisDayIsFullDate);
                                courtDiv.Attributes.Add("class", "courtDivs B"+C.CourtId);

                                HtmlGenericControl pBookedBy = new HtmlGenericControl("p");
                                pBookedBy.InnerHtml = "Ledig tid";

                                bool subscribed = false;
                                bool reserved = false;

                                //Loopar och jämför varje bana+tid och sätter ev. bokningstatus För abonnemangstider.
                                foreach (Subscriptions sub in allSubscriptions)
                                {
                                    if (sub.CourtId == C.CourtId && sub.CourtTimeId == CT.CourtTimeId && sub.DayId == D.DayId)
                                    {
                                        subscribed = true;
                                        if (Session["lip"] != null)
                                        {
                                            if (sub.MemberId == lip.member.MemberId)
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs subscribedCourt mySubscribedCourt masterTiptool B" + C.CourtId);
                                            }
                                            else
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs subscribedCourt masterTiptool B" + C.CourtId);
                                            }
                                        }
                                        else
                                        {
                                            courtDiv.Attributes.Add("class", "courtDivs subscribedCourt masterTiptool B" + C.CourtId);

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

                                //Loopar och jämför varje bana+tid och sätter ev. bokningstatus För strötider.
                                foreach (Reservations res in allReservations)
                                {
                                    if (res.CourtId == C.CourtId && res.StartDate == Convert.ToDateTime((thisDayIsFullDate + " " + thisDayIsFullTime)))
                                    {
                                        reserved = true;
                                        if (Session["lip"] != null)
                                        {
                                            if (res.MemberId == lip.member.MemberId)
                                            {
                                                DateTime dateOfBooking = Convert.ToDateTime(res.StartDate.ToShortDateString());
                                                string dayOfWeek = dateOfBooking.ToString("dddd", new CultureInfo("sv-SE"));
                                                string shortDayName = method.FixName(dayOfWeek.Substring(0, 3));
                                                string dateOfDate = dateOfBooking.ToString("%d", new CultureInfo("sv-SE"));
                                                string monthNumber = dateOfBooking.ToString("%M", new CultureInfo("sv-SE"));

                                                string myResValue = C.Description +" "+ shortDayName + " " + dateOfDate + "/" + monthNumber + " " + shortTime + ":00";

                                                string cancelParameter = "cb_" + C.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime;
                                                courtDiv.Attributes.Add("onclick", "CancelThisRes('"+cancelParameter+"', '"+myResValue+"')");




                                                courtDiv.Attributes.Add("class", "courtDivs reservedCourt myReservedCourt masterTiptool B" + C.CourtId);
                                            }
                                            else
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs reservedCourt masterTiptool B" + C.CourtId);
                                            }
                                        }
                                        else
                                        {
                                            courtDiv.Attributes.Add("class", "courtDivs reservedCourt masterTiptool B" + C.CourtId);

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

                                //Om användaren är inloggad
                                if (Session["lip"] != null)
                                {

                                    string bookingDivId = thisDayIsFullDate + "_" + shortTime + "_" + C.CourtId.ToString();

                                    //"hf" associeras till den klickbara div som symboliserar den bana och tid som ska bokas, via IDt hf + bookingDivId. Värdet initieras till 0 och sätts till 1 vid klick i JavaScript-funktionen "chosenCourt"
                                    HiddenFieldWithClass hf = new HiddenFieldWithClass();
                                    hf.ID = "hf" + bookingDivId;
                                    hf.CssClass = "BookingHf";
                                    hf.Value = "0";


                                    //Dessa element läggs till i bokningsoverlayen som visas efter klick på ledig bana.
                                    //---------------------------------------
                                    HtmlGenericControl bookingDiv = new HtmlGenericControl("div");
                                    bookingDiv.Attributes.Add("id", bookingDivId);
                                    bookingDiv.Attributes.Add("class", "bookingDiv");
                                    bookingPageDivcc.Controls.Add(hf);

                                    HtmlGenericControl bookCourtDiv = new HtmlGenericControl("div");
                                    HtmlGenericControl courtImgDiv = new HtmlGenericControl("div");

                                    courtImgDiv.InnerHtml = "<img class='courtImg' src='Images/squashB" + C.CourtId.ToString() + "lightgreen.svg' />";
                                    //----------------------------------------
                                    
                                    

                                    if (subscribed == false && reserved == false)
                                    {
                                        courtDiv.Attributes.Add("onclick", "OpenBookingOverlay('" + hourBookingDivId + "')");
                                        courtImgDiv.Attributes.Add("class", "courtImgDivFree");
                                        courtImgDiv.Attributes.Add("onclick", "chosenCourt('" + "hf" + bookingDivId + "','" + C.CourtId.ToString() + "','" + bookingDivId + "')");
                                        courtDiv.Attributes.Add("class", "courtDivs freeCourt masterTiptool B" + C.CourtId);
                                        courtDiv.Attributes.Add("title", "Klicka för att boka Bana " + C.CourtId.ToString() + ", " + thisDayIs + " " + thisDayIsDate + "/" + thisDayIsMonth);
                                        descriptionDiv.Controls.Add(pBookedBy);

                                    }
                                    else if (subscribed == true && reserved == false)
                                    {
                                        courtImgDiv.Attributes.Add("class", "courtImgDivBooked");
                                        courtImgDiv.InnerHtml = "<img class='courtImg CourtImgGray' src='Images/squashB" + C.CourtId.ToString() + "lightcoral.svg' />";
                                        descriptionDiv.Controls.Add(pBookedBy);
                                    }

                                    else if (subscribed == false && reserved == true)
                                    {
                                        courtImgDiv.Attributes.Add("class", "courtImgDivReserved");
                                        courtImgDiv.InnerHtml = "<img class='courtImg CourtImgGray' src='Images/squashB" + C.CourtId.ToString() + "lightblue.svg' />";
                                        descriptionDiv.Controls.Add(pBookedBy);
                                    }
                                    bookingDescriptionDiv.Controls.Add(descriptionDiv);

                                    bookCourtDiv.Controls.Add(courtImgDiv);

                                    bookingDiv.Controls.Add(bookCourtDiv);
                                    hourBookingDiv.Controls.Add(bookingDiv);

                                }
                                else if (subscribed == false && reserved == false)
                                {
                                    courtDiv.Attributes.Add("class", "courtDivs freeCourt masterTiptool B" + C.CourtId);
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
                            btnBook.Attributes.Add("disabled", "disabled");
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


        //public void ShowMyReservations()
        //{
        //    List<Tuple<Reservations, Courts, ReservationTypes>> bookingInfoList = new List<Tuple<Reservations, Courts, ReservationTypes>>();

        //    string query = "SELECT r.StartDate, c.Description AS courtName, c.CourtId, rt.Description AS resType FROM reservations r "
        //                   + "INNER JOIN courts c ON c.CourtId = r.CourtId "
        //                   + "INNER JOIN reservationtypes rt ON rt.ReservationTypeId = r.ReservationType "
        //                   + "WHERE r.MemberId = '" + lip.member.MemberId + "' AND StartDate > NOW() ORDER BY StartDate;";

        //    MySqlDataReader dr = method.myReader(query, conn);

        //    while (dr.Read())
        //    {
        //        Reservations r = new Reservations();
        //        r.StartDate = Convert.ToDateTime(dr["StartDate"]);
        //        //if (dr["ReservationType"] != DBNull.Value)
        //        //{
        //        //    r.ReservationType = Convert.ToInt16(dr["ReservationType"]);
        //        //}
        //        //else
        //        //{
        //        //    r.ReservationType = 0;
        //        //}

        //        Courts c = new Courts();
        //        c.CourtId = Convert.ToInt16(dr["CourtId"]);
        //        c.Description = dr["courtName"].ToString();


        //        ReservationTypes rt = new ReservationTypes();
        //        rt.Description = dr["resType"].ToString();

        //        Tuple<Reservations, Courts, ReservationTypes> tupleResList = new Tuple<Reservations, Courts, ReservationTypes>(r, c, rt);
        //        bookingInfoList.Add(tupleResList);
        //    }

        //    //List<HtmlTableRow> trList = new List<HtmlTableRow>();
           

        //    foreach (Tuple<Reservations, Courts, ReservationTypes> t in bookingInfoList)
        //    {
        //        HtmlTableRow tr = new HtmlTableRow();
        //        tr.Attributes.Add("class", "myBookingsTR");
        //        //trList.Add(tr);

        //        for (int x = 0; x < 5; x++)
        //        {
        //            HtmlTableCell td = new HtmlTableCell();
        //            td.Attributes.Add("class", "myBookingsTD");
        //            if (x == 0)
        //            {
        //                DateTime dateOfBooking = Convert.ToDateTime(t.Item1.StartDate.ToShortDateString());
        //                string dayOfWeek = dateOfBooking.ToString("dddd", new CultureInfo("sv-SE"));
        //                string shortDayName = method.FixName(dayOfWeek.Substring(0,3));
        //                string dateOfDate = dateOfBooking.ToString("%d", new CultureInfo("sv-SE"));
        //                string monthNumber = dateOfBooking.ToString("%M", new CultureInfo("sv-SE"));

        //                td.InnerText = shortDayName + " " + dateOfDate + "/" + monthNumber;

        //                //td.InnerText = t.Item1.StartDate.ToShortDateString();
        //            }
        //            if (x == 1)
        //            {
        //                td.InnerText = t.Item1.StartDate.ToShortTimeString();
        //            }
        //            if (x == 2)
        //            {
        //                td.InnerText = t.Item2.CourtId.ToString();
        //            }
        //            if (x == 3)
        //            {
        //                td.InnerText = t.Item3.Description.ToString();
        //            }
        //            if (x == 4)
        //            {
        //                //td.InnerText = "1234";

        //                //visa det högsta datumet från CodeLock om det inte är senare än reservationsdatumet, då är det nästa.
        //                List<CodeLock> codeLockList = new List<CodeLock>();
        //                string queryGetCodeLocks = "SELECT CodeLockId, CodeLock, DateOfChange FROM codelock ORDER BY DateOfChange DESC;";

        //                MySqlDataReader dr2 = method.myReader(queryGetCodeLocks, conn);

        //                while(dr2.Read())
        //                {
        //                    CodeLock cl = new CodeLock();
        //                    cl.CodeLockId = Convert.ToInt16(dr2["CodeLockId"]);
        //                    cl.Code = dr2["CodeLock"].ToString();
        //                    cl.DateOfChange = Convert.ToDateTime(dr2["DateOfChange"]);

        //                    codeLockList.Add(cl);
        //                }


        //                foreach(CodeLock codelock in codeLockList)
        //                {
        //                    if(codelock.DateOfChange > t.Item1.StartDate == false)
        //                    {
        //                        DateTime d = DateTime.Now.AddHours(1);
        //                        if (DateTime.Now.AddHours(1) > t.Item1.StartDate)
        //                        {
        //                            td.InnerText = codelock.Code;

        //                            //Adda ny cell med knapp för avbokning.
        //                        }
        //                        else
        //                        {
        //                            HyperLink h = new HyperLink();
        //                            h.Text = "Visa PIN-kod";
        //                            h.NavigateUrl = "http://www.nba.com";
        //                            td.InnerText = h.Text;
        //                            Page.Controls.Add(h);
        //                        }
        //                        break;
        //                    }

        //                    //else
        //                    //{
        //                    //    td.InnerText = "EXPIRED";
        //                    //}
        //                }

        //            }

        //            tr.Controls.Add(td);
        //        }
        //        bookingsTable.Rows.Add(tr);
        //        myBookingsDiv.Visible = true;
        //    }

        //}


        //public void ShowMySubscriptions()
        //{
        //    List<Tuple<Subscriptions, CourtTimes, Days>> subscriptionInfoList = new List<Tuple<Subscriptions, CourtTimes, Days>>();

        //    string query = "SELECT d.Description, ct.StartHour, s.CourtId FROM subscriptions s "
        //                    + "INNER JOIN courts c ON s.CourtId = c.CourtId "
        //                    + "INNER JOIN courttimes ct ON s.CourtTimeId = ct.CourtTimeId "
        //                    + "INNER JOIN days d ON s.DayId = d.DayId "
        //                    + "WHERE s.MemberId = " + lip.member.MemberId + " ORDER BY s.DayId;";

        //    MySqlDataReader dr = method.myReader(query, conn);

        //    while(dr.Read())
        //    {
        //        Subscriptions s = new Subscriptions();
        //        s.CourtId = Convert.ToInt16(dr["CourtId"]);


        //        CourtTimes ct = new CourtTimes();
        //        ct.StartHour = Convert.ToInt16(dr["StartHour"]);

                
        //        Days d = new Days();
        //        d.Description = dr["Description"].ToString();


        //        Tuple<Subscriptions, CourtTimes, Days> tupleSubList = new Tuple<Subscriptions, CourtTimes, Days>(s, ct, d);
        //        subscriptionInfoList.Add(tupleSubList);
        //    }

        //    foreach (Tuple<Subscriptions, CourtTimes, Days> tup in subscriptionInfoList)
        //    {
        //        HtmlTableRow tr = new HtmlTableRow();
        //        tr.Attributes.Add("class", "mySubscriptionsTR");

        //        for (int y = 0; y < 5; y++)
        //        {
        //            HtmlTableCell td = new HtmlTableCell();
        //            td.Attributes.Add("class", "mySubscriptionsTD");

        //            if(y == 0)
        //            {
        //                td.InnerText = method.EngSweDaySwitch(tup.Item3.Description)+"ar";
        //                //td.InnerText = tup.Item3.Description+"s";
        //            }

        //            if(y == 1)
        //            {
        //                td.InnerText = tup.Item2.StartHour + ":00";
        //            }
        //            if(y == 2)
        //            {
        //                td.InnerText = tup.Item1.CourtId.ToString();
        //            }
        //            if(y == 3)
        //            {
        //                td.InnerText = "100 kr";
        //            }
        //            if(y == 4)
        //            {
        //                td.InnerText = "9876";
        //            }

        //            tr.Controls.Add(td);
        //        }
        //        bookingsTable.Rows.Add(tr);
        //        myBookingsDiv.Visible = true;
        //    }
                            
        //}


        protected void btnBook_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            List<int> courtIdList = new List<int>(); 

            
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

            string insertQuery = "INSERT INTO reservations VALUES ";

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

            
            

            int allreadyBookedCounter = 0;
            string bookingMessageString = "<u>Du har bokat:</u> <br/>";
            foreach(HiddenFieldWithClass hf in hfwcList)
            {
                //IF MEMBERTYPE == 3
                //AND IF lip.member.MemberId INTE HAR NÅGON TID DENNA VECKA
                //RESERVATIONTYPE = 3
                string checkReservationQuery = "SELECT count(*) AS c FROM reservations WHERE CourtId = " + Convert.ToInt16(hf.Value) + " AND StartDate = '" + Convert.ToDateTime(corrStartTime) + "'; ";
                MySqlDataReader dr = method.myReader(checkReservationQuery, conn);

                if (dr.Read())
                {
                    int count = Convert.ToInt16(dr["c"]);
                    if (count > 0)
                    {
                        allreadyBookedCounter += 1;
                    }
                    else
                    {
                        insertQuery += "(" + Convert.ToInt16(hf.Value) + ", " + lip.member.MemberId + ", '" + Convert.ToDateTime(corrStartTime) + "', NULL, 1),";
                        string theDate = Convert.ToDateTime(corrStartTime).ToString("%d",  new CultureInfo("sv-SE"));
                        string theMonth = Convert.ToDateTime(corrStartTime).ToString("%M", new CultureInfo("sv-SE"));
                        string theTime = corrStartTime.Substring(11, 5);

                        bookingMessageString += "Bana " + hf.Value + ", " + theDate + "/" + theMonth + " " + theTime + ". <br />";

                        //string BM = "Bana " + hf.Value + ", " + theDate + "/" + theMonth + " " + Convert.ToDateTime(corrStartTime).ToString("hh:mm") + ". ";
                        //bookingMessageString += "Bana " + hf.Value + ", " + Convert.ToDateTime(corrStartTime).Date.ToString("dd-MM") + " " + Convert.ToDateTime(corrStartTime).ToString("hh:mm") + ". ";

                    }
                }
            }

            char[] s = { ',' };
            string finalQuery = insertQuery.TrimEnd(s) + ";";
            

            MySqlCommand cmdInsertRes = new MySqlCommand(finalQuery, conn);

            if(allreadyBookedCounter == 0)
            {
                conn.Close();
                conn.Open();
                cmdInsertRes.ExecuteNonQuery();
                conn.Close();
                
            }
            else
            {
                bookingMessageString = "Hoppsan, någon hann före, en eller flera av dina valda bantider är redan bokad.";
            }
                
                bool showBMessage = true;
                Session["bookingMessage"] = bookingMessageString;
                Session["showBookingMessage"] = showBMessage;

            //TRY CATCH FINALLY
            Response.Redirect("Booking.aspx");
        }


        protected void btnCancelOK_Click(object sender, EventArgs e)
        {
            string allIDs = Request.Form["__EVENTARGUMENT"].ToString();

            string[] IDs = allIDs.Split(',');

            List<string> IDList = new List<string>();

            foreach (string s in IDs)
            {
                if (s != "")
                {
                    IDList.Add(s);
                }
            }
            
            List<Reservations> lr = new List<Reservations>();
          
            string query = "START TRANSACTION; ";

            for (int i = 0; i < IDList.Count; i++)
            {


                Reservations r = new Reservations();
                r.CourtId = Convert.ToInt16(IDs[i].Substring(3, 1));
                string yyyymmddhhmmss = IDs[i].Substring(5, 10) + " ";
                yyyymmddhhmmss += IDs[i].Substring(16) + ":00:00";
                r.StartDate = Convert.ToDateTime((yyyymmddhhmmss));

                lr.Add(r);

                query += ("DELETE FROM reservations WHERE CourtId = @CID" + i.ToString() + " AND StartDate = @SD" + i.ToString() + "; ");

            }
            query += "COMMIT;";
            MySqlConnection conn = method.myConn();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            for (int i = 0; i < lr.Count; i++)
            {
                DateTime d = lr[i].StartDate;
                

                cmd.Parameters.AddWithValue("CID" + i.ToString(), lr[i].CourtId);
                cmd.Parameters.AddWithValue("SD" + i.ToString(), d.ToString());
            }

            try
            {
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();

            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            Response.Redirect("Booking.aspx");
        }

        //protected void btnCancelOK_Command(object sender, CommandEventArgs e)
        //{
        //    string allIDs = e.CommandArgument.ToString();

        //    string[] IDs = allIDs.Split(',');
        //    List<Reservations> lr = new List<Reservations>();

        //    string query = "START TRANSACTION;";

        //    for (int i = 0; i < IDs.Length; i++)
        //    {
        //        Reservations r = new Reservations();
        //        r.CourtId = Convert.ToInt16(IDs[i].Substring(3,1));
        //        string yyyymmddhhmmss = IDs[i].Substring(5, 10);
        //        yyyymmddhhmmss += IDs[i].Substring(16) + ":00:00";
        //        r.StartDate = Convert.ToDateTime(yyyymmddhhmmss);

        //        lr.Add(r);

        //        query += "DELETE FROM reservations WHERE CourtId = @CID" + i.ToString() + ", AND StartDate = @SD" + i.ToString() + ";";

        //    }
        //    query += "COMMIT;";
        //    MySqlConnection conn = method.myConn();
        //    MySqlCommand cmd = new MySqlCommand(query, conn);
        //    for (int i = 0; i < lr.Count; i++)
        //    {
        //        cmd.Parameters.AddWithValue("CID"+i.ToString(), lr[i].CourtId);
        //        cmd.Parameters.AddWithValue("SD" + i.ToString(), lr[i].StartDate);
        //    }

        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //    conn.Dispose();

        //    Response.Redirect("Booking.aspx");
        //}
    }
}