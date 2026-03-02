using Newtonsoft.Json.Linq;
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
    public partial class report2 : System.Web.UI.Page
    {
        DateTime today = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpRepotStartDate.SelectedDate = today;
                deReportEndDate.SelectedDate = today;

                DataTable table = BindSumGrid(dpRepotStartDate.Text, deReportEndDate.Text);
                Grid5.DataSource = table;
                Grid5.DataBind();
                Grid5.SummaryData = OutputReportSummaryData(table);
            }
        }
        protected void btnReportSubmit_Click(object sender, EventArgs e)
        {
            DataTable table = BindSumGrid(dpRepotStartDate.Text, deReportEndDate.Text);
            Grid5.DataSource = table;
            Grid5.DataBind();
            Grid5.SummaryData = OutputReportSummaryData(table);
        }
        private DataTable BindSumGrid(string StartDate, string EndDate)
        {
            string sql = "select orderdate,sum(zzcnum) zzcnum,sum(jcnum) jcnum,sum(msnum) msnum,sum(ssnum) ssnum,sum(qsnum) qsnum from (select o.orderdate,case when l.lunchtype = '自助餐' then sum(o.ordernum) else 0 end zzcnum,case when l.lunchtype = '簡餐' then sum(o.ordernum) else 0 end jcnum,case when l.lunchtype = '麵食' then sum(o.ordernum) else 0 end msnum,case when l.lunchtype = '輕食餐' then sum(o.ordernum) else 0 end qsnum,case when o.lunchid = 0 then sum(o.ordernum) else 0 end ssnum from omorder o left join omlunch l on o.lunchid = l.lunchid and l.lunchdate=o.orderdate where o.ordernum <> 0 group by o.orderdate, l.lunchtype, o.lunchid ) where orderdate between to_date('" + StartDate + "','yyyy-MM-dd') AND to_date('" + EndDate + "','yyyy-MM-dd') group by orderdate order by orderdate";

            DataTable table = ltDll.ltClass.SelectFromMes(sql);
            ltDll.ltClass.PrintHideMessage(sql);
            return table;
        }
        private JObject OutputReportSummaryData(DataTable source)
        {
            int zzcnumTotal = 0;
            int jcnumTotal = 0;
            int msnumTotal = 0;
            int ssnumTotal = 0;
            int qsnumTotal = 0;
            JObject summary = new JObject();
            foreach (DataRow row in source.Rows)
            {
                zzcnumTotal += Convert.ToInt32(row["zzcnum"]);
                jcnumTotal += Convert.ToInt32(row["jcnum"]);
                msnumTotal += Convert.ToInt32(row["msnum"]);
                ssnumTotal += Convert.ToInt32(row["ssnum"]);
                qsnumTotal += Convert.ToInt32(row["qsnum"]);
            }
            summary.Add("orderdate", "合計");
            summary.Add("zzcnum", zzcnumTotal.ToString());
            summary.Add("jcnum", jcnumTotal.ToString());
            summary.Add("msnum", msnumTotal.ToString());
            summary.Add("ssnum", ssnumTotal.ToString());
            summary.Add("qsnum", qsnumTotal.ToString());

            return summary;

        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=report2.xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGrid5TableHtml());
            Response.End();

        }
        private string GetGrid5TableHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in Grid5.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            sb.Append("</tr>");

            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            int e = 0;
            foreach (GridRow row in Grid5.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < row.Values.Count(); i++)
                {
                    if (i == 1)
                    {
                        a += int.Parse(row.Values[i].ToString());
                    }
                    if (i == 2)
                    {
                        b += int.Parse(row.Values[i].ToString());
                    }
                    if (i == 3)
                    {
                        c += int.Parse(row.Values[i].ToString());
                    }
                    if (i == 4)
                    {
                        d += int.Parse(row.Values[i].ToString());
                    }
                    if (i == 5)
                    {
                        e += int.Parse(row.Values[i].ToString());
                    }
                    string html = row.Values[i].ToString();
                    sb.AppendFormat("<td>{0}</td>", html);
                }
                sb.Append("</tr>");
            }
            sb.Append("<tr>");
            sb.Append("<td>合計</td>");
            sb.Append("<td>" + a + "</td>");
            sb.Append("<td>" + b + "</td>");
            sb.Append("<td>" + c + "</td>");
            sb.Append("<td>" + d + "</td>");
            sb.Append("<td>" + e + "</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}