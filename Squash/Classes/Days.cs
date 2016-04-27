using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class Days
    {
        public int DayId { get; set; }
        public string Description { get; set; }
        public List<Courts> Courts { get; set; }
        public List<CourtTimes> CourtTimes { get; set; }


    }
}