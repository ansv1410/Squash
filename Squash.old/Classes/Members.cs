using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class Members
    {
        public int MemberId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public int ApprovedBy { get; set; }
        public string SessionId { get; set; }
        public int MemberType { get; set; }

    }
}