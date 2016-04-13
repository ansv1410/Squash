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




        #endregion


    }
}