using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class Reservations
    {
        public int CourtId { get; set; }
        public int MemberId { get; set; }
        public DateTime StartDate { get; set; }
        public int HandledBy { get; set; }
        public int ReservationType { get; set; }

        public string FullMemberName { get; set; }
    }
}