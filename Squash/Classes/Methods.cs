using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Sql;
using MySql.Data.MySqlClient;
using System.Configuration;
using Squash.Classes;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Globalization;
using System.Diagnostics;

namespace Squash.Classes
{
    public class Methods
    {
        #region Connection, myReader, myDelete
        public MySqlConnection myConn()
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["squashOnline"].ConnectionString);
            return conn;
        }
        public MySqlDataReader myReader(string query, MySqlConnection conn)
        {
            //MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["squash"].ConnectionString);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Close();
            conn.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        public void myDelete(string query, MySqlConnection conn)
        {
            //MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["squash"].ConnectionString);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        #endregion


        #region LogIn
        public bool EmailExist(string email)
        {
            MySqlConnection conn = myConn();
            string queryEmailExist = "SELECT * FROM users_updated WHERE EMail = '" + email + "'";
            MySqlDataReader dr = myReader(queryEmailExist, conn);

            if (!dr.HasRows)
            {
                conn.Close();
                return false;
            }
            else
            {
                conn.Close();
                return true;
            }
        }

        public string Hashify(string pw)
        {
            string input = pw;
            ASCIIEncoding ASCII = new ASCIIEncoding();
            byte[] HashValue;
            byte[] MessageBytes = ASCII.GetBytes(input);
            SHA1Managed sha1 = new SHA1Managed();

            string finalHash = "";

            HashValue = sha1.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
            {
                finalHash += String.Format("{0:x2}", b);
            }

            return finalHash;
        }

        public void ResetPW(string toEmail)
        {

            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[8];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(8);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            pwResetMail(toEmail, result.ToString());


        }


        #endregion

        #region E-mail
        public void pwResetMail(string toEmail, string newPW)
        {
            string content = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Ditt nya lösenord</title></head><body><h2>Ditt nya lösenord</h2><hr /><p>" + newPW + "</p></body></html>";


            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.From = new MailAddress("admin@ostersundssquash.se", "Östersunds Squashförening", System.Text.Encoding.UTF8);
            mail.Subject = "Ditt nya lösenord";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = content;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

            SmtpClient client = new SmtpClient();

            try
            {
                client.Send(mail);

                try
                {

                    string queryUpdatePW = "UPDATE users_updated "
                    + "SET Password = @pw "
                    + "WHERE EMail = '" + toEmail + "'";
                    MySqlConnection conn = myConn();

                    MySqlCommand cmd = new MySqlCommand(queryUpdatePW, conn);
                    cmd.Parameters.AddWithValue("pw", Hashify(newPW));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
                catch (MySqlException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Debug.WriteLine(ex);
            }



        }


        #endregion

        #region CheckTelephoneNumber

        public string FixNumber(string number)
        {
            string newNum = "";

            foreach (char c in number)
            {
                if (Char.IsLetter(c))
                {
                    newNum += "";
                }
                else if (Char.IsDigit(c))
                {
                    newNum += c;
                }
                else
                {
                    newNum += "";
                }
            }

            return newNum;
        }


        public string SplitNumber(string phoneNr)
        {
            string newNumber = "";
            string areaCode = "";

            if (phoneNr == "")
            {
                return phoneNr;
            }

            else if (phoneNr.Substring(0, 2) == "46")
            {
                phoneNr = "0" + phoneNr.Substring(2);
            }

            if (phoneNr.Length >= 8 && phoneNr.Length <= 10)
            {
                if (phoneNr.Substring(0, 2) == "07")
                {
                    areaCode = phoneNr.Substring(0, 3);
                    if (phoneNr.Length == 8)
                    {
                        newNumber = areaCode + "-" + phoneNr.Substring(3, 3) + " " + phoneNr.Substring(6, 2);
                    }
                    if (phoneNr.Length == 9)
                    {
                        newNumber = areaCode + "-" + phoneNr.Substring(3, 2) + " " + phoneNr.Substring(5, 2) + " " + phoneNr.Substring(7, 2);
                    }
                    if (phoneNr.Length == 10)
                    {
                        newNumber = areaCode + "-" + phoneNr.Substring(3, 3) + " " + phoneNr.Substring(6, 2) + " " + phoneNr.Substring(8);

                    }

                    return newNumber;
                }
                else if (phoneNr.Substring(0, 3) == "063")
                {
                    areaCode = phoneNr.Substring(0, 3);

                    if (phoneNr.Length == 8)
                    {
                        newNumber = areaCode + "-" + phoneNr.Substring(3, 3) + " " + phoneNr.Substring(6, 2);
                    }
                    if (phoneNr.Length == 9)
                    {
                        newNumber = areaCode + "-" + phoneNr.Substring(3, 2) + " " + phoneNr.Substring(5, 2) + " " + phoneNr.Substring(7, 2);
                    }
                    return newNumber;
                }

                else
                {
                    areaCode = phoneNr.Substring(0, 4);
                    newNumber = areaCode + "-" + phoneNr.Substring(4);
                    return newNumber;
                }


            }

            else
            {
                return phoneNr;
            }

        }

        #endregion

        #region UpperCaseLowerCase

        public string FixName(string text)
        {
            string correctName = "";
            int count = 0;
            bool whitespaceAtStart = true;

            foreach (char c in text)
            {
                count++;
                //if (count == 1 && c.ToString() == " ")
                if (c.ToString() == " " && whitespaceAtStart == true)
                {
                    correctName += "";
                }
                else
                {
                    correctName += c;
                    whitespaceAtStart = false;
                }
            }

            CultureInfo ci = new CultureInfo("sv-SE");

            return ci.TextInfo.ToTitleCase(correctName);

            //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }


        public string FixNameRegister(string text)
        {
            string correctName = "";
            int count = 0;
            bool whitespaceAtStart = true;
            bool dashAtStart = true;

            foreach (char c in text)
            {
                count++;
                //if (count == 1 && c.ToString() == " ")
                if (c.ToString() == " " && whitespaceAtStart == true)
                {
                    correctName += "";
                }
                else if (c.ToString() == "-" && dashAtStart == true)
                {
                    correctName += "";
                }
                else if (Char.IsDigit(c))
                {
                    correctName += "";
                    whitespaceAtStart = false;
                    dashAtStart = false;
                }
                else if (Char.IsLetter(c) || c.ToString() == "-")
                {
                    correctName += c;
                    whitespaceAtStart = false;
                    dashAtStart = false;
                }
                else
                {
                    correctName += "";
                    whitespaceAtStart = false;
                    dashAtStart = false;
                }
            }

            CultureInfo ci = new CultureInfo("sv-SE");
            //toLowerFirst
            string corrName = correctName.ToLower();

            return ci.TextInfo.ToTitleCase(correctName);

            //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        #endregion


        #region BookingMethods

        public bool HasMaximizedReservations(DateTime dateToCheck, LoggedInPerson lip)
        {
            int numbersOfRes = 0;

            int todayNo = Convert.ToInt16(dateToCheck.DayOfWeek.ToString("d"));
            if (todayNo == 0)
            {
                todayNo = 7;
            }

            int startDay = todayNo - 1;
            int endDay = 7 - todayNo + 1;

            DateTime startDayOfWeek = dateToCheck.AddDays(-startDay);
            DateTime endDayOfWeek = dateToCheck.AddDays(endDay);

            string query = "SELECT COUNT(*) AS NoRes FROM reservations WHERE (StartDate BETWEEN DATE('" + startDayOfWeek + "') AND DATE('" + endDayOfWeek + "')) AND MemberId = " + lip.member.MemberId + ";";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            if(dr.Read())
            {
                numbersOfRes = Convert.ToInt16(dr["NoRes"]);
            }

            if (numbersOfRes < 5)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        public int HasFloatReservation(DateTime dateToCheck, LoggedInPerson lip)
        {
            int NoOfFloatResLeft = lip.memberfloatable.NoTimesWeek;

            int todayNo = Convert.ToInt16(dateToCheck.DayOfWeek.ToString("d"));
            if (todayNo == 0)
            {
                todayNo = 7;
            }

            int startDay = todayNo - 1;
            int endDay = 7 - todayNo + 1;

            DateTime startDayOfWeek = dateToCheck.AddDays(-startDay);
            DateTime endDayOfWeek = dateToCheck.AddDays(endDay);

            string query = "SELECT COUNT(*) AS NoRes FROM reservations WHERE (StartDate BETWEEN DATE('" + startDayOfWeek + "') AND DATE('" + endDayOfWeek + "')) AND ReservationType = 3 AND MemberId = " + lip.member.MemberId + ";";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            if (dr.Read())
            {
                NoOfFloatResLeft -= Convert.ToInt16(dr["NoRes"]);
                int test = Convert.ToInt16(dr["NoRes"]);
            }
            return NoOfFloatResLeft;
            //KOLLA stardatum på veckan. todayno - 1; För att veta hur många dagar vi ska gå tillbaka (måndag).
            //7-todayNo → så många dagar vi ska gå framåt.



        }
        public bool IsSubscriber(LoggedInPerson lip)
        {
            foreach (Subscriptions sub in GetSubscriptionList())
            {
                if (sub.MemberId == lip.member.MemberId)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Days> GetDayList()
        {
            List<Days> DayList = new List<Days>();

            string query = "SELECT * FROM days";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

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
            dr = myReader(query, conn);
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
            dr = myReader(query, conn);
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
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            while (dr.Read())
            {
                Subscriptions s = new Subscriptions();
                s.CourtId = Convert.ToInt16(dr["CourtId"]);
                s.CourtTimeId = Convert.ToInt16(dr["CourtTimeId"]);
                s.DayId = Convert.ToInt16(dr["DayId"]);
                s.MemberId = Convert.ToInt16(dr["MemberId"]);
                s.FullMemberName = FixName(dr["Firstname"].ToString() + " " + dr["Surname"].ToString());
                subscriptionList.Add(s);
            }

            return subscriptionList;
        }

        //HÄMTAR ALLA RESERVATIONER OCH LAGRAR DESSA I EN LISTA
        public List<Reservations> GetReservationsList(int numberOfDays)
        {
            List<Reservations> reservationsList = new List<Reservations>();

            string query = "SELECT r.CourtId, r.MemberId, r.StartDate, r.HandledBy, r.ReservationType, u.Firstname, u.Surname FROM reservations r "
                         + "INNER JOIN members m ON m.MemberId = r.MemberId "
                         + "INNER JOIN users u ON u.UserId = m.UserId "
                         + "WHERE StartDate BETWEEN DATE(NOW() - INTERVAL 1 HOUR) AND DATE(NOW() + INTERVAL '" + numberOfDays + "' DAY) "
                         + "ORDER BY StartDate ASC;";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

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
                r.FullMemberName = FixName(dr["Firstname"].ToString() + " " + dr["Surname"].ToString());
                reservationsList.Add(r);
            }
            return reservationsList;
        }

        //HÄMTAR ALLA FÖRETAG OCH LAGRAR DESSA I EN LISTA.
        public List<Companies> GetCompanyList()
        {
            List<Companies> companiesList = new List<Companies>();
            string query = "SELECT Id, Name FROM Companies";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            while (dr.Read())
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
            string query = "SELECT mc.MemberId, mc.CompanyId FROM MemberCompany mc";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            while (dr.Read())
            {
                MemberCompany mc = new MemberCompany();
                mc.MemberId = Convert.ToInt16(dr["MemberId"]);
                mc.CompanyId = Convert.ToInt16(dr["CompanyId"]);
                memberCompanyList.Add(mc);
            }
            conn.Close();

            return memberCompanyList;
        }

        #endregion

        #region DesignMethods
        /// <summary>
        /// Delar upp antal kolumner jämt på bokningssidan i desktopläge.
        /// </summary>
        /// <param name="percentToDivide">Hur många procent man vill dela kolumnerna på</param>
        /// <param name="noOfElements">Antalet kolumner som ska få dela på den totala procenten</param>
        /// <returns>En procent som kolumnerna ska få som bredd</returns>
        public string DivideWidth(double percentToDivide, double noOfElements)
        {
            double width = percentToDivide / noOfElements;
            string widthString = width.ToString();
            string widthInPercent = "";

            foreach (Char c in widthString)
            {
                if (c.ToString() == ",")
                {
                    widthInPercent += ".";
                }
                else
                {
                    widthInPercent += c;
                }
            }

            return widthInPercent + "%";
        }



        /// <summary>
        /// GÖr om till fulltid eller bara timmar.
        /// </summary>
        /// <param name="oneToZeroHour"></param>
        /// <param name="onlyHour">true för format: "06" eller false för: 06:00:00</param>
        /// <returns></returns>
        public string ConvertToFullTime(int oneToZeroHour, bool onlyHour)
        {
            string fullTime = "";
            if (onlyHour)
            {
                if (oneToZeroHour < 10)
                {
                    fullTime = "0" + oneToZeroHour.ToString();

                }
                else
                {
                    fullTime = oneToZeroHour.ToString();
                }
            }
            else
            {
                if (oneToZeroHour < 10)
                {
                    fullTime = "0" + oneToZeroHour.ToString() + ":00:00";

                }
                else
                {
                    fullTime = oneToZeroHour.ToString() + ":00:00";
                }
            }

            return fullTime;

        }


        /// <summary>
        /// GÖr ett meddelande om bokningen. Ex om man redan sett PIN-koden för dagen, att fullpris kommer debiteras, hur många bokningar man har kvar för veckan etc.
        /// </summary>
        /// <param name="lip">Logged In Person = inloggad person</param>
        /// <param name="chosenDay">Vald dag</param>
        /// <param name="HasCLReq">Om man har en CodeLockRequest</param>
        /// <param name="noOfResLeft">Antalet reservationer kvar</param>
        /// <returns>En div med en text i sig</returns>
        public HtmlGenericControl BookingInfoMessage(LoggedInPerson lip, DateTime chosenDay, bool HasCLReq, int noOfResLeft)
        {
            HtmlGenericControl bookingInfoTextDiv = new HtmlGenericControl("div");
            bookingInfoTextDiv.Attributes.Add("class", "bookingInfoTextDiv");

            HtmlGenericControl bookingInfoText = new HtmlGenericControl("p");
            bookingInfoText.Attributes.Add("class", "bookingInfoText");

            string divText = "";

            if(HasCLReq)
            {
                divText += "• Eftersom Ni redan har sett dagens PIN-kod kommer du inte kunna avboka tiden. <br />";

                if (lip.member.MemberType == 2)
                {
                    divText += "• Fullpris debiteras kontoansvarig.";
                }
                else if (lip.member.MemberType == 3)
                {
                    string onlyDate = chosenDay.ToString("yyyy-MM-dd");
                    DateTime floatableStart = Convert.ToDateTime(onlyDate + " 06:00:00");
                    DateTime floatableEnd = Convert.ToDateTime(onlyDate + " 16:00:00");
                    if (chosenDay >= floatableStart && chosenDay <= floatableEnd)
                    {
                        int resLeft = noOfResLeft;

                        if (resLeft > 0)
                        {
                            divText += "• Ni har <strong>" + resLeft + "</strong> fria bokningar kvar för den valda veckan, övriga bokningar debiteras fullpris.";
                        }
                        else
                        {
                            divText += "• Ni har <strong>inga</strong> fria bokningar kvar för den valda veckan, vid bokning debiteras fullpris.";
                        }
                    }
                    else
                    {
                        divText += "• Tiden ligger utanför abonnemangets tidsintervall (<strong>6 - 16</strong>) och debiteras fullpris.";
                    }

                }
            }
            else
            {
                divText += "• Ni kan avboka senast en timme i förväg. PIN-koden visas längst upp på sidan. <br />";
                if (lip.member.MemberType == 1)
                {
                    divText += "• Vill du se PIN-koden nu trycker du på Visa PIN längst upp på sidan.";
                }
                else if (lip.member.MemberType == 2)
                {
                    divText += "• Fullpris debiteras kontoansvarig.";
                }
                else if (lip.member.MemberType == 3)
                {
                    string onlyDate = chosenDay.ToString("yyyy-MM-dd");
                    DateTime floatableStart = Convert.ToDateTime(onlyDate + " 06:00:00");
                    DateTime floatableEnd = Convert.ToDateTime(onlyDate + " 16:00:00");
                    if (chosenDay >= floatableStart && chosenDay <= floatableEnd)
                    {
                        int resLeft = noOfResLeft;

                        if (resLeft > 0)
                        {
                            divText += "• Ni har <strong>" + resLeft + "</strong> fria bokningar kvar för den valda veckan, övriga bokningar debiteras fullpris.";
                        }
                        else
                        {
                            divText += "• Ni har <strong>inga</strong> fria bokningar kvar för den valda veckan, vid bokning debiteras fullpris.";
                        }
                    }
                    else
                    {
                        divText += "• Tiden ligger utanför abonnemangets tidsintervall (<strong>6 - 16</strong>) och debiteras fullpris.";
                    }

                }
            }

            bookingInfoText.InnerHtml = divText;


            bookingInfoTextDiv.Controls.Add(bookingInfoText);

            return bookingInfoTextDiv;
        }
        
        /// <summary>
        /// Gör en string med information om bokningen
        /// </summary>
        /// <param name="lip"></param>
        /// <returns>En sträng med bokningsinformationstexten</returns>
        public string BookingInfoString(LoggedInPerson lip)
        {

            DateTime chosenDay = DateTime.Now;
            string divText = "";

            if (lip.member.MemberType == 2)
            {
                divText += "• Fullpris för ströbokningar debiteras kontoansvarig.";
            }
            else if (lip.member.MemberType == 3)
            {
                int resLeft = HasFloatReservation(chosenDay, lip);

                if (resLeft > 0)
                {
                    divText += "• Ni har <strong>" + resLeft + "</strong> fria bokningar kvar för nuvarande vecka, dagtid mellan <strong>6 - 16</strong>. Övriga bokningar debiteras fullpris.";

                }
                else
                {
                    divText += "• Ni har <strong>inga</strong> fria bokningar kvar för nuvarande veckan, vid bokning debiteras fullpris.";
                }
            }

            return divText;
        }


        /// <summary>
        /// En DataTable som används vid statistiken med namn och med antalet bokningar
        /// </summary>
        /// <param name="startDate">Startdatum</param>
        /// <param name="endDate">Slutdatum</param>
        /// <returns>En Datatable med resultatet av SQLqueryn</returns>
        public DataTable PlayerStats(DateTime startDate, DateTime endDate)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
            MySqlConnection conn = myConn();
            DataTable dt = new DataTable();

            DateTime startMonth = Convert.ToDateTime(startDate.ToString("yyyy-MM"));
            DateTime endMonth = Convert.ToDateTime(endDate.ToString("yyyy-MM"));


            string query = "SELECT CONCAT(CONCAT(UCASE(LEFT(u.Firstname, 1)), LCASE(SUBSTRING(u.Firstname, 2))), ' ', CONCAT(UCASE(LEFT(u.Surname, 1)), LCASE(SUBSTRING(u.Surname, 2)))) AS name, COUNT(*) as NoOfReservations FROM reservations r, users u "
                            + "INNER JOIN members m ON m.UserId = u.UserId "
                            + "WHERE r.StartDate BETWEEN '" + startMonth + "' AND DATE('" + endMonth + "') AND r.ReservationType != 3 AND r.MemberId = m.MemberId "
                            + "GROUP BY r.MemberId "
                            + "ORDER BY NoOfReservations DESC, Surname ASC, Firstname ASC LIMIT 5; ";



            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            da.Fill(dt);
            conn.Close();
            conn.Dispose();
            da.Dispose();

            return dt;
        }


        #endregion


        #region BookingTable

        /// <summary>
        /// En tabell som innehåller alla bokningarna för lip. Gäller reservationer(ströbokningar) och även abonnemangstider
        /// </summary>
        /// <param name="lip">Logged in person = inloggad person</param>
        /// <returns>En HTML-tabell som sedan läggs i en div</returns>
        public HtmlTable MyBookingsTable(LoggedInPerson lip)
        {
            List<Tuple<Reservations, Courts, ReservationTypes>> bookingInfoList = GetResTuples(lip); //En tuplelista som innehåller 3 olika klasser (Reservations, Courts, ReservationTypes).
            List<Tuple<Subscriptions, CourtTimes, Days>> subscriptionInfoList = GetSubTuples(lip); //En tuplelista som innehåller 3 olika klasser (Subscriptions, CourtTimes, Days).
            MySqlConnection conn = myConn();

            if (bookingInfoList.Count > 0 || subscriptionInfoList.Count > 0) //Kontrollerar om anringen reservationstuplelistan eller abonnemangstuplelistan har mer än 0 item.
            {
                HtmlTable bookingsTable = new HtmlTable(); //Skapar en htmltabell
                bookingsTable.Attributes.Add("id", "bookingsTable");

                HtmlTableRow th = new HtmlTableRow(); //Tabellrad som blir rubriken
                th.Attributes.Add("class", "myBookingsTR");

                for (int i = 0; i < 5; i++) //For-loopen är 5 eftersom det är 5 olika värden i varje cell.
                {
                    HtmlTableCell td = new HtmlTableCell(); //tabellcell
                    td.Attributes.Add("class", "myBookingsTH");//Raden får klassen som en tableheader, alltså tabellkolumnsrubrik

                    if (i == 0)
                    {
                        td.InnerText = "Datum"; //Första cellen får "Datum" som rubrik
                    }
                    if (i == 1)
                    {
                        td.InnerText = "Tid"; //Andra cellen får "Tid" som rubrik
                    }
                    if (i == 2)
                    {
                        td.InnerText = "Bana"; //Tredje cellen får "Bana" som rubrik
                    }
                    if (i == 3)
                    {
                        td.InnerText = "Pris"; //Fjärde raden får "Pris" som rubrik
                    }
                    if (i == 4)
                    {
                        td.InnerText = "Avboka"; //Femte raden får "Avboka" som rubrik
                    }

                    th.Controls.Add(td); //Adderar cellerna till raden
                }
                bookingsTable.Controls.Add(th); //Adderar tabellcellsrubrikerna till tabellen



                foreach (Tuple<Reservations, Courts, ReservationTypes> t in bookingInfoList) //För varje reservation i reservationstuplelistan. Reservations = item1, Courts = item2, ReservationTypes = item3
                {
                    //Svenska som "kultur" så det blir rätt med tid.
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");

                    HtmlTableRow tr = new HtmlTableRow(); //Ny rad i tabellen
                    tr.Attributes.Add("class", "myBookingsTR"); //Raden får en CSS-klass

                    string dayOfWeek = "";
                    string shortDayName = "";
                    string dateOfDate = "";
                    string monthNumber = "";

                    for (int x = 0; x < 5; x++) //Likadan For-loop som tidigare.
                    {
                        HtmlTableCell td = new HtmlTableCell(); //Ny cell för den aktuella raden
                        td.Attributes.Add("class", "myBookingsTD"); //Cellen får en CSS-klass
                        if (x == 0)
                        {
                            DateTime dateOfBooking = Convert.ToDateTime(t.Item1.StartDate.ToShortDateString());
                            dayOfWeek = dateOfBooking.ToString("dddd", new CultureInfo("sv-SE")); //Dagens namn
                            shortDayName = FixName(dayOfWeek.Substring(0, 3)); //Anropar Fixname() som returnerar de tre första bokstäverna i namnet på dagen, ex "Ons"
                            dateOfDate = dateOfBooking.ToString("%d", new CultureInfo("sv-SE")); //Datumet. Ex "5" eller "17"
                            monthNumber = dateOfBooking.ToString("%M", new CultureInfo("sv-SE")); //Månadsnumret. Ex "2" eller "12".

                            td.InnerText = shortDayName + " " + dateOfDate + "/" + monthNumber;
                        }
                        if (x == 1)
                        {
                            td.InnerText = t.Item1.StartDate.ToShortTimeString(); //Första itemet i "t" och skriver ut i cellen. I det här fallet datumet
                        }
                        if (x == 2)
                        {
                            td.InnerText = t.Item2.CourtId.ToString(); //Andra itemet i "t" och skriver ut i cellen. I det här fallet ID för banan
                        }
                        if (x == 3)
                        {
                            td.InnerText = t.Item3.Cost.ToString() + ":-"; //tredje itemet i "t" och skriver ut i cellen. I det här fallet kostnad
                        }
                        if (x == 4)
                        {

                            if ((t.Item1.StartDate > DateTime.Now.AddHours(1) && !HasCLRequest(lip)) || (t.Item1.StartDate.Date >= DateTime.Now.Date.AddDays(1))) //kontrollerar om datumet för reservationen är större än nu+1timme. och om lip inte har en CodeLockRequest. ELLER om reservationsdatumet är större än dagensdatum + 1.
                            {
                                string thisDayIsFullDate = t.Item1.StartDate.ToString("yyyy-MM-dd", new CultureInfo("sv-SE")); //hela datumet.
                                string sTime = t.Item1.StartDate.ToShortTimeString();
                                string shortTime = sTime.Substring(0, 2); //Skriver ut timmen med 2 siffror.

                                string id = "cb_" + t.Item1.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime;

                                HtmlGenericControl cbCancelReservation = new HtmlGenericControl("input");
                                cbCancelReservation.Attributes.Add("type", "checkbox");
                                cbCancelReservation.Attributes.Add("id", "cb_" + t.Item1.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime);
                                cbCancelReservation.Attributes.Add("value", "Bana " + t.Item1.CourtId.ToString() + " " + shortDayName + " " + dateOfDate + "/" + monthNumber + " " + shortTime + ":00");
                                cbCancelReservation.Attributes.Add("class", "cbCancelReservation");
                                cbCancelReservation.Attributes.Add("onclick", "checkOrUncheck('" + id + "')");

                                td.Controls.Add(cbCancelReservation);
                            }
                            #region bortkommenterad else if som ligger kvar just in case.
                            //else if (t.Item1.StartDate.Date >= DateTime.Now.Date.AddDays(1))
                            //{
                            //    string thisDayIsFullDate = t.Item1.StartDate.ToString("yyyy-MM-dd", new CultureInfo("sv-SE"));
                            //    string sTime = t.Item1.StartDate.ToShortTimeString();
                            //    string shortTime = sTime.Substring(0, 2);

                            //    string id = "cb_" + t.Item1.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime;

                            //    HtmlInputCheckBox cbCancelReservation = new HtmlInputCheckBox();
                            //    cbCancelReservation.Attributes.Add("id", "cb_" + t.Item1.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime);
                            //    cbCancelReservation.Attributes.Add("value", "Bana " + t.Item1.CourtId.ToString() + " " + shortDayName + " " + dateOfDate + "/" + monthNumber + " " + shortTime + ":00");
                            //    cbCancelReservation.Attributes.Add("class", "cbCancelReservation");
                            //    cbCancelReservation.Attributes.Add("onclick", "checkOrUncheck('" + id + "')");
                            //    cbCancelReservation.Attributes.Add("runat", "server");

                            //    td.Controls.Add(cbCancelReservation);

                            //}
                            #endregion
                        }

                        tr.Controls.Add(td); //Adderar cell till rad.
                    }
                    bookingsTable.Rows.Add(tr); //Adderar rad till tabell.
                }





                foreach (Tuple<Subscriptions, CourtTimes, Days> tup in subscriptionInfoList)
                {
                    HtmlTableRow tr = new HtmlTableRow(); //ny rad
                    tr.Attributes.Add("class", "mySubscriptionsTR"); //raden får CSS-klass

                    for (int y = 0; y < 5; y++)
                    {
                        HtmlTableCell td = new HtmlTableCell(); //ny cell
                        td.Attributes.Add("class", "mySubscriptionsTD"); //cellen får en CSS-klass

                        if (y == 0)
                        {
                            td.InnerText = EngSweDaySwitch(tup.Item3.Description) + "ar"; //Dagens namn på svenska i plural
                        }

                        if (y == 1)
                        {
                            td.InnerText = tup.Item2.StartHour + ":00"; //Ex "16:00"
                        }
                        if (y == 2)
                        {
                            td.InnerText = tup.Item1.CourtId.ToString();
                        }
                        if (y == 3)
                        {
                            td.InnerHtml = "<i>Abonnemang</i>";
                        }
                        if (y == 4)
                        {

                        }

                        tr.Controls.Add(td);
                    }
                    bookingsTable.Rows.Add(tr);
                }

                return bookingsTable; //Returnerar tabellen

            }
            else
            {
                return null;
            }


        }
        
        /// <summary>
        /// Hämtar PIN-koder från CodeLock i databasen och sorterar på det senaste.
        /// </summary>
        /// <returns>En lista med PIN-koder</returns>
        public List<CodeLock> GetCodeLocks()
        {
            MySqlConnection conn = myConn();
            List<CodeLock> codeLockList = new List<CodeLock>();
            string queryGetCodeLocks = "SELECT CodeLockId, CodeLock, DateOfChange FROM codelock ORDER BY DateOfChange DESC;";


            MySqlDataReader dr2 = myReader(queryGetCodeLocks, conn);

            while (dr2.Read())
            {
                CodeLock cl = new CodeLock();
                cl.CodeLockId = Convert.ToInt16(dr2["CodeLockId"]);
                cl.Code = dr2["CodeLock"].ToString();
                cl.DateOfChange = Convert.ToDateTime(dr2["DateOfChange"]);

                codeLockList.Add(cl);
            }
            return codeLockList;
        }

        /// <summary>
        /// Gör en lista av Tuple för reservationer med lip från databasen
        /// </summary>
        /// <param name="lip">Logged In Person = Inloggad person</param>
        /// <returns>En lista av tuple</returns>
        public List<Tuple<Reservations, Courts, ReservationTypes>> GetResTuples(LoggedInPerson lip)
        {
            MySqlConnection conn = myConn();

            List<Tuple<Reservations, Courts, ReservationTypes>> reservationInfoList = new List<Tuple<Reservations, Courts, ReservationTypes>>();

            DateTime d = DateTime.Now.AddHours(-1);


            string query = "SELECT r.CourtId AS RCourtId, r.MemberId AS RMemberId, r.StartDate, r.HandledBy, r.ReservationType AS RResType, c.CourtId AS CCourtId, c.Description AS courtName, rt.ReservationTypeId AS resTypeId, rt.Description AS resType, rt.Cost FROM reservations r "
                           + "INNER JOIN courts c ON c.CourtId = r.CourtId "
                           + "INNER JOIN reservationtypes rt ON rt.ReservationTypeId = r.ReservationType "
                           + "WHERE r.MemberId = '" + lip.member.MemberId + "' AND DATE(StartDate) >= DATE(NOW()) ORDER BY StartDate;";

            MySqlDataReader dr = myReader(query, conn);

            while (dr.Read())
            {
                Reservations r = new Reservations();
                r.CourtId = Convert.ToInt16(dr["RCourtId"]);
                r.MemberId = Convert.ToInt16(dr["RMemberId"]);
                r.StartDate = Convert.ToDateTime(dr["StartDate"]);
                if (dr["HandledBy"] != DBNull.Value)
                {
                    r.HandledBy = Convert.ToInt16(dr["HandledBy"]);
                }
                else
                {
                    r.HandledBy = 0;
                }
                if (dr["RResType"] != DBNull.Value)
                {
                    r.ReservationType = Convert.ToInt16(dr["RResType"]);
                }
                else
                {
                    r.ReservationType = 0;
                }

                Courts c = new Courts();
                c.CourtId = Convert.ToInt16(dr["CCourtId"]);
                c.Description = dr["courtName"].ToString();


                ReservationTypes rt = new ReservationTypes();
                rt.ReservationTypeId = Convert.ToInt16(dr["resTypeId"]);
                rt.Description = dr["resType"].ToString();
                rt.Cost = Convert.ToInt16(dr["Cost"]);


                Tuple<Reservations, Courts, ReservationTypes> tupleResList = new Tuple<Reservations, Courts, ReservationTypes>(r, c, rt);
                reservationInfoList.Add(tupleResList);
            }
            conn.Close();
            return reservationInfoList;
        }

        /// <summary>
        /// Gör en lista av Tuple för abonnemang med lip från databasen
        /// </summary>
        /// <param name="lip">Logged In Person = Inloggad person</param>
        /// <returns>En lista av tuple</returns>
        public List<Tuple<Subscriptions, CourtTimes, Days>> GetSubTuples(LoggedInPerson lip)
        {
            MySqlConnection conn = myConn();

            List<Tuple<Subscriptions, CourtTimes, Days>> subscriptionInfoList = new List<Tuple<Subscriptions, CourtTimes, Days>>();

            string query = "SELECT d.DayID, d.Description, ct.CourtTimeId, ct.StartHour, ct.Active, s.CourtId, s.CourtTimeId AS sCTId, s.DayId AS sDId, s.MemberId AS sMId FROM subscriptions s "
                            + "INNER JOIN courts c ON s.CourtId = c.CourtId "
                            + "INNER JOIN courttimes ct ON s.CourtTimeId = ct.CourtTimeId "
                            + "INNER JOIN days d ON s.DayId = d.DayId "
                            + "WHERE s.MemberId = " + lip.member.MemberId + " ORDER BY s.DayId;";

            MySqlDataReader dr = myReader(query, conn);

            while (dr.Read())
            {
                Subscriptions s = new Subscriptions();
                s.CourtId = Convert.ToInt16(dr["CourtId"]);
                s.CourtTimeId = Convert.ToInt16(dr["sCTId"]);
                s.DayId = Convert.ToInt16(dr["sDId"]);
                s.MemberId = Convert.ToInt16(dr["sMId"]);



                CourtTimes ct = new CourtTimes();
                ct.StartHour = Convert.ToInt16(dr["StartHour"]);
                ct.CourtTimeId = Convert.ToInt16(dr["CourtTimeId"]);
                ct.Active = dr.GetBoolean(dr.GetOrdinal("Active"));


                Days d = new Days();
                d.Description = dr["Description"].ToString();
                d.DayId = Convert.ToInt16(dr["DayId"]);

                Tuple<Subscriptions, CourtTimes, Days> tupleSubList = new Tuple<Subscriptions, CourtTimes, Days>(s, ct, d);
                subscriptionInfoList.Add(tupleSubList);
            }
            conn.Close();
            return subscriptionInfoList;

        }
        #endregion

        #region UserManagemet

        /// <summary>
        /// En metod som hämtar alla users från users_updated-tabellen i databasen och lägger dessa i en lista.
        /// </summary>
        /// <returns>En sorterad lista som är i första hand sorterad på förnamn och i andra hand på efternamn </returns>
        public List<Users> GetUserList()
        {
            MySqlConnection conn = myConn();
            string query = "SELECT * FROM users_updated ORDER BY Firstname ASC";
            MySqlDataReader dr = myReader(query, conn);
            List<Users> uList = new List<Users>();

            try
            {
                while (dr.Read())
                {
                    Users u = new Users();
                    u.Id = Convert.ToInt16(dr["Id"]);
                    u.UserId = Convert.ToInt16(dr["UserId"]);
                    u.FirstName = FixName(dr["Firstname"].ToString());
                    u.SurName = FixName(dr["Surname"].ToString());
                    u.Phone = SplitNumber(FixNumber(dr["Phone"].ToString()));
                    u.EMail = dr["EMail"].ToString().ToLower(); ;
                    u.StreatAddress = FixName(dr["StreetAddress"].ToString());
                    u.ZipCode = FixNumber(dr["ZipCode"].ToString());
                    u.City = FixName(dr["City"].ToString());
                    u.Password = dr["Password"].ToString();
                    u.Cellular = dr["Cellular"].ToString();
                    u.PublicAddres = Convert.ToInt32(dr["PublicAddress"].ToString());

                    uList.Add(u);
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            List<Users> sortedList = uList.OrderBy(u => u.FirstName).ThenBy(u => u.SurName).ToList();
            return sortedList;
        }

        #endregion





        /// <summary>
        /// Tar emot namnet på en veckodag på engelska och gör om det till svenska
        /// </summary>
        /// <param name="engDay">Namnet på veckodagen på engelska</param>
        /// <returns>Det svenska namnet på veckodagen</returns>
        public string EngSweDaySwitch(string engDay)
        {
            string sweDay = null;

            if (engDay == "Monday")
            {
                sweDay = "Måndag";
            }
            if (engDay == "Tuesday")
            {
                sweDay = "Tisdag";
            }
            if (engDay == "Wednesday")
            {
                sweDay = "Onsdag";
            }
            if (engDay == "Thursday")
            {
                sweDay = "Torsdag";
            }
            if (engDay == "Friday")
            {
                sweDay = "Fredag";
            }
            if (engDay == "Saturday")
            {
                sweDay = "Lördag";
            }
            if (engDay == "Sunday")
            {
                sweDay = "Söndag";
            }

            return sweDay;
        }


        /// <summary>
        /// En metod som letar kontroller på en sida. Den loopar om och om igen.
        /// </summary>
        /// <param name="root">Rootkontrollen, var man vill börja leta efter den önskade kontrollen</param>
        /// <param name="id">Den önskade kontrollen</param>
        /// <returns>Antingen returneras rootkontrollen, eller den önskade kontrollen eller null</returns>
        public Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }
        

        /// <summary>
        /// Metoden kontrollerar om lip har en CodeLockRequest i databasen
        /// </summary>
        /// <param name="lip">Logged In Person = Inloggad person</param>
        /// <returns>Sant eller falskt beroende på om det fins en CodeLockRequest eller inte</returns>
        public bool HasCLRequest(LoggedInPerson lip)
        {
            string query = "SELECT COUNT(*) AS c FROM codelockrequests WHERE MemberId = " + lip.member.MemberId + " AND DATE(DateOfRequest) = CURDATE() ORDER BY DateOfRequest DESC";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            if (dr.Read() && Convert.ToInt16(dr["c"]) != 0)
            {
                dr.Close();
                dr.Dispose();
                conn.Close();
                conn.Dispose();
                return true;
            }
            else
            {
                dr.Close();
                dr.Dispose();
                conn.Close();
                conn.Dispose();
                return false;
            }
        }



        /// <summary>
        /// Metoden kontrollerar om lip har en CodeLockRequest i databasen för chosenDay
        /// </summary>
        /// <param name="lip">Logged In Person = Inloggad person</param>
        /// <param name="chosenDay">Den valda dagen</param>
        /// <returns>Sant eller falskt beroende på om det fins en CodeLockRequest eller inte</returns>
        public bool HasCLRequest(LoggedInPerson lip, DateTime chosenDay)
        {
            string query = "SELECT COUNT(*) AS c FROM codelockrequests WHERE MemberId = " + lip.member.MemberId + " AND DATE(DateOfRequest) = DATE('" + chosenDay + "') ORDER BY DateOfRequest DESC";
            MySqlConnection conn = myConn();
            MySqlDataReader dr = myReader(query, conn);

            if (dr.Read() && Convert.ToInt16(dr["c"]) != 0)
            {
                dr.Close();
                dr.Dispose();
                conn.Close();
                conn.Dispose();
                return true;
            }
            else
            {
                dr.Close();
                dr.Dispose();
                conn.Close();
                conn.Dispose();
                return false;
            }
        }



    }
}