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
    public partial class report1 : System.Web.UI.Page
    {
        DateTime today = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpStartDate.SelectedDate = today;
                dpEndDate.SelectedDate = today;
                DataTable table1 = BindGrid(dpStartDate.Text, dpEndDate.Text, "自助餐");
                if (table1.Rows.Count == 0)
                {
                    Grid1.Hidden = true;
                }
                else
                {
                    Grid1.Hidden = false;
                }
                Grid1.DataSource = table1;
                Grid1.DataBind();
                Grid1.SummaryData = OutputSummaryData(table1);
                DataTable table2 = BindGrid(dpStartDate.Text, dpEndDate.Text, "簡餐");
                if (table2.Rows.Count == 0)
                {
                    Grid2.Hidden = true;
                }
                else
                {
                    Grid2.Hidden = false;
                }
                Grid2.DataSource = table2;
                Grid2.DataBind();
                Grid2.SummaryData = OutputSummaryData(table2);
                DataTable table3 = BindGrid(dpStartDate.Text, dpEndDate.Text, "麵食");
                if (table3.Rows.Count == 0)
                {
                    Grid3.Hidden = true;
                }
                else
                {
                    Grid3.Hidden = false;
                }
                Grid3.DataSource = table3;
                Grid3.DataBind();
                Grid3.SummaryData = OutputSummaryData(table3);
                DataTable table4 = BindGrid(dpStartDate.Text, dpEndDate.Text, "素食");
                if (table4.Rows.Count == 0)
                {
                    Grid4.Hidden = true;
                }
                else
                {
                    Grid4.Hidden = false;
                }
                Grid4.DataSource = table4;
                Grid4.DataBind();
                Grid4.SummaryData = OutputSummaryData(table4);
                DataTable table5 = BindGrid(dpStartDate.Text, dpEndDate.Text, "輕食餐");
                if (table5.Rows.Count == 0)
                {
                    Grid5.Hidden = true;
                }
                else
                {
                    Grid5.Hidden = false;
                }
                Grid5.DataSource = table5;
                Grid5.DataBind();
                Grid5.SummaryData = OutputSummaryData(table5);
            }
        }
        private DataTable BindGrid(string StartDate, string EndDate, string lunchtype)
        {
            string sql = "";
            if (lunchtype == "素食")
            {
                sql = string.Format("select case when l.lunchtype is null then '素食' else l.lunchtype end lunchtype , sum(ordernum) ordernum,case when o.userno='26712071' then '' else o.userno end userno,o.username,o.shiftname,max(updatedate) updatedate from omorder o left join omlunch l on l.lunchid=o.lunchid and l.lunchdate=o.orderdate where o.orderdate between to_date('{0}','yyyy-MM-dd') AND to_date('{1}','yyyy-MM-dd') and o.lunchid=0 and o.ordernum<>0 group by l.lunchtype, o.userno, o.username, o.shiftname order by decode(o.shiftname,'其他',1) desc, o.shiftname", StartDate, EndDate);
            }
            else
            {
                sql = string.Format("select case when l.lunchtype is null then '素食' else l.lunchtype end lunchtype , sum(ordernum) ordernum,case when o.userno='26712071' then '' else o.userno end userno,o.username,o.shiftname,max(updatedate) updatedate from omorder o left join omlunch l on l.lunchid=o.lunchid and l.lunchdate=o.orderdate where o.orderdate between to_date('{0}','yyyy-MM-dd') AND to_date('{1}','yyyy-MM-dd') and lunchtype='{2}' and o.ordernum<>0 group by l.lunchtype, o.userno, o.username, o.shiftname order by decode(o.shiftname,'其他',1) desc, o.shiftname", StartDate, EndDate, lunchtype);
            }

            DataTable table = ltDll.ltClass.SelectFromMes(sql);
            ltDll.ltClass.PrintHideMessage(sql);
            return table;
        }
        private JObject OutputSummaryData(DataTable source)
        {
            int ordernumTotal = 0;
            JObject summary = new JObject();
            foreach (DataRow row in source.Rows)
            {
                ordernumTotal += Convert.ToInt32(row["ordernum"]);
            }
            summary.Add("ordernum", ordernumTotal.ToString());

            return summary;

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable table1 = BindGrid(dpStartDate.Text, dpEndDate.Text, "自助餐");
            if (table1.Rows.Count > 0 &&(ddltype.SelectedValue== "自助餐"|| ddltype.SelectedValue == ""))
            {
                Grid1.Hidden = false;
            }
            else
            {
                Grid1.Hidden = true;
            }
            Grid1.DataSource = table1;
            Grid1.DataBind();
            Grid1.SummaryData = OutputSummaryData(table1);
            DataTable table2 = BindGrid(dpStartDate.Text, dpEndDate.Text, "簡餐");
            if (table2.Rows.Count > 0 && (ddltype.SelectedValue == "簡餐" || ddltype.SelectedValue == ""))
            {
                Grid2.Hidden = false;
            }
            else
            {
                Grid2.Hidden = true;
            }
            Grid2.DataSource = table2;
            Grid2.DataBind();
            Grid2.SummaryData = OutputSummaryData(table2);
            DataTable table3 = BindGrid(dpStartDate.Text, dpEndDate.Text, "麵食");
            if (table3.Rows.Count > 0 && (ddltype.SelectedValue == "麵食" || ddltype.SelectedValue == ""))
            {
                Grid3.Hidden = false;
            }
            else
            {
                Grid3.Hidden = true;
            }
            Grid3.DataSource = table3;
            Grid3.DataBind();
            Grid3.SummaryData = OutputSummaryData(table3);
            DataTable table4 = BindGrid(dpStartDate.Text, dpEndDate.Text, "素食");
            if (table4.Rows.Count > 0 && (ddltype.SelectedValue == "素食" || ddltype.SelectedValue == ""))
            {
                Grid4.Hidden = false;
            }
            else
            {
                Grid4.Hidden = true;
            }
            Grid4.DataSource = table4;
            Grid4.DataBind();
            Grid4.SummaryData = OutputSummaryData(table4);
            DataTable table5 = BindGrid(dpStartDate.Text, dpEndDate.Text, "輕食餐");
            if (table5.Rows.Count > 0 && (ddltype.SelectedValue == "輕食餐" || ddltype.SelectedValue == ""))
            {
                Grid5.Hidden = false;
            }
            else
            {
                Grid5.Hidden = true;
            }
            Grid5.DataSource = table5;
            Grid5.DataBind();
            Grid5.SummaryData = OutputSummaryData(table5);
        }
        protected void btnReportExcel_Click(object sender, EventArgs e)
        {
            string ym = DateTime.Parse(dpEndDate.Text).ToString("yyyy-MM-dd");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=report1_" + ym + ".xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGridTableReport1Html());
            Response.End();

        }
        private string GetGridTableReport1Html()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in Grid1.Columns)
            {
                if (column.HeaderText != "工號")
                {
                    sb.AppendFormat("<td>{0}</td>", column.HeaderText);
                }
            }
            sb.Append("<td></td>");
            sb.Append("</tr>");
            string TD_HTML = "<td>{0}</td>";
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            int e = 0;
            if (ddltype.SelectedValue == "自助餐" || ddltype.SelectedValue == "")
            {
                foreach (GridRow row in Grid1.Rows)
                {
                    a += int.Parse(row.Values[5].ToString());
                    sb.Append("<tr>");
                    sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                    //sb.AppendFormat(TD_HTML, row.Values[1].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[3].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[4].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[5].ToString());
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td></td>");
                //sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td>" + a + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
            if (ddltype.SelectedValue == "簡餐" || ddltype.SelectedValue == "")
            {
                foreach (GridRow row in Grid2.Rows)
                {
                    b += int.Parse(row.Values[5].ToString());
                    sb.Append("<tr>");
                    sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                    //sb.AppendFormat(TD_HTML, row.Values[1].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[3].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[4].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[5].ToString());
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td></td>");
                //sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td>" + b + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
            if (ddltype.SelectedValue == "麵食" || ddltype.SelectedValue == "")
            {
                foreach (GridRow row in Grid3.Rows)
                {
                    c += int.Parse(row.Values[5].ToString());
                    sb.Append("<tr>");
                    sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                    //sb.AppendFormat(TD_HTML, row.Values[1].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[3].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[4].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[5].ToString());
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td></td>");
                //sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td>" + c + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
            if (ddltype.SelectedValue == "素食" || ddltype.SelectedValue == "")
            {
                foreach (GridRow row in Grid4.Rows)
                {
                    d += int.Parse(row.Values[5].ToString());
                    sb.Append("<tr>");
                    sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                    //sb.AppendFormat(TD_HTML, row.Values[1].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[3].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[4].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[5].ToString());
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td></td>");
                //sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td>" + d + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
            if (ddltype.SelectedValue == "輕食餐" || ddltype.SelectedValue == "")
            {
                foreach (GridRow row in Grid5.Rows)
                {
                    e += int.Parse(row.Values[5].ToString());
                    sb.Append("<tr>");
                    sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                    //sb.AppendFormat(TD_HTML, row.Values[1].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[3].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[4].ToString());
                    sb.AppendFormat(TD_HTML, row.Values[5].ToString());
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td></td>");
                //sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("<td>" + e + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            return sb.ToString();
        }
        protected void btnReportExcel2_Click(object sender, EventArgs e)
        {
            string type = ddltype.SelectedValue;
            string ym = DateTime.Parse(dpEndDate.Text).ToString("yyyy-MM-dd");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=report1_"+ Server.UrlEncode(type) + "_" + ym + ".xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGridTableReport1Html2());
            Response.End();

        }
        private string GetGridTableReport1Html2()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;font-size:20px\">");

            sb.Append("<tr>");
            sb.Append("<td>部門</td>");
            sb.Append("<td>姓名</td>");
            sb.Append("<td>請打勾</td>");
            sb.Append("<td>部門</td>");
            sb.Append("<td>姓名</td>");
            sb.Append("<td>請打勾</td>");
            sb.Append("</tr>");
            string TD_HTML = "<td>{0}</td>";
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            int e = 0;
            if (ddltype.SelectedValue == "自助餐")
            {
                foreach (GridRow row in Grid1.Rows)
                {
                    if (a % 2 == 0)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());                        
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                    }
                    else
                    {                       
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    } 
                    a++;
                }
            }
            if (ddltype.SelectedValue == "簡餐")
            {
                foreach (GridRow row in Grid2.Rows)
                {
                    if (b % 2 == 0)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    b++;
                }                
            }
            if (ddltype.SelectedValue == "麵食")
            {
                foreach (GridRow row in Grid3.Rows)
                {
                    if (c % 2 == 0)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    c++;
                }
               
            }
            if (ddltype.SelectedValue == "素食" )
            {
                foreach (GridRow row in Grid4.Rows)
                {
                    if (d % 2 == 0)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    d++;
                }               
            }
            if (ddltype.SelectedValue == "輕食餐")
            {
                foreach (GridRow row in Grid5.Rows)
                {
                    if (e % 2 == 0)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                    }
                    else
                    {
                        sb.AppendFormat(TD_HTML, row.Values[0].ToString());
                        sb.AppendFormat(TD_HTML, row.Values[2].ToString());
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                    }
                    e++;
                }               
            }
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}