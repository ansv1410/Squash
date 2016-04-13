using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class CodeLockRequests
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime DateOfRequest { get; set; }

    }
}