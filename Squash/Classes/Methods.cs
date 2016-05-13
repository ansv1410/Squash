using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Sql;
using MySql.Data.Entity;
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
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["squash"].ConnectionString);
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
            
            if(!dr.HasRows)
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
           string content = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Ditt nya lösenord</title></head><body><h2>Ditt nya lösenord</h2><hr /><p>"+ newPW +"</p></body></html>";


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
            catch(System.Net.Mail.SmtpException ex)
            {
                Debug.WriteLine(ex);
            }
            
            

        }


        #endregion

        #region CheckTelephoneNumber

        public string FixNumber(string phonenumber)
        {
            string newNum = "";

            foreach (char c in phonenumber)
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

        #endregion

        #region UpperCaseLowerCase

        public string FixName(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        #endregion


        #region LoggedInBookingTable

        public void ShowMyBookings()
        {

        }

        #endregion

        #region DesignMethods

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
        /// 
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


        #endregion


        #region BookingTable
        public HtmlTable MyBookingsTable(LoggedInPerson lip)
        {
            List<Tuple<Reservations, Courts, ReservationTypes>> bookingInfoList = GetResTuples(lip);
            List<Tuple<Subscriptions, CourtTimes, Days>> subscriptionInfoList = GetSubTuples(lip);
            MySqlConnection conn = myConn();

            if (bookingInfoList.Count > 0 || subscriptionInfoList.Count > 0)
            {
                HtmlTable bookingsTable = new HtmlTable();
                bookingsTable.Attributes.Add("id", "bookingsTable");

                HtmlTableRow th = new HtmlTableRow();
                th.Attributes.Add("class", "myBookingsTR");

                for (int i = 0; i < 5; i++)
                {
                    HtmlTableCell td = new HtmlTableCell();
                    td.Attributes.Add("class", "myBookingsTH");

                    if (i == 0)
                    {
                        td.InnerText = "Datum";
                    }
                    if (i == 1)
                    {
                        td.InnerText = "Tid";
                    }
                    if (i == 2)
                    {
                        td.InnerText = "Bana";
                    }
                    if (i == 3)
                    {
                        td.InnerText = "Pris";
                    }
                    if (i == 4)
                    {
                        td.InnerText = "Avboka";
                    }

                    th.Controls.Add(td);
                }
                bookingsTable.Controls.Add(th);



                //List<HtmlTableRow> trList = new List<HtmlTableRow>();


                foreach (Tuple<Reservations, Courts, ReservationTypes> t in bookingInfoList)
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Attributes.Add("class", "myBookingsTR");
                    //trList.Add(tr);

                    string dayOfWeek = "";
                    string shortDayName = "";
                    string dateOfDate = "";
                    string monthNumber = "";

                    for (int x = 0; x < 5; x++)
                    {
                        HtmlTableCell td = new HtmlTableCell();
                        td.Attributes.Add("class", "myBookingsTD");
                        if (x == 0)
                        {
                            DateTime dateOfBooking = Convert.ToDateTime(t.Item1.StartDate.ToShortDateString());
                            dayOfWeek = dateOfBooking.ToString("dddd", new CultureInfo("sv-SE"));
                            shortDayName = FixName(dayOfWeek.Substring(0, 3));
                            dateOfDate = dateOfBooking.ToString("%d", new CultureInfo("sv-SE"));
                            monthNumber = dateOfBooking.ToString("%M", new CultureInfo("sv-SE"));

                            td.InnerText = shortDayName + " " + dateOfDate + "/" + monthNumber;

                            //td.InnerText = t.Item1.StartDate.ToShortDateString();
                        }
                        if (x == 1)
                        {
                            td.InnerText = t.Item1.StartDate.ToShortTimeString();
                        }
                        if (x == 2)
                        {
                            td.InnerText = t.Item2.CourtId.ToString();
                        }
                        if (x == 3)
                        {
                            td.InnerText = t.Item3.Cost.ToString() +":-";
                        }
                        if (x == 4)
                        {
                            if (t.Item1.StartDate > DateTime.Now.AddHours(1))
                            {
                                string thisDayIsFullDate = t.Item1.StartDate.ToString("yyyy-MM-dd", new CultureInfo("sv-SE"));
                                string sTime = t.Item1.StartDate.ToShortTimeString();
                                string shortTime = sTime.Substring(0, 2);

                                string id = "cb_"+ t.Item1.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime;

                                HtmlInputCheckBox cbCancelReservation = new HtmlInputCheckBox();
                                cbCancelReservation.Attributes.Add("id", "cb_"+ t.Item1.CourtId.ToString() + "_" + thisDayIsFullDate + "_" + shortTime);
                                cbCancelReservation.Attributes.Add("value", "Bana " + t.Item1.CourtId.ToString() + " " + shortDayName + " " + dateOfDate + "/" + monthNumber + " " + shortTime + ":00");
                                cbCancelReservation.Attributes.Add("class", "cbCancelReservation");
                                cbCancelReservation.Attributes.Add("onclick", "checkOrUncheck('" + id + "')");
                                cbCancelReservation.Attributes.Add("runat", "server");

                                td.Controls.Add(cbCancelReservation);
                            }



                            



                        }

                        tr.Controls.Add(td);
                    }
                    bookingsTable.Rows.Add(tr);

                    //myBookingsDiv.Visible = true;
                }





                foreach (Tuple<Subscriptions, CourtTimes, Days> tup in subscriptionInfoList)
                {
                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Attributes.Add("class", "mySubscriptionsTR");

                    for (int y = 0; y < 5; y++)
                    {
                        HtmlTableCell td = new HtmlTableCell();
                        td.Attributes.Add("class", "mySubscriptionsTD");

                        if (y == 0)
                        {
                            td.InnerText = EngSweDaySwitch(tup.Item3.Description) + "ar";
                            //td.InnerText = tup.Item3.Description+"s";
                        }

                        if (y == 1)
                        {
                            td.InnerText = tup.Item2.StartHour + ":00";
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




                return bookingsTable;

            }
            else
            {
                return null;
            }


        }
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


        public string EngSweDaySwitch(string engDay)
        {
            string sweDay = null;

            if(engDay == "Monday")
            {
                sweDay = "Måndag";
            }
            if(engDay == "Tuesday")
            {
                sweDay = "Tisdag";
            }
            if(engDay == "Wednesday")
            {
                sweDay = "Onsdag";
            }
            if(engDay == "Thursday")
            {
                sweDay = "Torsdag";
            }
            if(engDay == "Friday")
            {
                sweDay = "Fredag";
            }
            if(engDay == "Saturday")
            {
                sweDay = "Lördag";
            }
            if(engDay == "Sunday")
            {
                sweDay = "Söndag";
            }

            return sweDay;
        }


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
        //public Control FindControlsRecursive(Control root)
        //{
        //    if (root is HtmlInputCheckBox)
        //    {
        //        return root;
        //    }
        //    foreach (Htmlco c in root.Controls)
        //    {
        //        Control t = FindControlsRecursive(c);
        //        if (t != null)
        //        {
        //            if (t is HtmlInputCheckBox)
        //            {
        //                return t;
        //            }
        //        }
        //    }

        //    return null;
        //}



    }
}