using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Squash.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using Squash.Classes;
using System.Net;
using System.IO;
using System.Globalization;



namespace Squash.Account
{
    public partial class MyPage : System.Web.UI.Page
    {
        static Methods method = new Methods();
        MySqlConnection conn = method.myConn();
        LoggedInPerson lip;
        string pwFailed;
        string emailUpdateFail;

        protected void Page_Load(object sender, EventArgs e)
        {
            pwFailed = Request.QueryString["falsePw"];
            emailUpdateFail = Request.QueryString["EmailExist"];

            if (emailUpdateFail == "True")
            {
                myinfo.Visible = false;
                changepw.Visible = false;

                editinfo.Visible = true;



                Label l = (Label)method.FindControlRecursive(this.Page, "lblMPMessage");
                l.Visible = true;
                l.Text = "E-post finns redan i databasen, vänligen ange en ny.<br />";

                
            }

            else if (pwFailed != "1") //QueryString som returneras i URLen
            {

                editinfo.Visible = false;
                changepw.Visible = false;
                myinfo.Visible = true;

                lip = (LoggedInPerson)Session["lip"];


                lblName.Text = lip.user.FirstName + " " + lip.user.SurName;
                lblStreetAddress.Text = lip.user.StreatAddress;
                lblPostalCode.Text = lip.user.ZipCode;
                lblCity.Text = lip.user.City;
                lblTelephone.Text = lip.user.Phone;
                lblEmail.Text = lip.user.EMail;

                if (lip.user.PublicAddres == 1)
                {
                    lblAgreement.Text = "Du har valt att finnas i klubbens adresslista.";
                }
                else if (lip.user.PublicAddres == 0)
                {
                    lblAgreement.Text = "Du har valt att inte finnas klubbens adresslista.";
                }

            }


            else
            {
                editinfo.Visible = false;
                changepw.Visible = true;
                myinfo.Visible = false;
            }
        }
        
        protected void BtnShowEditInfo_Click(object sender, EventArgs e)
        {
            myinfo.Visible = false;
            changepw.Visible = false;
            
            editinfo.Visible = true;

        }

        protected void BtnShowEditPW_Click(object sender, EventArgs e)
        {
            myinfo.Visible = false;
            editinfo.Visible = false;

            changepw.Visible = true;
        }

    
    }
}