using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;


namespace Squash.Classes
{
    public class TitleWithToString:Title
    {
        public override string ToString()
        {
            return Text;
        }
    }
}