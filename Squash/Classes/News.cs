using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Squash.Classes
{
    public class News
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Newstext { get; set; }
        public string Imagepath { get; set; }
        public byte[] Imagebin { get; set; }
    }
}