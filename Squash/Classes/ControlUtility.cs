using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Squash.Classes
{
    public class ControlUtility
    {

        public static List<T> FindControls<T>(Control parent) where T : Control
        {
            List<T> foundControls = new List<T>();

            FindControls<T>(parent, foundControls);

            return foundControls;
        }


        static void FindControls<T>(Control parent, List<T> foundControls) where T : Control
        {
            foreach (Control c in parent.Controls)
            {
                if (c is T)
                foundControls.Add((T)c);
                else if (c.Controls.Count > 0)
                FindControls<T>(c, foundControls);
            }
        }
    }
}
