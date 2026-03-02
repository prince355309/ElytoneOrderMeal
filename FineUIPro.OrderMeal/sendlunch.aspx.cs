using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class sendlunch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _default.BindMailTable(DateTime.Now);
        }
        
    }
}