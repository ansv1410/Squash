using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Squash.Classes
{
    public class HiddenFieldWithClass : HiddenField
    {

        [CssClassProperty]
        [DefaultValue("")]
        public virtual string CssClass
        {
            get
            {
                string Value = this.ViewState["CssClass"] as string;
                if (Value == null)
                    Value = "";
                return Value;
            }
            set
            {
                this.ViewState["CssClass"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.CssClass != "")
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
            }
            base.Render(writer);
        }
    }
}
