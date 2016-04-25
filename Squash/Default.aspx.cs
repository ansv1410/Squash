﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Squash.Classes;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
//using Squash;
using Squash;


namespace Squash
{
    public partial class _Default : Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();

        protected void Page_Load(object sender, EventArgs e)
        {
            string queryGetNews = "SELECT * FROM News"; //WHERE date.......
            string showDivs = "";

            MySqlDataReader drNews = method.myReader(queryGetNews, conn);
            while (drNews.Read())
            {
                News n = new News();
                n.Id = Convert.ToInt32(drNews["Id"].ToString());
                n.Headline = drNews["Headline"].ToString();
                n.Newstext = drNews["Newstext"].ToString();
                n.Imagepath = drNews["Imagepath"].ToString();
                //n.Imagebin = ;

                showDivs += "<div><h2>" + n.Headline + "</h2><p>" + n.Newstext + "</p><br/><img runat='server' src=" + n.Imagepath + "></div>";
            }
                newsDiv.InnerHtml = showDivs;




            if (!IsPostBack)
            {
                if (Session["lip"] != null)
                {
                    messagesDiv.Visible = true;
                    presentationDiv.Visible = true;
                    //recruitDiv.Visible = true;
                    newsDiv.Visible = true;




                    //string query = "SELECT * FROM users WHERE UserId = 1";
                    //MySqlDataReader dr = method.myReader(query, conn);
                    //try
                    //{
                    //    while (dr.Read())
                    //    {
                    //        testarn.Text = dr["Firstname"].ToString();
                    //    }
                    //}
                    //catch (MySqlException ex)
                    //{
                    //    Debug.WriteLine(ex.Message);
                    //}
                    //finally
                    //{
                    //    conn.Close();
                    //}
                    List<Messages> messages = new List<Messages>();
                    string query = "SELECT * from Messages";
                    MySqlDataReader dr = method.myReader(query, conn);
                    try
                    {
                        while (dr.Read())
                        {
                            Messages m = new Messages();
                            m.Id = Convert.ToInt32(dr["Id"]);
                            m.Message = dr["Messages"].ToString();
                            messages.Add(m);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    foreach (Messages m in messages)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.InnerHtml = m.Message;
                        //messageList.Controls.Add(li);

                    }
                }
                else
                {
                    presentationDiv.Visible = true;
                    newsDiv.Visible = true;
                }
            }
        }

    }
}