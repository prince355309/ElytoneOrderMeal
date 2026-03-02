using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class Main : System.Web.UI.Page
    {
        string LA2 = ltDll.ltClass.SessionRead("OrderMeal", "LA2");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (LA2 == "1")
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                Response.Redirect("index.aspx");
            }
        }
    }
}