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
    public partial class report5 : System.Web.UI.Page
    {
        DateTime today = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (today < DateTime.Parse(today.ToString("yyyy-MM") + "-26"))
                {
                    dpMonthDateS.SelectedDate = DateTime.Parse(today.AddMonths(-1).ToString("yyyy-MM") + "-26");
                    dpMonthDateE.SelectedDate = DateTime.Parse(today.ToString("yyyy-MM") + "-25");
                }
                else
                {
                    dpMonthDateS.SelectedDate = DateTime.Parse(today.ToString("yyyy-MM") + "-26");
                    dpMonthDateE.SelectedDate = DateTime.Parse(today.AddMonths(1).ToString("yyyy-MM") + "-25");
                }                
                BindGrid7();
            }
        }
        protected void btnMonthSubmit_Click(object sender, EventArgs e)
        {
            BindGrid7();
        }
        private DataTable GetMonthGrid(string s,string e)
        {
            string sql = "select sum(ordernum) ordernum,case when o.userno='26712071' then '' else o.userno end userno,o.username,o.shiftname from omorder o left join omlunch l on l.lunchid=o.lunchid and l.lunchdate=o.orderdate where orderdate between to_date('" + s + "','yyyy-MM-dd') AND to_date('" + e + "','yyyy-MM-dd') and o.ordernum <> 0 group by o.userno, o.username, o.shiftname order by decode(o.shiftname,'其他',1) desc, o.shiftname";
            DataTable table = ltDll.ltClass.SelectFromMes(sql);
            //ltDll.ltClass.PrintHideMessage(sql);
            return table;
        }
        private void BindGrid7()
        {
            DataTable dt6=GetMonthGrid(dpMonthDateS.Text, dpMonthDateE.Text);
            //DataTable dt6 = GetMonthGrid(date.ToShortDateString());
            //DataTable dt7 = OtherMonthGrid(date.ToString("yyyy-MM"));
            //JObject defaultObj = new JObject();
            //int othernum = 0;
            //if (dt7.Rows.Count > 0)
            //{
            //    tbremarks.Text = dt7.Rows[0]["remarks"].ToString();
            //    nbordernum.Text = dt7.Rows[0]["ordernum"].ToString();
            //    othernum =int.Parse(dt7.Rows[0]["ordernum"].ToString());
            //    defaultObj.Add("shiftname", "其他");
            //    defaultObj.Add("userno", "");
            //    defaultObj.Add("username", dt7.Rows[0]["remarks"].ToString());
            //    defaultObj.Add("ordernum", dt7.Rows[0]["ordernum"].ToString());
            //}
            //else
            //{
            //    defaultObj.Add("shiftname", "其他");
            //    defaultObj.Add("userno", "");
            //    defaultObj.Add("username", "");
            //    defaultObj.Add("ordernum", "");
            //}
            //if (dt6.Rows.Count <= 0)
            //{
            //    DataTable dt7 = BindMonthGrid(today.AddMonths(-1).ToShortDateString());
            //    for (int i = 0; i < dt7.Rows.Count; i++)
            //    {
            //        string maxid = ltDll.ltClass.SelectFirstRowMes("select nvl(max(monthid)+1,1) from ommonth");
            //        string sql = string.Format("insert into ommonth (monthid,monthdate,shiftname,usernum) values ({0},'{1}','{2}',{3})", maxid, dt7.Rows[i]["monthdate"].ToString(), dt7.Rows[i]["shiftname"].ToString(), dt7.Rows[i]["usernum"].ToString());
            //        ltDll.ltClass.ExecuteToMes(sql);
            //    }

            //    dt6 = GetMonthGrid(today.AddMonths(-1).ToShortDateString());
            //}


            //Grid7.AddNewRecord(defaultObj, true);
            Grid7.DataSource = dt6;
            Grid7.DataBind();
            Grid7.SummaryData = OutputSummaryUserData(dt6);
        }
        //private DataTable GetMonthGrid(string MonthDate)
        //{
        //    string sql = "select sum(ordernum) ordernum,case when o.userno='26712071' then '' else o.userno end userno,o.username,o.shiftname from omorder o left join omlunch l on l.lunchid=o.lunchid and l.lunchdate=o.orderdate where to_char(orderdate,'yyyy-mm')= to_char(to_date('" + MonthDate + "', 'yyyy-MM-dd'), 'yyyy-mm') and o.ordernum <> 0 group by o.userno, o.username, o.shiftname order by decode(o.shiftname,'其他',1) desc, o.shiftname";
        //    DataTable table = ltDll.ltClass.SelectFromMes(sql);
        //    //ltDll.ltClass.PrintHideMessage(sql);
        //    return table;
        //}
        private JObject OutputSummaryUserData(DataTable source)
        {
            int userTotal = 0;// othernum;
            JObject summary = new JObject();
            foreach (DataRow row in source.Rows)
            {
                userTotal += Convert.ToInt32(row["ordernum"]);
            }
            summary.Add("ordernum", userTotal.ToString());

            return summary;
        }
        protected void btnMonthExcel_Click(object sender, EventArgs e)
        {
            //string ym = DateTime.Parse(dpMonthDate.Text).ToString("yyyy-MM");
            string ym = DateTime.Parse(dpMonthDateE.Text).ToString("yyyy-MM");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + ym + "Report.xls");
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetGrid7TableHtml(ym));
            Response.End();
        }
        private string GetGrid7TableHtml(string ym)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            sb.Append("<tr>");
            foreach (GridColumn column in Grid7.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            //DataTable dt7 = OtherMonthGrid(ym);
            //int othernum = 0;
            //string remarks = "";
            //if (dt7.Rows.Count > 0)
            //{
            //    othernum = int.Parse(dt7.Rows[0]["ordernum"].ToString());
            //    remarks = dt7.Rows[0]["remarks"].ToString();
            //}

            int a = 0;// othernum;
            foreach (GridRow row in Grid7.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < row.Values.Count(); i++)
                {
                    if (i == 3)
                    {
                        a += int.Parse(row.Values[i].ToString());
                    }
                    sb.AppendFormat("<td>{0}</td>", row.Values[i].ToString());
                }
                sb.Append("</tr>");
            }
            //sb.Append("</tr>");
            //sb.Append("<tr>");
            //sb.Append("<td>其他</td>");
            //sb.Append("<td></td>");
            //sb.Append("<td>" + remarks + "</td>");
            //sb.Append("<td>" + othernum + "</td>");
            //sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td></td>");
            sb.Append("<td></td>");
            sb.Append("<td></td>");
            sb.Append("<td>" + a + "</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        //protected void btnMonthSave_Click(object sender, EventArgs e)
        //{
        //    string ym = DateTime.Parse(dpMonthDate.Text).ToString("yyyy-MM");
        //    DataTable dt = OtherMonthGrid(ym);
        //    if (dt.Rows.Count > 0)
        //    {
        //        string sql = string.Format("update ommonth set remarks='{0}',ordernum={1} where monthdate='{2}'", tbremarks.Text, nbordernum.Text, ym);
        //        ltDll.ltClass.ExecuteToMes(sql);
        //    }
        //    else
        //    {
        //        string maxid = ltDll.ltClass.SelectFirstRowMes("select nvl(max(monthid)+1,1) from ommonth");
        //        string sql = string.Format("insert into ommonth (monthid,monthdate,remarks,ordernum) values ({0},'{1}','{2}',{3})", maxid, ym, tbremarks.Text, nbordernum.Text);
        //        ltDll.ltClass.ExecuteToMes(sql);
        //    }
        //    ShowNotify("数据保存成功！", MessageBoxIcon.Success);
        //    BindGrid7(DateTime.Parse(dpMonthDate.Text));

        //}
        //protected void Grid7_PreDataBound(object sender, EventArgs e)
        //{
        //    // 设置LinkButtonField的点击客户端事件
        //    LinkButtonField deleteField = Grid7.FindColumn("Delete") as LinkButtonField;
        //    deleteField.OnClientClick = GetDeleteScript();
        //}
        //private string GetDeleteScript()
        //{
        //    return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, Grid7.GetDeleteSelectedRowsReference(), String.Empty);
        //}
        //protected void btnMonthFind_Click(object sender, EventArgs e)
        //{
        //if (Grid7.GetModifiedData().Count == 0)
        //{                
        //    return;
        //}
        // 修改的现有数据
        //Dictionary<int, Dictionary<string, object>> modifiedDict = Grid7.GetModifiedDict();
        //foreach (int rowIndex in modifiedDict.Keys)
        //{
        //    int rowID = Convert.ToInt32(Grid7.DataKeys[rowIndex][0]);
        //    if (modifiedDict[rowIndex].ContainsKey("shiftname"))
        //    {
        //        ltDll.ltClass.ExecuteToMes("update ommonth set shiftname='" + modifiedDict[rowIndex]["shiftname"] + "' where monthid=" + rowID);
        //    }
        //    if (modifiedDict[rowIndex].ContainsKey("usernum"))
        //    {
        //        ltDll.ltClass.ExecuteToMes("update ommonth set usernum=" + modifiedDict[rowIndex]["usernum"] + " where monthid=" + rowID);
        //    }               
        //}
        // 删除现有数据
        //List<int> deletedRows = Grid7.GetDeletedList();
        //foreach (int rowIndex in deletedRows)
        //{
        //    int rowID = Convert.ToInt32(Grid7.DataKeys[rowIndex][0]);
        //    string sql = string.Format("delete from ommonth where monthid={0}", rowID);
        //    ltDll.ltClass.ExecuteToMes(sql);
        //}

        //DataTable table = GetMonthGrid(dpMonthDate.Text);
        //Grid7.DataSource = table;
        //Grid7.DataBind();
        //Grid7.SummaryData = OutputSummaryUserData(table);
        //ShowNotify("数据保存成功！", MessageBoxIcon.Success);
        //}


    }
}