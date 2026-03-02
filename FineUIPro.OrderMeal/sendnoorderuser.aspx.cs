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
    public partial class sendnoorderuser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dt = DateTime.Now.ToShortDateString();
            string week = DateTime.Now.DayOfWeek.ToString();
            if (week != "Saturday" && week != "Sunday")
            {
                if (week == "Friday")
                {
                    dt = DateTime.Now.AddDays(3).ToShortDateString();
                }
                else
                {
                    dt = DateTime.Now.AddDays(1).ToShortDateString();
                }
                StringBuilder body1 = new StringBuilder();
                string sql = "select lower(substr(mail,instr(mail,',',3)+1)) mail,username,shiftname,userno from tblusruserbasis_view left join ltdll_view3 on substr(mail, instr(mail, '*') + 1, instr(mail, '(') - instr(mail, '*') - 1) = tblusruserbasis_view.username left join ltusers on ltusers.luser = tblusruserbasis_view.userno where tblusruserbasis_view.issuestate = 2 and ltusers.LPROGRAM = 'OrderMeal' and userno in (select userno from omorder where ordernum = 0 and lunchid = 0 and ORDERDATE = to_date('" + dt + "', 'yyyy-mm-dd'))  and mail not like '%com.tw%' order by shiftname";
                DataTable tb = ltDll.ltClass.SelectFromMes(sql);
                if (tb.Rows.Count > 0)
                {
                    body1.Append("<table border='1' cellpadding='1' cellspacing='0'> <tr> ");
                    body1.Append("<td>工號</td>");
                    body1.Append("<td>姓名</td>");
                    body1.Append("<td>部門</td>");
                    body1.Append("</tr>");
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        body1.Append("<tr>");
                        body1.Append("<td>" + tb.Rows[i]["userno"].ToString() + "</td>");
                        body1.Append("<td>" + tb.Rows[i]["username"].ToString() + "</td>");
                        body1.Append("<td>" + tb.Rows[i]["shiftname"].ToString() + "</td>");
                        body1.Append("</tr>");
                        if (tb.Rows[i]["mail"].ToString() != "")
                        {
                            StringBuilder body = new StringBuilder();
                            if (week == "Friday")
                            {
                                body.Append(tb.Rows[i]["username"].ToString() + "　您好，您確認周一不訂餐嗎？ 若有錯誤請於16:50前進系統重新訂餐。<br/><br/>");
                            }
                            else
                            {
                                body.Append(tb.Rows[i]["username"].ToString() + "　您好，您確認明天不訂餐嗎？ 若有錯誤請於16:50前進系統重新訂餐。<br/><br/>");
                            }
                            ltDll.ltClass.SendEmail("不訂餐確認訊息", tb.Rows[i]["mail"].ToString(), "", "", body.ToString());
                        }
                    }
                    body1.Append("</table>");
                    body1.Append("<br/>");
                    ltDll.ltClass.SendEmail("不訂餐的員工", "Angela@elytone.com.tw", "", "", body1.ToString());
                }
            }
        }
    }
}