using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using FineUIPro;
using Newtonsoft.Json.Linq;

namespace FineUIPro.OrderMeal
{
    public partial class report : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime today= DateTime.Now;
                dpStartDate.SelectedDate = today;
                dpEndDate.SelectedDate = today;
                DataTable table=BindGrid(today.ToShortDateString(), today.ToShortDateString());
                Grid1.DataSource = table;
                Grid1.DataBind();
                OutputSummaryData(table);
            }
        }
        private void OutputSummaryData(DataTable source)
        {
            int ordernumTotal = 0;
            foreach (DataRow row in source.Rows)
            {
                ordernumTotal += Convert.ToInt32(row["ordernum"]);
            }
            JObject summary = new JObject();
            summary.Add("ordernum", ordernumTotal.ToString());
            Grid1.SummaryData = summary;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable table = BindGrid(dpStartDate.Text, dpEndDate.Text);
            Grid1.DataSource = table;
            Grid1.DataBind();
            OutputSummaryData(table);
        }

        private DataTable BindGrid(string StartDate,string EndDate)
        {
            string sql = string.Format("select case when l.lunchtype is null then '素食' else l.lunchtype end lunchtype , sum(ordernum) ordernum,o.userno,o.username,o.shiftname from omorder o left join omlunch l on l.lunchid=o.lunchid where o.orderdate between to_date('{0}','yyyy-MM-dd') AND to_date('{1}','yyyy-MM-dd') and o.ordernum<>0 group by l.lunchtype, o.userno, o.username, o.shiftname order by decode(lunchtype,'自助餐',1,'簡餐',2,'麵食',3,'素食',4)", StartDate,EndDate);
            DataTable table = ltDll.ltClass.SelectFromMes(sql);
            ltDll.ltClass.PrintHideMessage(sql);
            return table;            
        }

    }
}
