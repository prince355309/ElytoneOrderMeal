using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class guest : PageBase
    {
        string userno = "26712071";
        DateTime today = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DateTime d = DateTime.Parse(today.ToShortDateString() + " 09:10:00");

                //if (today > d)
                //{
                //    dpdate.SelectedDate = today.AddDays(1);
                //    dpdate.MinDate = today.AddDays(1);
                //}
                //else
                //{
                    dpdate.SelectedDate = today;
                    dpdate.MinDate = today;
                //}
                btnOrder.OnClientClick = Window1.GetShowReference();
                BindWindow();
                BindTable(today);
            }
        }
        private void BindTable(DateTime dt)
        {
            DatePicker1.SelectedDate = dt;
            
            DateTime monday = DateTime.Parse(dt.AddDays(1 - _default.DayOfWeek(dt)).ToShortDateString());           

            Label2.Text = monday.Month + "月" + monday.Day + "日";
            Label2.Label = monday.ToShortDateString();
            Label2.ShowLabel = false;
            Label3.Text = monday.AddDays(1).Month + "月" + monday.AddDays(1).Day + "日";
            Label3.Label = monday.AddDays(1).ToShortDateString();
            Label3.ShowLabel = false;
            Label4.Text = monday.AddDays(2).Month + "月" + monday.AddDays(2).Day + "日";
            Label4.Label = monday.AddDays(2).ToShortDateString();
            Label4.ShowLabel = false;
            Label5.Text = monday.AddDays(3).Month + "月" + monday.AddDays(3).Day + "日";
            Label5.Label = monday.AddDays(3).ToShortDateString();
            Label5.ShowLabel = false;
            Label6.Text = monday.AddDays(4).Month + "月" + monday.AddDays(4).Day + "日";
            Label6.Label = monday.AddDays(4).ToShortDateString();
            Label6.ShowLabel = false;

            DataTable dt1 = _default.omlunch(monday.ToShortDateString(), "自助餐");
            string n1 = ""; string i1 = "";
            if (dt1.Rows.Count > 0)
            {
                n1 = dt1.Rows[0]["LUNCHNAME"].ToString();
                i1 = dt1.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.ToShortDateString(), userno, i1);
                NumberBox1.Text = num;
            }
            else
            {
                NumberBox1.Text = "";
            }
            DataTable dt2 = _default.omlunch(monday.AddDays(1).ToShortDateString(), "自助餐");
            string n2 = ""; string i2 = "";
            if (dt2.Rows.Count > 0)
            {
                n2 = dt2.Rows[0]["LUNCHNAME"].ToString();
                i2 = dt2.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(1).ToShortDateString(), userno, i2);                
                NumberBox2.Text = num;
            }
            else
            {
                NumberBox2.Text = "";
            }
            DataTable dt3 = _default.omlunch(monday.AddDays(2).ToShortDateString(), "自助餐");
            string n3 = ""; string i3 = "";
            if (dt3.Rows.Count > 0)
            {
                n3 = dt3.Rows[0]["LUNCHNAME"].ToString();
                i3 = dt3.Rows[0]["LUNCHID"].ToString();               
                string num = _default.Checked(monday.AddDays(2).ToShortDateString(), userno, i3);
                NumberBox3.Text = num;

            }
            else
            {
                NumberBox3.Text = "";
            }
            DataTable dt4 = _default.omlunch(monday.AddDays(3).ToShortDateString(), "自助餐");
            string n4 = ""; string i4 = "";
            if (dt4.Rows.Count > 0)
            {
                n4 = dt4.Rows[0]["LUNCHNAME"].ToString();
                i4 = dt4.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(3).ToShortDateString(), userno, i4);
                NumberBox4.Text = num;
            }
            else
            {
                NumberBox4.Text = "";
            }
            DataTable dt5 = _default.omlunch(monday.AddDays(4).ToShortDateString(), "自助餐");
            string n5 = ""; string i5 = "";
            if (dt5.Rows.Count > 0)
            {
                n5 = dt5.Rows[0]["LUNCHNAME"].ToString();
                i5 = dt5.Rows[0]["LUNCHID"].ToString();                
                string num = _default.Checked(monday.AddDays(4).ToShortDateString(), userno, i5);
                NumberBox5.Text = num;
            }
            else
            {
                NumberBox5.Text = "";
            }
            DataTable dt8 = _default.omlunch(monday.ToShortDateString(), "簡餐");
            string n8 = ""; string i8 = "";
            if (dt8.Rows.Count > 0)
            {
                n8 = dt8.Rows[0]["LUNCHNAME"].ToString();
                i8 = dt8.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.ToShortDateString(), userno, i8);                
                NumberBox8.Text = num;
            }
            else
            {
                NumberBox8.Text = "";
            }
            DataTable dt9 = _default.omlunch(monday.AddDays(1).ToShortDateString(), "簡餐");
            string n9 = ""; string i9 = "";
            if (dt9.Rows.Count > 0)
            {
                n9 = dt9.Rows[0]["LUNCHNAME"].ToString();
                i9 = dt9.Rows[0]["LUNCHID"].ToString();                
                string num = _default.Checked(monday.AddDays(1).ToShortDateString(), userno, i9);
                NumberBox9.Text = num;
            }
            else
            {
                NumberBox9.Text = "";
            }
            DataTable dt10 = _default.omlunch(monday.AddDays(2).ToShortDateString(), "簡餐");
            string n10 = ""; string i10 = "";
            if (dt10.Rows.Count > 0)
            {
                n10 = dt10.Rows[0]["LUNCHNAME"].ToString();
                i10 = dt10.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(2).ToShortDateString(), userno, i10);                
                NumberBox10.Text = num;
            }
            else
            {
                NumberBox10.Text = "";
            }
            DataTable dt11 = _default.omlunch(monday.AddDays(3).ToShortDateString(), "簡餐");
            string n11 = ""; string i11 = "";
            if (dt11.Rows.Count > 0)
            {
                n11 = dt11.Rows[0]["LUNCHNAME"].ToString();
                i11 = dt11.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(3).ToShortDateString(), userno, i11);
                NumberBox11.Text = num;
            }
            else
            {
                NumberBox11.Text = "";
            }
            DataTable dt12 = _default.omlunch(monday.AddDays(4).ToShortDateString(), "簡餐");
            string n12 = ""; string i12 = "";
            if (dt12.Rows.Count > 0)
            {
                n12 = dt12.Rows[0]["LUNCHNAME"].ToString();
                i12 = dt12.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(4).ToShortDateString(), userno, i12);
                NumberBox12.Text = num;
            }
            else
            {
                NumberBox12.Text = "";
            }
            DataTable dt15 = _default.omlunch(monday.ToShortDateString(), "麵食");
            string n15 = ""; string i15 = "";
            if (dt15.Rows.Count > 0)
            {
                n15 = dt15.Rows[0]["LUNCHNAME"].ToString();
                i15 = dt15.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.ToShortDateString(), userno, i15);
                NumberBox15.Text = num;
            }
            else
            {
                NumberBox15.Text = "";
            }
            DataTable dt16 = _default.omlunch(monday.AddDays(1).ToShortDateString(), "麵食");
            string n16 = ""; string i16 = "";
            if (dt16.Rows.Count > 0)
            {
                n16 = dt16.Rows[0]["LUNCHNAME"].ToString();
                i16 = dt16.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(1).ToShortDateString(), userno, i16);
                NumberBox16.Text = num;
            }
            else
            {
                NumberBox16.Text = "";
            }
            DataTable dt17 = _default.omlunch(monday.AddDays(2).ToShortDateString(), "麵食");
            string n17 = ""; string i17 = "";
            if (dt17.Rows.Count > 0)
            {
                n17 = dt17.Rows[0]["LUNCHNAME"].ToString();
                i17 = dt17.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(2).ToShortDateString(), userno, i17);
                NumberBox17.Text = num;
            }
            else
            {
                NumberBox17.Text = "";
            }
            DataTable dt18 = _default.omlunch(monday.AddDays(3).ToShortDateString(), "麵食");
            string n18 = ""; string i18 = "";
            if (dt18.Rows.Count > 0)
            {
                n18 = dt18.Rows[0]["LUNCHNAME"].ToString();
                i18 = dt18.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(3).ToShortDateString(), userno, i18);
                NumberBox18.Text = num;
            }
            else
            {
                NumberBox18.Text = "";
            }
            DataTable dt19 = _default.omlunch(monday.AddDays(4).ToShortDateString(), "麵食");
            string n19 = ""; string i19 = "";
            if (dt19.Rows.Count > 0)
            {
                n19 = dt19.Rows[0]["LUNCHNAME"].ToString();
                i19 = dt19.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(4).ToShortDateString(), userno, i19);
                NumberBox19.Text = num;
            }
            else
            {
                NumberBox19.Text = "";
            }
            DataTable dt13 = _default.omlunch(monday.AddDays(1).ToShortDateString(), "輕食餐");
            string n13 = ""; string i13 = "";
            if (dt13.Rows.Count > 0)
            {
                n13 = dt13.Rows[0]["LUNCHNAME"].ToString();
                i13 = dt13.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(1).ToShortDateString(), userno, i13);
                NumberBox13.Text = num;
            }
            else
            {
                NumberBox13.Text = "";
            }
            DataTable dt6 = _default.omlunch(monday.AddDays(2).ToShortDateString(), "輕食餐");
            string n6 = ""; string i6 = "";
            if (dt6.Rows.Count > 0)
            {
                n6 = dt6.Rows[0]["LUNCHNAME"].ToString();
                i6 = dt6.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(2).ToShortDateString(), userno, i6);
                NumberBox6.Text = num;
            }
            else
            {
                NumberBox6.Text = "";
            }
            DataTable dt14 = _default.omlunch(monday.AddDays(3).ToShortDateString(), "輕食餐");
            string n14 = ""; string i14 = "";
            if (dt14.Rows.Count > 0)
            {
                n14 = dt14.Rows[0]["LUNCHNAME"].ToString();
                i14 = dt14.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(3).ToShortDateString(), userno, i14);
                NumberBox14.Text = num;
            }
            else
            {
                NumberBox14.Text = "";
            }
            DataTable dt7 = _default.omlunch(monday.AddDays(4).ToShortDateString(), "輕食餐");
            string n7 = ""; string i7 = "";
            if (dt7.Rows.Count > 0)
            {
                n7 = dt7.Rows[0]["LUNCHNAME"].ToString();
                i7 = dt7.Rows[0]["LUNCHID"].ToString();
                string num = _default.Checked(monday.AddDays(4).ToShortDateString(), userno, i7);
                NumberBox7.Text = num;
            }
            else
            {
                NumberBox7.Text = "";
            }
            string num1 = _default.Checked(monday.ToShortDateString(), userno, "0");           
            NumberBox22.Text = (num1 == "0" ? "" : num1);
            string num2 = _default.Checked(monday.AddDays(1).ToShortDateString(), userno, "0");
            NumberBox23.Text = (num2 == "0" ? "" : num2);             
            string num3 = _default.Checked(monday.AddDays(2).ToShortDateString(), userno, "0");
            NumberBox24.Text = (num3 == "0" ? "" : num3);
            string num4 = _default.Checked(monday.AddDays(3).ToShortDateString(), userno, "0");
            NumberBox25.Text = (num4 == "0" ? "" : num4);
            string num5 = _default.Checked(monday.AddDays(4).ToShortDateString(), userno, "0");
            NumberBox26.Text = (num5 == "0" ? "" : num5);

            Label15.Text = _default.replacebr(n6);
            Label16.Text = _default.replacebr(n7);
            Label23.Text = _default.replacebr(n13);
            Label24.Text = _default.replacebr(n14);

            Label18.Text = _default.replacebr(n1);
            Label19.Text = _default.replacebr(n2);
            Label20.Text = _default.replacebr(n3);
            Label21.Text = _default.replacebr(n4);
            Label22.Text = _default.replacebr(n5);
            Label26.Text = _default.replacebr(n8);
            Label27.Text = _default.replacebr(n9);
            Label28.Text = _default.replacebr(n10);
            Label29.Text = _default.replacebr(n11);
            Label30.Text = _default.replacebr(n12);
            Label34.Text = _default.replacebr(n15);
            Label35.Text = _default.replacebr(n16);
            Label36.Text = _default.replacebr(n17);
            Label37.Text = _default.replacebr(n18);
            Label38.Text = _default.replacebr(n19);
        }
        protected void DatePicker1_TextChanged(object sender, EventArgs e)
        {
            DateTime sd = DateTime.Parse(DatePicker1.Text);
            BindTable(sd);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string lunchdate = dpdate.Text;
            string lunchtype = ddltype.SelectedValue;
            string nbnum = nbxnum.Text;
            string lunchid = lblunchid.Text;
            if (lunchid == ""|| taname.Text == "\r\n" || taname.Text == "")
            {
                ShowNotify("沒有 " + lunchtype+" 不能訂餐！", MessageBoxIcon.Error);
            }
            else
            {
                updatelunch(lunchdate, lunchid, nbnum, userno, "客人","其他");
                ShowNotify("訂餐成功！", MessageBoxIcon.Success);
            }
            BindTable(DateTime.Parse(lunchdate));
        }
        public static void updatelunch(string lunchdate, string lunchid, string ordernum1, string userno, string username, string shiftname)
        {
            string sql1 = string.Format("select * from omorder where ORDERDATE=to_date('{0}', 'yyyy-mm-dd') and USERNO='{1}' and lunchid={2}", lunchdate, userno, lunchid);
            DataTable dt1 = ltDll.ltClass.SelectFromMes(sql1);
            if (dt1.Rows.Count > 0)
            {
                string orderid = dt1.Rows[0]["ORDERID"].ToString();
                string ordernum = dt1.Rows[0]["ordernum"].ToString();

                if (ordernum1 != ordernum)
                {
                    if (lunchid == "0")
                    {
                        string sql = string.Format("delete from omorder where orderid={0}", orderid);
                        ltDll.ltClass.ExecuteToMes(sql);
                    }
                    else
                    {
                        string up1 = string.Format("update omorder set LUNCHID={0},ordernum={1},updatedate=sysdate where orderid={2}", lunchid, ordernum1, orderid);
                        ltDll.ltClass.ExecuteToMes(up1);
                    }
                }
            }
            else
            {
                string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
                string in1 = string.Format("insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP,updatedate,username,shiftname) values ({0},to_date('{1}', 'yyyy-mm-dd'),{2},{3},'{4}','{5}',sysdate,'{6}','{7}')", maxid, lunchdate, ordernum1, lunchid, userno, ltDll.ltClass.GetIPAddress(), username, shiftname);
                ltDll.ltClass.ExecuteToMes(in1);
            }
        }
        protected void dpdate_TextChanged(object sender, EventArgs e)
        {
            BindWindow();
        }
        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWindow();
        }
        private void BindWindow()
        {
            if (ddltype.SelectedValue == "素食")
            {
                taname.Text = "素食盒餐";
                lblunchid.Text = "0";
                nbxnum.Text = _default.Checked(dpdate.Text, userno, "0");
            }
            else
            {
                DataTable dt = _default.omlunch(dpdate.Text, ddltype.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    taname.Text = dt.Rows[0]["LUNCHNAME"].ToString();
                    lblunchid.Text = dt.Rows[0]["LUNCHID"].ToString();
                    nbxnum.Text = _default.Checked(dpdate.Text, userno, dt.Rows[0]["LUNCHID"].ToString());
                }
                else
                {
                    taname.Text = "";
                    lblunchid.Text = "";
                    nbxnum.Text = "";
                }
            }
        }
    }
}