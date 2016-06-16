using System;
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
            string queryGetNews = "SELECT * FROM news ORDER BY Id DESC LIMIT 10"; //Hämtar de senaste 10 nyheterna i databasen
            string queryGetMessages = "SELECT * FROM messages"; //Hämtar alla meddelanden från databasen
            string showNews = "";
            string showMessages = "";

            try
            {
                MySqlDataReader drNews = method.myReader(queryGetNews, conn);

                while (drNews.Read())//Läser in alla nyheterna och lägger ut dem i divar på sidan.
                {
                    News n = new News();
                    n.Id = Convert.ToInt32(drNews["Id"].ToString());
                    n.Headline = drNews["Headline"].ToString();
                    n.Newstext = drNews["Newstext"].ToString();
                    n.Imagepath = drNews["Imagepath"].ToString();
                    //n.Imagebin = ;


                    if (n.Imagepath != "")
                    {
                        showNews += "<div><h2>" + n.Headline + "</h2><p class=" + "foldedText" + ">" + n.Newstext + "</p><br/><img runat='server' class='newsImg' src=" + n.Imagepath + "><br /><hr /></div>";
                    }
                    else
                    {
                        showNews += "<div><h2>" + n.Headline + "</h2><p class=" + "foldedText" + ">" + n.Newstext + "</p><hr /></div>";
                    }
                }
                newsDiv.InnerHtml = showNews;

                MySqlDataReader drMessages = method.myReader(queryGetMessages, conn);
                while (drMessages.Read())//Läser alla meddelanden och lägger ut de i divar på sidan
                {
                    Messages m = new Messages();
                    m.Id = Convert.ToInt32(drMessages["Id"].ToString());
                    m.Headline = drMessages["Headline"].ToString();
                    m.Message = drMessages["Messages"].ToString();

                    showMessages += "<div><p class=" + "foldedText" + "><span class=" + "messageHeaderP" + ">" + m.Headline + "</span> <br /> " + m.Message + "</p></div>";
                }
                //string bookingLink = "<p class='foldedText' id='bookingLinkP'><span class='messageBookingP'>Boka bana <a class='redirectLinks' href='Booking.aspx' title='Till sidan för att boka'>här</a></span></p>";
                string bookingLink = "<p class='foldedText' id='bookingLinkP'><span class='messageBookingP'><a id='bookCourtLink' href='Booking.aspx' title='Till sidan för att boka'>Boka bana</a></span></p>";

                //messagesDiv.InnerHtml = "<h2>Meddelanden</h2>" + bookingLink + showMessages + "<hr />";
                messagesDiv.InnerHtml = bookingLink + "<h2>Meddelanden</h2>" + showMessages + "<hr />";
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }



            if (!IsPostBack)
            {
                if (Session["lip"] != null)
                {
                    messagesDiv.Visible = true;
                    presentationDiv.Visible = true;
                    //recruitDiv.Visible = true;
                    newsDiv.Visible = true;


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