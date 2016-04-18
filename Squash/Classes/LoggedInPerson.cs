using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class LoggedInPerson
    {
        public Users user { get; set; }
        public Members member { get; set; }
        public Logins logins { get; set; }
    }
}