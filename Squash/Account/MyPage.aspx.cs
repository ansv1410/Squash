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
        Members loggedInMember = new Members();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            loggedInMember.MemberId = Convert.ToInt32(Session["MemberId"]);
                
            Response.Write("<script>alert('" + loggedInMember.MemberId + "')</script>");
        }
    }
}