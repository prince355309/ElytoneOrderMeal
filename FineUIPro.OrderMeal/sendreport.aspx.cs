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
    public partial class sendreport : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {
            string week = DateTime.Now.DayOfWeek.ToString();
            if (week != "Saturday" || week != "Sunday")
            {
                StringBuilder body = new StringBuilder();
                string sql = "select to_char(orderdate,'yyyy/mm/dd') 日期,sum(zzcnum) 自助餐,sum(jcnum) 簡餐,sum(msnum) 麵食,sum(ssnum) 素食,sum(qsnum) 輕食餐 from (select o.orderdate,case when l.lunchtype = '自助餐' then sum(o.ordernum) else 0 end zzcnum,case when l.lunchtype = '簡餐' then sum(o.ordernum) else 0 end jcnum,case when l.lunchtype = '麵食' then sum(o.ordernum) else 0 end msnum,case when l.lunchtype = '輕食餐' then sum(o.ordernum) else 0 end qsnum,case when o.lunchid = 0 then sum(o.ordernum) else 0 end ssnum from omorder o left join omlunch l on o.lunchid = l.lunchid and l.lunchdate=o.orderdate where o.ordernum <> 0 group by o.orderdate, l.lunchtype, o.lunchid ) where orderdate=trunc(sysdate) group by orderdate order by orderdate";
                DataTable tb = ltDll.ltClass.SelectFromMes(sql);
                //this.GridView1.DataSource = tb;
                //this.GridView1.DataBind();
                //自動郵件出現亂碼換種方式也不行，真奇怪,只能改自動網頁發送了。
                if (tb.Rows.Count > 0)
                {
                    body.Append("<table border='1' cellpadding='1' cellspacing='0'> <tr> ");
                    body.Append("<td>日期</td>");
                    body.Append("<td>自助餐</td>");
                    body.Append("<td>簡餐</td>");
                    body.Append("<td>麵食</td>");
                    body.Append("<td>素食</td>");
                    body.Append("<td>輕食餐</td>");
                    body.Append("</tr>");
                    body.Append("<tr>");
                    body.Append("<td>" + tb.Rows[0]["日期"].ToString() + "</td>");
                    body.Append("<td>" + tb.Rows[0]["自助餐"].ToString() + "</td>");
                    body.Append("<td>" + tb.Rows[0]["簡餐"].ToString() + "</td>");
                    body.Append("<td>" + tb.Rows[0]["麵食"].ToString() + "</td>");
                    body.Append("<td>" + tb.Rows[0]["素食"].ToString() + "</td>");
                    body.Append("<td>" + tb.Rows[0]["輕食餐"].ToString() + "</td>");
                    body.Append("</tr>");
                    body.Append("</table>");
                    body.Append("<br/>");
                    Response.Write(body);
                    ltDll.ltClass.SendEmail("音律訂餐數量", "chang65101@gmail.com,weian8316@gmail.com", "Angela@elytone.com.tw", "janetxie@lightion.com", body.ToString());
                }
                
            }
        }
    }
}