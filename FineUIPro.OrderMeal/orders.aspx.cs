using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using System.Data;

namespace FineUIPro.OrderMeal
{
    public partial class orders : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                dpdate.SelectedDate = DateTime.Now.AddDays(1);
                dpFormdate.SelectedDate= DateTime.Now.AddDays(3);
                dpFormdate.MinDate = DateTime.Now.AddDays(1);
                dpFormdate.MaxDate = DateTime.Now.AddDays(10);                
                Grid1.DataSource = BindGrid(DateTime.Now.AddDays(1).ToShortDateString(), "");
                Grid1.DataBind();
                BindName();
            }
        }


        #region BindGrid

        private DataTable BindGrid(string orderdate,string userno)
        {
            string where = "1=1";
            if (orderdate != "")
            {
                where += " and orderdate=to_date('" + orderdate + "', 'yyyy-mm-dd')";
            }
            if (userno != "")
            {
                where += " and userno='" + userno + "'";
            }
            string sql = "select o.orderid,o.orderdate,o.ordernum,o.userno, CASE when o.lunchid=0 then '不訂餐' else l.lunchtype end as lunchtype,l.lunchname from omorder o left join omlunch l on o.lunchid=l.lunchid where " + where;
            DataTable table = ltDll.ltClass.SelectFromMes(sql);
            return table;
        }

        #endregion

        #region Events   
        protected void btnFind_Click(object sender, EventArgs e)
        {
            Grid1.DataSource = BindGrid(dpdate.Text, tbxUserno.Text);
            Grid1.DataBind();
        }

        // 保存数据
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strRowID = hfFormID.Text;
            string userno = tbxFormUserno.Text;
            string lunchdate = dpFormdate.Text;
            string lunchtype = ddlFormtype.SelectedValue;
            string ordernum = nbxFromnum.Text;
            string jcuser = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(luser),'0') from ltusers,tblusruserbasis where LA2=1 and LUSER='" + userno + "' and luser=userno(+) and lprogram='OrderMeal'");
            if (String.IsNullOrEmpty(strRowID))
            {
                // 新增  
                string sql = "select * from tblusruserbasis_view where userno='" + userno + "' and issuestate=2";
                DataTable dt = ltDll.ltClass.SelectFromMes(sql);
                string sql3 = string.Format("select * from omorder where ORDERDATE=to_date('{0}', 'yyyy-mm-dd') and USERNO='{1}'", lunchdate, userno);
                DataTable dt3 = ltDll.ltClass.SelectFromMes(sql3);
                if (dt.Rows.Count > 0)
                { 
                    DataTable dt2 = omlunch(lunchdate, lunchtype);
                    if (dt2.Rows.Count > 0)
                    {
                        string lunchid = dt2.Rows[0]["LUNCHID"].ToString();
                        string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
                        if (jcuser!="0")
                        {                            
                            string sql5 = string.Format("insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP) values ({0},to_date('{1}', 'yyyy-mm-dd'),{2},{3},'{4}','{5}')", maxid, lunchdate, ordernum, lunchid, userno, ltDll.ltClass.GetIPAddress());
                            if (ltDll.ltClass.ExecuteToMes(sql5) == true)
                            {
                                ShowNotify("訂餐成功！", MessageBoxIcon.Success);
                            }
                        }
                        else
                        {                            
                            if (dt3.Rows.Count > 0)
                            {
                                ShowNotify(lunchdate + "你已經訂餐！", MessageBoxIcon.Success);
                            }
                            else
                            {                                
                                string sql2 = string.Format("insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP) values ({0},to_date('{1}', 'yyyy-mm-dd'),1,{2},'{3}','{4}')", maxid, lunchdate, lunchid, userno, ltDll.ltClass.GetIPAddress());
                                if (ltDll.ltClass.ExecuteToMes(sql2) == true)
                                {
                                    ShowNotify("訂餐成功！", MessageBoxIcon.Success);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lunchtype == "不訂餐")
                        {                            
                            if (dt3.Rows.Count > 0)
                            {
                                ShowNotify(lunchdate + "你已經訂餐！", MessageBoxIcon.Success);
                            }
                            else
                            {
                                string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
                                string sql6 = string.Format("insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP) values ({0},to_date('{1}', 'yyyy-mm-dd'),0,0,'{2}','{3}')", maxid, lunchdate, userno, ltDll.ltClass.GetIPAddress());
                                if (ltDll.ltClass.ExecuteToMes(sql6) == true)
                                {
                                    ShowNotify("不訂餐成功！", MessageBoxIcon.Success);
                                }
                            }
                        }
                        else
                        {
                            ShowNotify(lunchdate + "沒有" + lunchtype, MessageBoxIcon.Success);
                        }
                    }


                }
                else
                {
                    ShowNotify("你沒有權限！", MessageBoxIcon.Success);
                }                
                
            }
            else
            {
                // 编辑               
                string ip = ltDll.ltClass.GetIPAddress();
                string addIp = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderip),'NULL') from omorder where orderid=" + strRowID);
                if (addIp != ip)
                {
                    ShowNotify("非資料增加人員,不可以修改 [增加IP:" + addIp + "]", MessageBoxIcon.Success);
                }
                else
                {
                    string lunchid = "0";
                                    
                    if (lunchtype != "不訂餐")
                    {
                        lunchid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(lunchid),'0') from omlunch where lunchdate=to_date('" + lunchdate + "', 'yyyy-mm-dd') and lunchtype='" + lunchtype + "'");
                        if (lunchid == "0")
                        {
                            ShowNotify(lunchdate + "沒有" + lunchtype, MessageBoxIcon.Success);
                        }
                        else
                        {
                            if (jcuser != "0")
                            {
                                string sql = string.Format("update omorder set LUNCHID={0},ordernum={1} where orderid={2}", lunchid,ordernum,strRowID);
                                if (ltDll.ltClass.ExecuteToMes(sql) == true)
                                {
                                    ShowNotify("修改成功！", MessageBoxIcon.Success);
                                }
                            }
                            else
                            {
                                string sql2 = string.Format("update omorder set LUNCHID={0} where orderid={1}", lunchid,strRowID);
                                if (ltDll.ltClass.ExecuteToMes(sql2) == true)
                                {
                                    ShowNotify("修改成功！", MessageBoxIcon.Success);
                                }
                            }
                        }
                    }
                    else
                    {
                        string sql2 = string.Format("update omorder set LUNCHID=0,ORDERNUM=0 where ORDERID={0}", strRowID);
                        if (ltDll.ltClass.ExecuteToMes(sql2) == true)
                        {
                            ShowNotify("修改成功！", MessageBoxIcon.Success);
                        }
                    }
                }
            }
            
            Grid1.DataSource = BindGrid(lunchdate,userno);
            Grid1.DataBind();            
            PageContext.RegisterStartupScript(String.Format("F('{0}').selectRow('{1}');", Grid1.ClientID, strRowID) + Window1.GetHideReference());
        }        
       
        private void BindName()
        {
            DataTable dt = omlunch(dpFormdate.Text, ddlFormtype.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                taname.Text = dt.Rows[0]["LUNCHNAME"].ToString();
            }
            else
            {
                taname.Text = "";
            }
        }
        private DataTable omlunch(string lunchdate, string lunchtype)
        {
            string sql = "select * from omlunch where LUNCHDATE=to_date('" + lunchdate + "','yyyy-mm-dd') and LUNCHTYPE='" + lunchtype + "'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            return dt;
        }
        protected void dpFormdate_TextChanged(object sender, EventArgs e)
        {
            BindName();
        }
        protected void ddlFormtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindName();
        }

        #endregion

        #region Data


        // 根据行ID来获取行数据
        private DataRow FindRowByID(int rowID)
        {
            DataTable table = BindGrid("","");
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["ORDERID"]) == rowID)
                {
                    return row;
                }
            }
            return null;
        }        

      

        #endregion


    }
}
