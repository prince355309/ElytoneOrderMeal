using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class lunch : PageBase
    {       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpdate.SelectedDate = DateTime.Now.AddDays(4);
                dpdate.MinDate = DateTime.Now.AddDays(1);
                //dpdate.MaxDate = DateTime.Now.AddDays(10);
                DataTable dt = omlunch(dpdate.Text, ddltype.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    taname.Text = dt.Rows[0]["LUNCHNAME"].ToString();
                }
                else
                {
                    taname.Text = "";
                }
            }
        }        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string lunchdate = dpdate.Text;
            string lunchtype = ddltype.SelectedValue;
            string lunchname = taname.Text;
            DataTable dt2 = omlunch(lunchdate, lunchtype);
            if (taname.Text == string.Empty)
            {
                if (dt2.Rows.Count > 0)
                {
                    string sql2 = string.Format("delete from omlunch where lunchid='{0}'", dt2.Rows[0]["lunchid"].ToString());
                    if (ltDll.ltClass.ExecuteToMes(sql2) == true)
                    {
                        ShowNotify("修改成功！", MessageBoxIcon.Success);
                    }
                }
                else
                {
                    ShowNotify("請輸入菜單描述！", MessageBoxIcon.Success);
                }
            }
            else
            {
                if (dt2.Rows.Count > 0)
                {
                    string sql2 = string.Format("update omlunch set LUNCHNAME='{0}' where LUNCHDATE=to_date('{1}','yyyy-mm-dd') and LUNCHTYPE='{2}'", lunchname, lunchdate, lunchtype);
                    if (ltDll.ltClass.ExecuteToMes(sql2) == true)
                    {
                        ShowNotify("修改成功！", MessageBoxIcon.Success);
                        PageContext.RegisterStartupScript("F.getActiveWindow().window.reloadTable();");                        
                    }
                }
                else
                {
                    string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(lunchid)+1,1) from omlunch");
                    string sql3 = string.Format("insert into omlunch (LUNCHDATE,LUNCHTYPE,LUNCHNAME,LUNCHID) values (to_date('{0}', 'yyyy-mm-dd'),'{1}','{2}',{3})", lunchdate, lunchtype, lunchname, maxid);
                    if (ltDll.ltClass.ExecuteToMes(sql3) == true)
                    {
                        ShowNotify("添加成功！", MessageBoxIcon.Success);
                        //PageContext.RegisterStartupScript("F.getActiveWindow().window.reloadTable();");
                    }
                }
            }               
            
        }
        private DataTable omlunch(string lunchdate, string lunchtype)
        {
            string sql = "select * from omlunch where LUNCHDATE=to_date('"+ lunchdate + "','yyyy-mm-dd') and LUNCHTYPE='" + lunchtype + "'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            return dt;
        }
        protected void dpdate_TextChanged(object sender, EventArgs e)
        {            
            DataTable dt = omlunch(dpdate.Text, ddltype.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                taname.Text = dt.Rows[0]["LUNCHNAME"].ToString();
            }
            else
            {
                taname.Text = "";
            }
        }
        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {            
            DataTable dt = omlunch(dpdate.Text, ddltype.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                taname.Text = dt.Rows[0]["LUNCHNAME"].ToString();
            }
            else
            {
                taname.Text = "";
            }
        }
    }
}