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
    public partial class senduser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            string dt = DateTime.Now.ToShortDateString();
            string week = DateTime.Now.DayOfWeek.ToString();
            if (week != "Saturday" && week != "Sunday")
            {
                StringBuilder body1 = new StringBuilder();
                string sql = $@"select lower(substr(mail, instr(mail, ',', 3) + 1)) mail,
                                    username,
                                    shiftname,
                                    tblusruserbasis_view.userno
                               from tblusruserbasis_view
                               left join ltdll_view3
                                 on substr(mail,
                                           instr(mail, '*') + 1,
                                           instr(mail, '(') - instr(mail, '*') - 1) =
                                    tblusruserbasis_view.username
                               left join ltusers
                                 on ltusers.luser = tblusruserbasis_view.userno
                               left join OrderMealNotice
                                 on OrderMealNotice.UserNo = tblusruserbasis_view.userno
                                and OrderMealNotice.Type = '1'
                              where tblusruserbasis_view.issuestate = 2
                                and ltusers.LPROGRAM = 'OrderMeal'
                                and tblusruserbasis_view.userno not like 'LB%'
                                and mail like '%elytone.com'
                                and OrderMealNotice.isEnable = 1
                                and tblusruserbasis_view.userno not in (select luser
                                                     from ltusers
                                                    where lprogram = 'OrderMeal'
                                                      and LB1 = 1)
                                and tblusruserbasis_view.userno not in
                                    (select userno
                                       from omorder
                                      where ORDERDATE = to_date('{dt}', 'yyyy-mm-dd')) 
                                order by shiftname";
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
                            body.Append(tb.Rows[i]["username"].ToString() + "　您好，提醒您今天還沒訂餐哦，請盡快點選今天午餐， 請到　http://192.168.1.41/T100/jing/OrderMeal/login.aspx　進行訂餐，謝謝！ <br/><br/>");
                            //body.Append("<span color='red'>訂餐方式為每日訂隔天的餐點，並請再次確認訂餐傳送是否成功進入系統 ( 請登出再登入檢查哦 )。</span>。<br/><br/>");
                            //body.Append("1) 隔日之訂餐最晚當天早上<span color='red'>8:40分前</span>，要上系統選訂 。以避免點餐未食用之情況。<br/>");
                            //body.Append("<span color='red'>例：週二午餐，於週一都可以訂餐，最晚週二早上８：４０分前訂餐………以此類推。</span><br/><br/>");
                            //body.Append("<span>2) 當日忘記訂餐者，請勿佔用他人的名額，以避免主餐不夠 ~ ~ 但開放於12:35分後可選用自助餐。</span><br/><br/>");
                            //body.Append("<span>3) 不用餐的同仁，請勾選 [ 不訂餐 ]。</span><br/><br/>");
                            //body.Append("<span color='red'>4) 當日若已知 : 無法用餐、需外出、出差或請假者，請務必於8:40分前取消訂餐，節省資源不造成餐食之浪費。謝謝配合！！</span><br/><br/>");
                            //body.Append("<br/>");
                            //body.Append("1)	請記得您點餐的選擇，或於用餐前上系統確認，避免誤食同仁的餐點 ");
                            //body.Append("<br/>");
                            //body.Append("<span color='red'>2) 當日若已知:不用餐、需外出者或出差者，請務必於8:40分前取消訂餐，節省資源不造成餐食之浪費，謝謝配合</span>");
                            //body.Append("<br/>");
                            ltDll.ltClass.SendEmail("訂餐提醒", tb.Rows[i]["mail"].ToString(), "", "", body.ToString());
                            //ltDll.ltClass.SendEmail("訂餐提醒", "janetxie@lightion.com.cn", "", "", body.ToString());
                        }
                    }
                    body1.Append("</table>");
                    body1.Append("<br/>");
                    ltDll.ltClass.SendEmail("沒有訂餐的員工", "Angela@elytone.com.tw", "", "", body1.ToString());
                    //ltDll.ltClass.SendEmail("沒有訂餐的員工", "janetxie@lightion.com.cn", "", "", body1.ToString());
                }

                // === DingTalk (釘釘) 通知 ===
                // 查詢已啟用 DingTalk 通知 (Type='2', isEnable=1) 且今天尚未訂餐的使用者
                string sqlDD = "select n.UserNo from OrderMealNotice n " +
                    "inner join ltusers u on u.luser = n.UserNo " +
                    "inner join tblusruserbasis_view b on b.userno = n.UserNo " +
                    "where n.Type = '2' and n.isEnable = 1 " +
                    "and b.issuestate = 2 " +
                    "and u.LPROGRAM = 'OrderMeal' " +
                    "and n.UserNo not like 'LB%' " +
                    "and n.UserNo not in (select luser from ltusers where lprogram='OrderMeal' and LB1=1) " +
                    "and n.UserNo not in (select userno from omorder where ORDERDATE = to_date('" + dt + "', 'yyyy-mm-dd'))";
                DataTable tbDD = ltDll.ltClass.SelectFromMes(sqlDD);
                if (tbDD.Rows.Count > 0)
                {
                    for (int i = 0; i < tbDD.Rows.Count; i++)
                    {
                        string userNo = tbDD.Rows[i]["UserNo"].ToString();
                        string ddContent = "提醒您今天還沒訂餐哦，請盡快點選今天午餐，謝謝！";
                        string ddTitle = "訂餐提醒";
                        try
                        {
                            ltDll.ltClass.SendDDByActionCard(userNo, ddContent, ddTitle, "Click", "https://mis.lightion.com.cn:89/");
                        }
                        catch (Exception ex)
                        {
                            // 記錄 DingTalk 發送失敗，但不影響其他通知
                            System.Diagnostics.Debug.WriteLine("DingTalk 發送失敗 UserNo=" + userNo + ", Error=" + ex.Message);
                        }
                    }
                }
            }
        }
    }
}