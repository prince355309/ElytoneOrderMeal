using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userno = tbUserNo.Text.ToUpper();
            if (userno == "26712071")
            {
                if (tbPassword.Text == "26712071")
                {
                    Response.Redirect("guest.aspx");
                }
                else
                {
                    ShowNotify("請檢查你的密碼是否正確！", MessageBoxIcon.Error);
                }
            }
            else
            {
                string sql = "select * from ltusers a,tblusruserbasis_view b where a.luser=b.userno and b.issuestate=2 and a.LPROGRAM='OrderMeal' and a.luser='" + userno + "'";
                DataTable dt = ltDll.ltClass.SelectFromMes(sql);
                if (dt.Rows.Count > 0)
                {
                    ltDll.ltClass.SessionWrite("OrderMeal", "USERNO", userno, 30);
                    ltDll.ltClass.SessionWrite("OrderMeal", "USERNAME", dt.Rows[0]["username"].ToString(), 30);
                    ltDll.ltClass.SessionWrite("OrderMeal", "SHIFTNAME", dt.Rows[0]["shiftname"].ToString(), 30);
                    ltDll.ltClass.SessionWrite("OrderMeal", "SHIFTNO", dt.Rows[0]["shiftno"].ToString(), 30);
                    ltDll.ltClass.SessionWrite("OrderMeal", "LA4", dt.Rows[0]["LA4"].ToString(), 30);
                    ltDll.ltClass.SessionWrite("OrderMeal", "LA2", dt.Rows[0]["LA2"].ToString(), 30);
                    if (dt.Rows[0]["LA2"].ToString() == "1")
                    {
                        if (tbPassword.Text == dt.Rows[0]["lpw"].ToString())
                        {
                            Response.Redirect("default.aspx");
                        }
                        else
                        {
                            ShowNotify("請檢查你的密碼是否正確！", MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Response.Redirect("index.aspx");
                    }
                }
                else
                {
                    ShowNotify("你輸入的工號無效！", MessageBoxIcon.Error);
                }
            }
        }
    }
}