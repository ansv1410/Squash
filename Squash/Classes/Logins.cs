using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class Logins
    {
        public int MemberId { get; set; }
        public DateTime LoggedIn { get; set; }
        public string IPAddress { get; set; }

    }
}