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
    public partial class sendmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltDll.ltClass.SendEmail("添加菜單", "Angela@elytone.com.tw", "", "", "記得添加下周菜單哦！");
            //BindTable(DateTime.Now.AddDays(3));
        }
        //private void BindTable(DateTime dt)
        //{
        //    DateTime monday = dt.AddDays(1 - DayOfWeek(dt));
        //    StringBuilder table = new StringBuilder();
        //    table.Append("<table border='1' cellpadding='1' cellspacing='0'> <tr> ");
        //    table.Append("<td>日期</td>");
        //    table.Append("<td>" + monday.Month + "月" + monday.Day + "日" + "</td>");
        //    table.Append("<td>" + monday.AddDays(1).Month + "月" + monday.AddDays(1).Day + "日" + "</td>");
        //    table.Append("<td>" + monday.AddDays(2).Month + "月" + monday.AddDays(2).Day + "日" + "</td>");
        //    table.Append("<td>" + monday.AddDays(3).Month + "月" + monday.AddDays(3).Day + "日" + "</td>");
        //    table.Append("<td>" + monday.AddDays(4).Month + "月" + monday.AddDays(4).Day + "日" + "</td>");
        //    table.Append("</tr> <tr>");
        //    table.Append("<td>星期</td>");
        //    table.Append("<td>星期一</td>");
        //    table.Append("<td>星期二</td>");
        //    table.Append("<td>星期三</td>");
        //    table.Append("<td>星期四</td>");
        //    table.Append("<td>星期五</td>");
        //    table.Append("</tr> <tr>");
        //    table.Append("<td>自助餐</td>");
        //    table.Append("<td>"+ replacebr(omlunch(monday.ToShortDateString(), "自助餐"))+"</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(1).ToShortDateString(), "自助餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(2).ToShortDateString(), "自助餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(3).ToShortDateString(), "自助餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(4).ToShortDateString(), "自助餐")) + "</td>");
        //    table.Append("</tr> <tr>");
        //    table.Append("<td>簡餐</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.ToShortDateString(), "簡餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(1).ToShortDateString(), "簡餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(2).ToShortDateString(), "簡餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(3).ToShortDateString(), "簡餐")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(4).ToShortDateString(), "簡餐")) + "</td>");
        //    table.Append("</tr> <tr>");
        //    table.Append("<td>麵食</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.ToShortDateString(), "麵食")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(1).ToShortDateString(), "麵食")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(2).ToShortDateString(), "麵食")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(3).ToShortDateString(), "麵食")) + "</td>");
        //    table.Append("<td>" + replacebr(omlunch(monday.AddDays(4).ToShortDateString(), "麵食")) + "</td>");
        //    table.Append("</tr> <tr>");
        //    table.Append("<td>素食</td>");
        //    table.Append("<td>素食盒餐</td>");
        //    table.Append("<td>素食盒餐</td>");
        //    table.Append("<td>素食盒餐</td>");
        //    table.Append("<td>素食盒餐</td>");
        //    table.Append("<td>素食盒餐</td>");
        //    table.Append("</tr> </table> <br/>");
        //    table.Append("<span color='red'>請同仁於今日下班前進系統點選下周餐點.</span>");
        //    string sql = "select lower(substr(mail,instr(mail,',',3)+1)) mail from ltdll_view3 left join tblusruserbasis_view on substr(mail, instr(mail, '*') + 1, instr(mail, '(') - instr(mail, '*') - 1) = tblusruserbasis_view.username left join ltusers on ltusers.luser = tblusruserbasis_view.userno where tblusruserbasis_view.issuestate = 2 and ltusers.LPROGRAM = 'FineUIPro.OrderMeal' and userno not in ('LB173716','LB028001','EB137004')";
        //    DataTable tb = ltDll.ltClass.SelectFromMes(sql);
        //    if (tb.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < tb.Rows.Count; i++)
        //        {
        //            ltDll.ltClass.SendEmail("下周菜單", tb.Rows[i]["mail"].ToString(), "", "", table.ToString());
        //        }
        //    }

        //}
        //private string omlunch(string lunchdate, string lunchtype)
        //{
        //    string sql = "select * from omlunch where LUNCHDATE=to_date('" + lunchdate + "','yyyy-mm-dd') and LUNCHTYPE='" + lunchtype + "'";
        //    DataTable dt = ltDll.ltClass.SelectFromMes(sql);
        //    if (dt.Rows.Count > 0)
        //    {
        //        return dt.Rows[0]["LUNCHNAME"].ToString();
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}

        //public int DayOfWeek(DateTime dt)
        //{
        //    string strDayOfWeek = dt.DayOfWeek.ToString().ToLower();
        //    int Week = 0;
        //    switch (strDayOfWeek)
        //    {
        //        case "monday":
        //            Week = 1;
        //            break;
        //        case "tuesday":
        //            Week = 2;
        //            break;
        //        case "wednesday":
        //            Week = 3;
        //            break;
        //        case "thursday":
        //            Week = 4;
        //            break;
        //        case "friday":
        //            Week = 5;
        //            break;
        //        case "saturday":
        //            Week = 6;
        //            break;
        //        case "sunday":
        //            Week = 7;
        //            break;
        //    }
        //    return Week;

        //}
        //private string replacebr(string str)
        //{
        //    return str.Replace("\r\n", "<br/>");
        //}
    }
}