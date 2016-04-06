using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class CodeLock
    {
        public int CodeLockId { get; set; }
        public string Code { get; set; }
        public DateTime DateOfChange { get; set; }
    }
}