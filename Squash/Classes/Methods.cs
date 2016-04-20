using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
            conn.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        public void myDelete(string query, MySqlConnection conn)
        {
            //MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["squash"].ConnectionString);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        #endregion


        #region LogIn
        public bool CheckEmailExist(string email, MySqlConnection conn)
        {
            return true;
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


            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
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
            client.Send(mail);

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

    }
}