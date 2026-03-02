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
    public partial class report4 : System.Web.UI.Page
    {
        DateTime today = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpNoorderdate.SelectedDate = today;
                BindNoorder(today.ToShortDateString());
            }
        }
        protected void btnNoorderSubmit_Click(object sender, EventArgs e)
        {
            DateTime sd = DateTime.Parse(dpNoorderdate.Text);
            BindNoorder(sd.ToShortDateString());
        }
        private void BindNoorder(string date)
        {
            string sql = "select userno,username,shiftname,updatedate from omorder where ORDERDATE = to_date('" + date + "', 'yyyy-mm-dd') and ordernum=0 and lunchid=0 order by shiftname";
            DataTable tb1 = ltDll.ltClass.SelectFromMes(sql);
            Grid8.DataSource = tb1;
            Grid8.DataBind();
        }
        protected void btnNoorderExcel_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=noorderreport.xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGrid8TableHtml());
            Response.End();
        }
        private string GetGrid8TableHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in Grid8.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            sb.Append("</tr>");
            foreach (GridRow row in Grid8.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < row.Values.Count(); i++)
                {
                    sb.AppendFormat("<td>{0}</td>", row.Values[i].ToString());
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}