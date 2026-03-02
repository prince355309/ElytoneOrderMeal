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
    public partial class report3 : System.Web.UI.Page
    {
        DateTime today = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpOrderDate.SelectedDate = today;
                BindZZ(today.ToShortDateString());
            }
        }
        protected void btnZZSubmit_Click(object sender, EventArgs e)
        {
            DateTime sd = DateTime.Parse(dpOrderDate.Text);
            BindZZ(sd.ToShortDateString());
        }
        private void BindZZ(string date)
        {
            string sql = "select userno,username,shiftname from ltusers a,tblusruserbasis_view b where a.luser=b.userno and b.issuestate=2 and a.LPROGRAM='OrderMeal' and userno not like 'LB%' and userno not in (select userno from omorder where ORDERDATE = to_date('" + date + "', 'yyyy-mm-dd')) order by shiftname";
            DataTable tb1 = ltDll.ltClass.SelectFromMes(sql);
            Grid6.DataSource = tb1;
            Grid6.DataBind();
        }
        protected void btnNotorderExcel_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=notorderreport.xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGrid6TableHtml());
            Response.End();
        }
        private string GetGrid6TableHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in Grid6.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            sb.Append("</tr>");
            foreach (GridRow row in Grid6.Rows)
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