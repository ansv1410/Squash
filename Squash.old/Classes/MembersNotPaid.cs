using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class MembersNotPaid
    {
        public int MemberId { get; set; }
        public int CourtId { get; set; }
        public DateTime Date { get; set; }
        public int InsertedBy { get; set; }
        public int HandledByMember { get; set; }

    }
}