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
        bool isLoggedInMember;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            string ua = Request.UserAgent;
            if (ua != null
                && (ua.IndexOf("iPhone", StringComparison.CurrentCultureIgnoreCase) >= 0
                || ua.IndexOf("iPad", StringComparison.CurrentCultureIgnoreCase) >= 0
                || ua.IndexOf("iPod", StringComparison.CurrentCultureIgnoreCase) >= 0)
                && ua.IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                this.ClientTarget = "uplevel";
            }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            isLoggedInMember = false;
            lip = (LoggedInPerson)Session["lip"];
            //hfChosenCourts.Value = "0";
            //"8" är antalet dagar som metoden ska hämta, dynamiskt och kan ändras. 
            hfNoOfClickedCourts.Value = "0";
            if(Session["lip"] != null)
            {
                isLoggedInMember = true;
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
                        preBookingInfo.Visible = true;
                        bookingConfirmationMessage.InnerHtml = Session["bookingMessage"].ToString();
                        preBookingInfo.InnerHtml = method.BookingInfoString(lip);
                    }
                    else
                    {
                        bookingErrorMessage.Visible = true;
                        preBookingInfo.Visible = true;
                        bookingErrorMessage.InnerHtml = Session["bookingMessage"].ToString();
                        preBookingInfo.InnerHtml = method.BookingInfoString(lip);
                    }

                    //bookingMessage.Visible = true;
                    //bookingMessage.InnerText = (string)Session["bookingMessage"];

                    Session["showBookingMessage"] = false;
                }
                else if(lip.member.MemberType != 1)
                {
                    preBookingInfo.Visible = true;
                    preBookingInfo.InnerHtml = method.BookingInfoString(lip);
                }


            }

            if (Session["lip"] != null)
            {
                LIPBuildSchedule(8);
            }
            else
            {
                BuildSchedule(8);
            }


        }
        /// <summary>
        /// Loopar all bokningsdata data och genererar ett dynamiskt schema för det valda antalet dagar(nOfDays)
        /// </summary>
        /// <param name="noOfDays">Antalet dagar att visa</param>
        public void BuildSchedule(int noOfDays)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");

            DateTime bookingDate = DateTime.Now.Date;
            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));
            
            if (todayNo == 0)
            {
                todayNo = 7;
            }

            List<Days> DayList = method.GetDayList();
            List<Subscriptions> allSubscriptions = method.GetSubscriptionList();
            List<Reservations> allReservations = method.GetReservationsList(noOfDays);
            List<Companies> allCompanies = method.GetCompanyList();
            List<MemberCompany> allMemberCompany = method.GetMemberCompanyList();

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
                        
                        //HF NEDAN
                        hfWidthOfDaySelectors.Value = method.DivideWidth(100, Convert.ToDouble(noOfDays));
                        hfWidthOfDayDivs.Value = method.DivideWidth(94, Convert.ToDouble(noOfDays));
                        //

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


                        DateTime FullDateTime = new DateTime();


                        //Loopar alla starttider för den aktuella dagen och skapar element för dessa.
                        foreach (CourtTimes CT in D.CourtTimes)
                        {
                            HtmlGenericControl hourDiv = new HtmlGenericControl("div");
                            hourDiv.Attributes.Add("id", "" + D.DayId + CT.CourtTimeId + "");
                            hourDiv.Attributes.Add("class", "hourDivs");

                            //NY METOD
                            string thisDayIsFullTime = method.ConvertToFullTime(CT.StartHour, false);
                            string shortTime = method.ConvertToFullTime(CT.StartHour, true);
                            //
                            string hourBookingDivId = thisDayIsFullDate + "_" + shortTime;

                            FullDateTime = Convert.ToDateTime(thisDayIsFullDate + " " + thisDayIsFullTime);

                            HtmlGenericControl pTime = new HtmlGenericControl("p");
                            pTime.Attributes.Add("class", "pTime");
                            pTime.InnerHtml = shortTime+":00";

                            hourDiv.Controls.Add(pTime);

                            HtmlGenericControl hourBookingDiv = new HtmlGenericControl("div");
                            hourBookingDiv.Attributes.Add("class", "hourBookingDiv");
                            hourBookingDiv.Attributes.Add("id", hourBookingDivId);
                            hourBookingDiv.InnerHtml = "<h3>Boka - " + shortDayIs + " " + thisDayIsDate + "/" + thisDayIsMonth + " " + shortTime + ":00</h3><p>Klicka på önskade banor.</p><hr />";

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
                                //pBookedBy.InnerHtml = "Ledig tid";

                                bool subscribed = false;
                                bool reserved = false;

                                //Loopar och jämför varje bana+tid och sätter ev. bokningstatus För abonnemangstider.
                                foreach (Subscriptions sub in allSubscriptions)
                                {
                                    if (sub.CourtId == C.CourtId && sub.CourtTimeId == CT.CourtTimeId && sub.DayId == D.DayId)
                                    {
                                        subscribed = true;
                                        
                                        courtDiv.Attributes.Add("class", "courtDivs subscribedCourt masterTiptool B" + C.CourtId);

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
                                        break;
                                    }
                                }

                                //Loopar och jämför varje bana+tid och sätter ev. bokningstatus För strötider.
                                foreach (Reservations res in allReservations)
                                {
                                    if (res.CourtId == C.CourtId && res.StartDate == Convert.ToDateTime((thisDayIsFullDate + " " + thisDayIsFullTime)))
                                    {
                                        reserved = true;
                                       
                                        courtDiv.Attributes.Add("class", "courtDivs reservedCourt masterTiptool B" + C.CourtId);
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
                                        allReservations.Remove(res);
                                        break;


                                    }
                                }

                                //Om användaren är inloggad
                                
                                if (subscribed == false && reserved == false)
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


                        if(D.DayId != todayNo)
                        {
                            DayList.Remove(D);
                            break;
                        }
                    }

                }

                if (i == 7)
                {
                    i = 0;
                }

            }
        }
















        public void LIPBuildSchedule(int noOfDays)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");

            bool hasCLReq = false;
            int resLeft = 0;
            bool maximizedReservations = false;

            DateTime bookingDate = DateTime.Now.Date;
            int todayNo = Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("d"));
            if (todayNo == 0)
            {
                todayNo = 7;
            }

            List<Days> DayList = method.GetDayList();
            List<Subscriptions> allSubscriptions = method.GetSubscriptionList();
            List<Reservations> allReservations = method.GetReservationsList(noOfDays);
            List<Companies> allCompanies = method.GetCompanyList();
            List<MemberCompany> allMemberCompany = method.GetMemberCompanyList();

            List<HtmlGenericControl> daySelectorList = new List<HtmlGenericControl>();

            int counter = 0;
            int mondayCounter = 0;
            bool drawtimes = true;


            //Startar loopen på dagens "todayNo", ex. Måndag = 1 eller Fredag = 5.
            for (int i = todayNo; i <= noOfDays; i++)
            {
                counter++;
                if(i == 1 && counter != 1)
                {
                    mondayCounter++;
                }

                if (lip.member.MemberType == 3)
                {
                    if (counter == 1)
                    {
                        resLeft = method.HasFloatReservation(DateTime.Now, lip);
                        mondayCounter++;
                    }
                    else if (mondayCounter > 1 && i == 1)
                    {
                        resLeft = method.HasFloatReservation(DateTime.Now.AddDays(counter -1), lip);
                    }
                }


                if (lip.member.MemberType == 1 || lip.member.MemberType == 2)
                {
                    if(counter == 1)
                    {
                        maximizedReservations = method.HasMaximizedReservations(DateTime.Now.Date, lip);
                        preBookingInfo.Visible = maximizedReservations;
                        preBookingInfo.InnerHtml = "Ni har bokat max reservationer för nuvarande vecka.";
                        mondayCounter++;
                    }
                    else if (mondayCounter > 1 && i == 1)
                    {
                        maximizedReservations = method.HasMaximizedReservations(DateTime.Now.AddDays(counter - 1), lip);
                    }
                }

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

                //Days loopDay = DayList.Single(cus => cus.DayId == i);
                //int idOfLoopDay = loopDay.DayId;
                //bool theDay = DayList.Any(cus => cus.DayId == i);
                //Loopar veckans dagar enligt de dagar som specificerats i databasen skapar element för dessa.
                foreach (Days D in DayList)
                {
                    IEnumerable<Subscriptions> wantedSubsDay;
                    if (D.DayId == i)
                    {

                        wantedSubsDay = (from s in allSubscriptions where s.DayId == D.DayId select s);


                        HtmlGenericControl dayDiv = new HtmlGenericControl("div");
                        dayDiv.Attributes.Add("id", counter.ToString() + "_day");
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
                        
                        if(Convert.ToDateTime(thisDayIsFullDate).Date == DateTime.Now.Date)
                        {
                            hasCLReq = method.HasCLRequest(lip);
                        }
                        else
                        {
                            hasCLReq = false;
                        }

                        //HtmlGenericControl bookingTextDiv = method.BookingInfoMessage(lip, Convert.ToDateTime(thisDayIsFullDate).Date, hasCLReq, resLeft);
                        





                        selectorDiv.Controls.Add(daySelector);
                        dayDiv.Controls.Add(staticDayDiv);


                        DateTime FullDateTime = new DateTime();


                        //Loopar alla starttider för den aktuella dagen och skapar element för dessa.
                        foreach (CourtTimes CT in  D.CourtTimes)
                        {
                            IEnumerable<Subscriptions> wantedSubsCT = (from w in wantedSubsDay where w.CourtTimeId == CT.CourtTimeId select w);

                            HtmlGenericControl hourDiv = new HtmlGenericControl("div");
                            hourDiv.Attributes.Add("id", "" + D.DayId + CT.CourtTimeId + "");
                            hourDiv.Attributes.Add("class", "hourDivs");

                            string thisDayIsFullTime = method.ConvertToFullTime(CT.StartHour, false);
                            string shortTime = method.ConvertToFullTime(CT.StartHour, true);
                            string hourBookingDivId = thisDayIsFullDate + "_" + shortTime;

                            FullDateTime = Convert.ToDateTime(thisDayIsFullDate + " " + thisDayIsFullTime);

                            HtmlGenericControl pTime = new HtmlGenericControl("p");
                            pTime.Attributes.Add("class", "pTime");
                            pTime.InnerHtml = shortTime + ":00";

                            hourDiv.Controls.Add(pTime);

                            HtmlGenericControl hourBookingDiv = new HtmlGenericControl("div");
                            hourBookingDiv.Attributes.Add("class", "hourBookingDiv");
                            hourBookingDiv.Attributes.Add("id", hourBookingDivId);
                            hourBookingDiv.InnerHtml = "<h3>Boka - " + shortDayIs + " " + thisDayIsDate + "/" + thisDayIsMonth + " " + shortTime + ":00</h3><p>Klicka på önskade banor.</p><hr />";

                            HtmlGenericControl bookingDescriptionDiv = new HtmlGenericControl("div");
                            bookingDescriptionDiv.Attributes.Add("id", "bookingDescriptionDiv" + CT.CourtTimeId);
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
                                courtDiv.Attributes.Add("class", "courtDivs B" + C.CourtId);

                                HtmlGenericControl pBookedBy = new HtmlGenericControl("p");
                                //pBookedBy.InnerHtml = "Ledig tid";

                                bool subscribed = false;
                                bool reserved = false;







                                //Subscriptions loopSub = allSubscriptions.(cus => cus.DayId == i);
                                //int idOfLoopSub = loopSub.DayId;

                                //IEnumerable<Subscriptions> wantedSubs = allSubscriptions.Where(s => s.DayId == loopSub.DayId);
                                //IEnumerable<Subscriptions> wantedSubsDay = (from s in allSubscriptions where s.DayId == D.DayId select s);
                                //List<Subscriptions> wDList = wantedSubsDay.ToList();

                                //IEnumerable<Subscriptions> wantedSubsCT = (from w in wantedSubsDay where w.CourtTimeId == CT.CourtTimeId select w);
                                //List<Subscriptions> wCTList = wantedSubsCT.ToList();

                                try
                                {
                                    Subscriptions sub = wantedSubsCT.Single(cus => cus.CourtId == C.CourtId);


                                    //IEnumerable<Subscriptions> wantedSubsC = (from c in wantedSubsCT where c.CourtId == C.CourtId select c);

                                    //List<Subscriptions> wCList = wantedSubsC.ToList();
                                    //Subscriptions sub = wCList[0];


                                    //IEnumerable<Subscriptions> wantedSubs = allSubscriptions.Where(t => t.DayId == idOfLoopDay);


                                    //SELECT ALL SUBSCIPTIONS WHERE CourtId = C.CourtId AND CourtTimeId = CT.CourtTimeId && DayId == loopDay.DayId/D.DayId


                                    //Loopar och jämför varje bana+tid och sätter ev. bokningstatus För abonnemangstider.
                                    //foreach (Subscriptions sub in wCList)
                                    //{
                                    //if (sub.CourtId == C.CourtId && sub.CourtTimeId == CT.CourtTimeId && sub.DayId == D.DayId)
                                    //{
                                    subscribed = true;

                                    if (sub.MemberId == lip.member.MemberId)
                                    {
                                        courtDiv.Attributes.Add("class", "courtDivs subscribedCourt mySubscribedCourt masterTiptool B" + C.CourtId);
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
                                        //}
                                        break;
                                    }
                                }
                                catch
                                {

                                }
                                //}

                                //Loopar och jämför varje bana+tid och sätter ev. bokningstatus För strötider.
                                foreach (Reservations res in allReservations)
                                {
                                    if (res.CourtId == C.CourtId && res.StartDate == Convert.ToDateTime((thisDayIsFullDate + " " + thisDayIsFullTime)))
                                    {
                                        reserved = true;
                                        if (res.MemberId == lip.member.MemberId)
                                        {
                                            if (res.StartDate > DateTime.Now.AddHours(1))
                                            {
                                                DateTime dateOfBooking = Convert.ToDateTime(res.StartDate.ToShortDateString());
                                                string dayOfWeek = dateOfBooking.ToString("dddd", new CultureInfo("sv-SE"));
                                                string shortDayName = method.FixName(dayOfWeek.Substring(0, 3));
                                                string dateOfDate = dateOfBooking.ToString("%d", new CultureInfo("sv-SE"));
                                                string monthNumber = dateOfBooking.ToString("%M", new CultureInfo("sv-SE"));

                                                string myResValue = C.Description + " " + shortDayName + " " + dateOfDate + "/" + monthNumber + " " + shortTime + ":00";

                                                //if (!method.HasCLRequest(lip, Convert.ToDateTime(thisDayIsFullDate)))
                                                if(!hasCLReq)
                                                {
                                                    string cancelParameter = "cb_" + C.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime;
                                                    courtDiv.Attributes.Add("onclick", "CancelThisRes('" + cancelParameter + "', '" + myResValue + "')");
                                                    courtDiv.Attributes.Add("title", "Klicka för att avboka din tid.");
                                                    courtDiv.Attributes.Add("class", "courtDivs reservedCourt myReservedCourt showPointer masterTiptool B" + C.CourtId);
                                                }
                                                else
                                                {
                                                    courtDiv.Attributes.Add("title", "PIN-kod visad, du kan inte avboka tiden.");
                                                    courtDiv.Attributes.Add("class", "courtDivs reservedCourt myReservedCourt masterTiptool B" + C.CourtId);
                                                }
                                            }

                                            else
                                            {
                                                courtDiv.Attributes.Add("class", "courtDivs reservedCourt myReservedCourt B" + C.CourtId);

                                            }


                                        }
                                        else
                                        {
                                            courtDiv.Attributes.Add("class", "courtDivs reservedCourt masterTiptool B" + C.CourtId);
                                            courtDiv.Attributes.Add("title", "Redan bokad av " + res.FullMemberName);
                                        }


                                        courtDiv.InnerHtml = res.FullMemberName;

                                        pBookedBy.InnerHtml = "Bokad av " + res.FullMemberName;

                                        foreach (MemberCompany mc in allMemberCompany)
                                        {
                                            if (mc.MemberId == res.MemberId)
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
                                        allReservations.Remove(res);
                                        break;


                                    }
                                }



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
                                    if(!maximizedReservations)
                                    {
                                        courtDiv.Attributes.Add("onclick", "OpenBookingOverlay('" + hourBookingDivId + "')");
                                        courtImgDiv.Attributes.Add("class", "courtImgDivFree");
                                        courtImgDiv.Attributes.Add("onclick", "chosenCourt('" + "hf" + bookingDivId + "','" + C.CourtId.ToString() + "','" + bookingDivId + "')");
                                        courtDiv.Attributes.Add("title", "Klicka för att boka Bana " + C.CourtId.ToString() + ", " + thisDayIs + " " + thisDayIsDate + "/" + thisDayIsMonth);
                                    }
                                    else if(maximizedReservations)
                                    {
                                        courtDiv.Attributes.Add("title", "Ni har bokat max antal tider denna vecka, försök boka måndag istället.");
                                    }

                                    if ((lip.member.MemberType == 3) && (CT.StartHour < 6 || CT.StartHour > 16))
                                    {
                                        courtDiv.Attributes.Add("class", "freeCourt fcsDivs courtDivs masterTiptool B" + C.CourtId);
                                    }
                                    else
                                    {
                                        courtDiv.Attributes.Add("class", "courtDivs freeCourt masterTiptool B" + C.CourtId);
                                    }
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

                            //Segt
                            hourBookingDiv.Controls.Add(method.BookingInfoMessage(lip, FullDateTime, hasCLReq, resLeft));
                            //
                            hourBookingDiv.Controls.Add(btnBook);

                            bookingOverlayMessage.Controls.Add(hourBookingDiv);
                            dayDiv.Controls.Add(hourDiv);
                        }
                        scheduleDiv.Controls.Add(dayDiv);


                        if (D.DayId != todayNo)
                        {
                            DayList.Remove(D);
                            break;
                        }
                    }

                }

                if (i == 7)
                {
                    i = 0;
                }

            }

        }


        protected void btnBook_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            List<int> courtIdList = new List<int>();


            string startTime = btn.CommandArgument.ToString();
            string corrStartTime = "";

            foreach (char c in startTime)
            {
                if (c.ToString() == "_")
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

            string insertQuery = "INSERT INTO reservations(CourtId, MemberId, StartDate, ReservationType) VALUES(@CID, @MID, @SD, @RT)";

            List<Control> controlList = new List<Control>();
            List<HiddenFieldWithClass> hfwcList = new List<HiddenFieldWithClass>();
            //foreach (HiddenFieldWithClass hf in this.Page.Controls)
            foreach (Control c in this.Page.Controls)
            {
                //controlList.Add(c);
                foreach (Control c2 in c.Controls)
                {
                    //controlList.Add(c2);
                    foreach (Control c3 in c2.Controls)
                    {
                        //controlList.Add(c3);
                        foreach (Control c4 in c3.Controls)
                        {
                            //controlList.Add(c4);
                            foreach (Control c5 in c4.Controls)
                            {
                                if (c5 is HiddenFieldWithClass)
                                {
                                    HiddenFieldWithClass a = (HiddenFieldWithClass)c5;
                                    if (a.Value != "0")
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

            foreach (HiddenFieldWithClass hf in hfwcList)
            {
                string checkReservationQuery = "SELECT count(*) AS c FROM reservations WHERE CourtId = " + Convert.ToInt16(hf.Value) + " AND StartDate = '" + Convert.ToDateTime(corrStartTime) + "'; ";
                MySqlDataReader dr = method.myReader(checkReservationQuery, conn);
                if (dr.Read())
                {
                    int count = Convert.ToInt16(dr["c"]);
                    if (count > 0)
                    {
                        allreadyBookedCounter += 1;
                    }
                }
            }


            if (allreadyBookedCounter == 0)
            {
                foreach (HiddenFieldWithClass hf in hfwcList)
                {
                    //IF MEMBERTYPE == 3 @CID, @MID, @SD, @RT
                    //AND IF lip.member.MemberId INTE HAR NÅGON TID DENNA VECKA
                    //RESERVATIONTYPE = 3
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("CID", Convert.ToInt16(hf.Value));
                    cmd.Parameters.AddWithValue("MID", lip.member.MemberId);
                    cmd.Parameters.AddWithValue("SD", Convert.ToDateTime(corrStartTime));

                    int playHour = Convert.ToInt16(corrStartTime.Substring(11, 2));

                    if (lip.member.MemberType == 3 && (playHour >= 6 && playHour <= 16) && method.HasFloatReservation(Convert.ToDateTime(corrStartTime), lip) > 0)
                    {
                        cmd.Parameters.AddWithValue("RT", 3);
                    }
                    else
                    {
                        if (method.IsSubscriber(lip) && Convert.ToDateTime(corrStartTime).AddHours(-6) < DateTime.Now && lip.member.MemberType == 1)
                        {
                            cmd.Parameters.AddWithValue("RT", 4);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("RT", 1);
                        }
                    }

                    //insertQuery += "(" + Convert.ToInt16(hf.Value) + ", " + lip.member.MemberId + ", '" + Convert.ToDateTime(corrStartTime) + "', NULL, 1),";
                    string theDate = Convert.ToDateTime(corrStartTime).ToString("%d", new CultureInfo("sv-SE"));
                    string theMonth = Convert.ToDateTime(corrStartTime).ToString("%M", new CultureInfo("sv-SE"));
                    string theTime = corrStartTime.Substring(11, 5);

                    bookingMessageString += "Bana " + hf.Value + ", " + theDate + "/" + theMonth + " " + theTime + ". <br />";

                    //string BM = "Bana " + hf.Value + ", " + theDate + "/" + theMonth + " " + Convert.ToDateTime(corrStartTime).ToString("hh:mm") + ". ";
                    //bookingMessageString += "Bana " + hf.Value + ", " + Convert.ToDateTime(corrStartTime).Date.ToString("dd-MM") + " " + Convert.ToDateTime(corrStartTime).ToString("hh:mm") + ". ";
                    conn.Close();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();

                }
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

 
    }
}