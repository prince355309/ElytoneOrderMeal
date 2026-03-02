
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class _default : PageBase
    {
        //string userno = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
        //string username = ltDll.ltClass.SessionRead("OrderMeal", "USERNAME");
        //string shiftname = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNAME");
        //string LA4 = ltDll.ltClass.SessionRead("OrderMeal", "LA4");  // Christine新增 代訂者登入即可處理同一部門多位作業員之訂餐
        //string shiftno = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNO");

        string userno = "EB237003";
        protected string username;
        string shiftname = "";
        string LA4 = "0";
        string shiftno = "";
        DateTime today = DateTime.Now;      

        protected void Page_Load(object sender, EventArgs e)
        {
            string _DomainName = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).Host;
            if (_DomainName != "localhost")
            {
                userno = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
                username = ltDll.ltClass.SessionRead("OrderMeal", "USERNAME");
                shiftname = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNAME");
                LA4 = ltDll.ltClass.SessionRead("OrderMeal", "LA4");  // Christine新增 代訂者登入即可處理同一部門多位作業員之訂餐
                shiftno = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNO");
            }
            else
            {
                userno = "EB237003";
                username = "";
                shiftname = "";
                LA4 = "0";
                shiftno = "";
            }

            if (!IsPostBack)
            {
                getEmp();
                ddlLA4.SelectedIndex = ddlLA4.Items.IndexOf(ddlLA4.Items.FindByValue(userno));

                if (LA4 == "1")
                {
                    ddlLA4.Hidden = false;
                }

                // Populate hidden fields for the new card-based UI
                hfUserNo.Value = userno;
                hfUserName.Value = username;
                DateTime monday = GetMondayForAdmin(today);
                var weekData = GetWeekMealsDataAdmin(monday);
                var orderData = GetUserOrdersDataAdmin(monday, userno);
                hfWeekData.Value = JsonConvert.SerializeObject(weekData);
                hfOrderData.Value = JsonConvert.SerializeObject(orderData);

                DatePicker1.SelectedDate = today;
                // BindTable no longer needed - new UI loads data via hfWeekData/hfOrderData hidden fields
                // BindTable(today.AddDays(3));

                if (!addlunchuser())
                {
                    // Hide admin sidebar management section for non-admin users
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "hideAdmin",
                        @"document.querySelectorAll('.sidebar-nav')[1].parentElement.previousElementSibling.style.display='none';
                          document.querySelectorAll('.sidebar-nav')[1].style.display='none';", true);
                }

                dpdate.SelectedDate = DateTime.Now.AddDays(4);
                DataTable dt = omlunch(dpdate.Text, ddltype.SelectedValue);
                string initialMenuDesc = "";
                if (dt.Rows.Count > 0)
                {
                    taname.Text = dt.Rows[0]["LUNCHNAME"].ToString();
                    initialMenuDesc = dt.Rows[0]["LUNCHNAME"].ToString().Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");
                }
                else
                    taname.Text = "";

                // Pre-populate custom modal inputs with initial data
                string initMenuDate = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "initMenuData",
                    string.Format(@"
                        document.addEventListener('DOMContentLoaded', function() {{
                            var md = document.getElementById('menuDate');
                            var mt = document.getElementById('menuType');
                            var mdesc = document.getElementById('menuDesc');
                            if(md) md.value = '{0}';
                            if(mt) mt.value = '{1}';
                            if(mdesc) mdesc.value = '{2}';
                        }});", initMenuDate, ddltype.SelectedValue, initialMenuDesc), true);
            }
        }

        #region WebMethods for AJAX-based UI

        /// <summary>
        /// WebMethod 專用：取得當前使用者的 UserNo。
        /// 邏輯與 Page_Load 相同 — localhost 回傳測試帳號，正式環境從 Session 讀取。
        /// （因 WebMethod 必須是 static，無法存取 instance fields userno 等，故抽成此方法共用）
        /// </summary>
        private static string GetCurrentUserNo()
        {
            string host = HttpContext.Current.Request.Url.Host;
            if (host == "localhost")
                return "EB237003";
            return ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
        }

        private static DateTime GetMondayForAdmin(DateTime dt)
        {
            int dayOfWeek = index.DayOfWeek(dt);
            return DateTime.Parse(dt.AddDays(1 - dayOfWeek).ToShortDateString() + " 09:00:00");
        }

        private static Dictionary<string, Dictionary<string, object>> GetWeekMealsDataAdmin(DateTime monday)
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            string[] mealTypeList = { "自助餐", "簡餐", "麵食", "輕食餐", "素食" };

            for (int i = 0; i < 5; i++)
            {
                DateTime currentDate = monday.AddDays(i);
                string dateKey = currentDate.ToString("yyyy-MM-dd");
                var dayMeals = new Dictionary<string, object>();

                foreach (string mealType in mealTypeList)
                {
                    DataTable dt = omlunch(currentDate.ToShortDateString(), mealType);
                    if (dt.Rows.Count > 0)
                    {
                        string lunchName = dt.Rows[0]["LUNCHNAME"].ToString();
                        string lunchId = dt.Rows[0]["LUNCHID"].ToString();
                        if (!string.IsNullOrEmpty(lunchName) && lunchName != "\r\n")
                        {
                            dayMeals[mealType] = new { id = lunchId, name = replacebr(lunchName), available = true };
                        }
                    }
                }

                if (!dayMeals.ContainsKey("素食"))
                {
                    dayMeals["素食"] = new { id = "0", name = "素食盒餐", available = !checksushi(currentDate.ToShortDateString()) };
                }

                result[dateKey] = dayMeals;
            }
            return result;
        }

        private static Dictionary<string, object> GetUserOrdersDataAdmin(DateTime monday, string userNo)
        {
            var result = new Dictionary<string, object>();
            for (int i = 0; i < 5; i++)
            {
                DateTime currentDate = monday.AddDays(i);
                string dateKey = currentDate.ToString("yyyy-MM-dd");
                string[] types = { "自助餐", "簡餐", "麵食", "輕食餐" };
                bool found = false;

                foreach (string mealType in types)
                {
                    DataTable dt = omlunch(currentDate.ToShortDateString(), mealType);
                    if (dt.Rows.Count > 0)
                    {
                        string lunchId = dt.Rows[0]["LUNCHID"].ToString();
                        string checkedResult = Checked(currentDate.ToShortDateString(), userNo, lunchId);
                        if (!string.IsNullOrEmpty(checkedResult))
                        {
                            result[dateKey] = new { lunchId = lunchId, type = mealType, orderNum = checkedResult };
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    string vegCheck = Checked(currentDate.ToShortDateString(), userNo, "0");
                    if (vegCheck == "0")
                        result[dateKey] = new { lunchId = "0", type = "不訂餐", orderNum = "0" };
                    else if (!string.IsNullOrEmpty(vegCheck))
                        result[dateKey] = new { lunchId = "0", type = "素食", orderNum = vegCheck };
                }
            }
            return result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetNoticeSettings()
        {
            try
            {
                string userNo = GetCurrentUserNo();

                string sqlMail = string.Format(
                    "SELECT isEnable FROM OrderMealNotice WHERE UserNo='{0}' AND Type='1'", userNo);
                string sqlDing = string.Format(
                    "SELECT isEnable FROM OrderMealNotice WHERE UserNo='{0}' AND Type='2'", userNo);

                string mailRow = ltDll.ltClass.SelectFromMesFirstRow(sqlMail);
                string dingRow = ltDll.ltClass.SelectFromMesFirstRow(sqlDing);

                // Default to enabled (true) if no record exists
                bool mailEnabled = (mailRow != "Null" && mailRow != "") ? mailRow == "1" : true;
                bool dingTalkEnabled = (dingRow != "Null" && dingRow != "") ? dingRow == "1" : true;

                return new { success = true, mailEnabled = mailEnabled, dingTalkEnabled = dingTalkEnabled };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message, mailEnabled = true, dingTalkEnabled = true };
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object SaveNoticeSettings(int mailEnabled, int dingTalkEnabled)
        {
            try
            {
                string userNo = GetCurrentUserNo();

                UpsertNotice(userNo, "1", mailEnabled);
                UpsertNotice(userNo, "2", dingTalkEnabled);

                return new { success = true };
            }
            catch (Exception ex)
            {
                return new { success = false, message = "儲存失敗: " + ex.Message };
            }
        }

        private static void UpsertNotice(string userNo, string type, int isEnable)
        {
            string checkSql = string.Format(
                "SELECT COUNT(*) FROM OrderMealNotice WHERE UserNo='{0}' AND Type='{1}'", userNo, type);
            string count = ltDll.ltClass.SelectFromMesFirstRow(checkSql);

            if (count == "0" || count == "Null" || count == "")
            {
                string insertSql = string.Format(
                    "INSERT INTO OrderMealNotice (UserNo, Type, isEnable) VALUES ('{0}', '{1}', {2})",
                    userNo, type, isEnable);
                ltDll.ltClass.ExecuteToMes(insertSql);
            }
            else
            {
                string updateSql = string.Format(
                    "UPDATE OrderMealNotice SET isEnable={0} WHERE UserNo='{1}' AND Type='{2}'",
                    isEnable, userNo, type);
                ltDll.ltClass.ExecuteToMes(updateSql);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object LoadWeekData(string startDate, string endDate)
        {
            try
            {
                string userNo = GetCurrentUserNo();

                DateTime start = DateTime.Parse(startDate);
                DateTime monday = DateTime.Parse(start.ToShortDateString() + " 09:00:00");

                var mealsData = GetWeekMealsDataAdmin(monday);
                var ordersData = GetUserOrdersDataAdmin(monday, userNo);

                return new
                {
                    success = true,
                    mealsData = JsonConvert.SerializeObject(mealsData),
                    ordersData = JsonConvert.SerializeObject(ordersData)
                };
            }
            catch (Exception ex)
            {
                return new { success = false, message = "載入資料失敗: " + ex.Message };
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object SaveWeekOrders(string ordersJson)
        {
            try
            {
                string userNo;
                string userName;
                string shiftName;

                string _DomainName = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).Host;
                if (_DomainName != "localhost")
                {
                    userNo = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
                    userName = ltDll.ltClass.SessionRead("OrderMeal", "USERNAME");
                    shiftName = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNAME");
                }
                else
                {
                    userNo = "EB237003";
                    userName = "";
                    shiftName = "";
                }
               
                DateTime todayNow = DateTime.Now;

                if (string.IsNullOrEmpty(userNo))
                    return new { success = false, message = "請重新登入" };

                var orders = JsonConvert.DeserializeObject<Dictionary<string, index.OrderInfo>>(ordersJson);
                DateTime dtime = index.GetLatestDate(todayNow);

                foreach (var kvp in orders)
                {
                    string dateStr = kvp.Key;
                    var order = kvp.Value;
                    DateTime orderDate = DateTime.Parse(dateStr + " 09:00:00");

                    DateTime todayCutoff = todayNow.Date.AddHours(9);

                    if (orderDate.Date < todayNow.Date)
                        continue;

                    if (orderDate.Date == todayNow.Date && todayNow >= todayCutoff)
                        continue;

                    if (orderDate.DayOfWeek == System.DayOfWeek.Saturday || orderDate.DayOfWeek == System.DayOfWeek.Sunday)
                        continue;

                    if (orderDate.Date > dtime.Date)
                        continue;

                    string sql = string.Format("select * from omorder where ORDERDATE=to_date('{0}', 'yyyy-mm-dd') and USERNO='{1}'",
                        orderDate.ToShortDateString(), userNo);
                    DataTable existingOrder = ltDll.ltClass.SelectFromMes(sql);

                    string lunchId = order.lunchId ?? "0";
                    string orderNum = order.orderNum ?? "0";

                    if (existingOrder.Rows.Count > 0)
                    {
                        string orderId = existingOrder.Rows[0]["ORDERID"].ToString();
                        string existingLunchId = existingOrder.Rows[0]["LUNCHID"].ToString();
                        string existingOrderNum = existingOrder.Rows[0]["ORDERNUM"].ToString();

                        if (lunchId != existingLunchId || orderNum != existingOrderNum)
                        {
                            string updateSql = string.Format(
                                "update omorder set LUNCHID={0},ordernum={1},updatedate=sysdate where orderid={2}",
                                lunchId, orderNum, orderId);
                            ltDll.ltClass.ExecuteToMes(updateSql);
                        }
                    }
                    else if (int.Parse(orderNum) >= 1)
                    {
                        string maxId = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
                        string insertSql = string.Format(
                            "insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP,updatedate,username,shiftname,euser,edate) values ({0},to_date('{1}', 'yyyy-mm-dd'),{2},{3},'{4}','{5}',sysdate,'{6}','{7}','{8}',sysdate)",
                            maxId, orderDate.ToShortDateString(), orderNum, lunchId, userNo, ltDll.ltClass.GetIPAddress(), userName, shiftName, userNo);
                        ltDll.ltClass.ExecuteToMes(insertSql);
                    }
                }

                return new { success = true, message = "訂餐成功！" };
            }
            catch (Exception ex)
            {
                return new { success = false, message = "儲存失敗: " + ex.Message };
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetMenuData(string lunchDate, string lunchType)
        {
            try
            {
                DataTable dt = omlunch(lunchDate, lunchType);
                if (dt.Rows.Count > 0)
                {
                    return new { success = true, lunchName = dt.Rows[0]["LUNCHNAME"].ToString() };
                }
                return new { success = true, lunchName = "" };
            }
            catch (Exception ex)
            {
                return new { success = false, lunchName = "", message = ex.Message };
            }
        }

        #endregion

        protected void DatePicker1_TextChanged(object sender, EventArgs e)
        {
            // BindTable no longer needed - new UI uses AJAX/PageMethods
            // DateTime sd = DateTime.Parse(DatePicker1.Text);
            // BindTable(sd);
        }

#if false
        private void BindTable(DateTime dt)
        {
            //CheckBoxList cb;
            DateTime monday = DateTime.Parse(dt.AddDays(1 - DayOfWeek(dt)).ToShortDateString() + " 09:00:00");
            DateTime td = DateTime.Parse(today.ToShortDateString() + " 09:00:00");
            //if (userno == "EA912003")
            //{
            //    monday = DateTime.Parse(dt.AddDays(1 - DayOfWeek(dt)).ToShortDateString() + " 09:30:00");
            //}

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

            DataTable dt1 = omlunch(monday.ToShortDateString(), "自助餐");
            string n1 = ""; string i1 = "";
            if (dt1.Rows.Count > 0)
            {
                n1 = dt1.Rows[0]["LUNCHNAME"].ToString();
                i1 = dt1.Rows[0]["LUNCHID"].ToString();
                CheckBox1.Label = i1;
                CheckBox1.ShowLabel = false;
                string num = Checked(monday.ToShortDateString(), ddlLA4.SelectedValue, i1);
                CheckBox1.Checked = (num == "" ? false : true);
                NumberBox1.Text = num;
            }
            else
            {
                CheckBox1.Checked = false;
                NumberBox1.Text = "";
            }
            DataTable dt2 = omlunch(monday.AddDays(1).ToShortDateString(), "自助餐");
            string n2 = ""; string i2 = "";
            if (dt2.Rows.Count > 0)
            {
                n2 = dt2.Rows[0]["LUNCHNAME"].ToString();
                i2 = dt2.Rows[0]["LUNCHID"].ToString();
                CheckBox2.Label = i2;
                CheckBox2.ShowLabel = false;
                string num = Checked(monday.AddDays(1).ToShortDateString(), ddlLA4.SelectedValue, i2);
                CheckBox2.Checked = (num == "" ? false : true);
                NumberBox2.Text = num;
            }
            else
            {
                CheckBox2.Checked = false;
                NumberBox2.Text = "";
            }
            DataTable dt3 = omlunch(monday.AddDays(2).ToShortDateString(), "自助餐");
            string n3 = ""; string i3 = "";
            if (dt3.Rows.Count > 0)
            {
                n3 = dt3.Rows[0]["LUNCHNAME"].ToString();
                i3 = dt3.Rows[0]["LUNCHID"].ToString();
                CheckBox3.Label = i3;
                CheckBox3.ShowLabel = false;
                string num = Checked(monday.AddDays(2).ToShortDateString(), ddlLA4.SelectedValue, i3);
                CheckBox3.Checked = (num == "" ? false : true);
                NumberBox3.Text = num;

            }
            else
            {
                CheckBox3.Checked = false;
                NumberBox3.Text = "";
            }
            DataTable dt4 = omlunch(monday.AddDays(3).ToShortDateString(), "自助餐");
            string n4 = ""; string i4 = "";
            if (dt4.Rows.Count > 0)
            {
                n4 = dt4.Rows[0]["LUNCHNAME"].ToString();
                i4 = dt4.Rows[0]["LUNCHID"].ToString();
                CheckBox4.Label = i4;
                CheckBox4.ShowLabel = false;
                string num = Checked(monday.AddDays(3).ToShortDateString(), ddlLA4.SelectedValue, i4);
                CheckBox4.Checked = (num == "" ? false : true);
                NumberBox4.Text = num;
            }
            else
            {
                CheckBox4.Checked = false;
                NumberBox4.Text = "";
            }
            DataTable dt5 = omlunch(monday.AddDays(4).ToShortDateString(), "自助餐");
            string n5 = ""; string i5 = "";
            if (dt5.Rows.Count > 0)
            {
                n5 = dt5.Rows[0]["LUNCHNAME"].ToString();
                i5 = dt5.Rows[0]["LUNCHID"].ToString();
                CheckBox5.Label = i5;
                CheckBox5.ShowLabel = false;
                string num = Checked(monday.AddDays(4).ToShortDateString(), ddlLA4.SelectedValue, i5);
                CheckBox5.Checked = (num == "" ? false : true);
                NumberBox5.Text = num;
            }
            else
            {
                CheckBox5.Checked = false;
                NumberBox5.Text = "";
            }

            DataTable dt8 = omlunch(monday.ToShortDateString(), "簡餐");
            string n8 = ""; string i8 = "";
            if (dt8.Rows.Count > 0)
            {
                n8 = dt8.Rows[0]["LUNCHNAME"].ToString();
                i8 = dt8.Rows[0]["LUNCHID"].ToString();
                CheckBox8.Label = i8;
                CheckBox8.ShowLabel = false;
                string num = Checked(monday.ToShortDateString(), ddlLA4.SelectedValue, i8);
                CheckBox8.Checked = (num == "" ? false : true);
                NumberBox8.Text = num;
            }
            else
            {
                CheckBox8.Checked = false;
                NumberBox8.Text = "";
            }
            DataTable dt9 = omlunch(monday.AddDays(1).ToShortDateString(), "簡餐");
            string n9 = ""; string i9 = "";
            if (dt9.Rows.Count > 0)
            {
                n9 = dt9.Rows[0]["LUNCHNAME"].ToString();
                i9 = dt9.Rows[0]["LUNCHID"].ToString();
                CheckBox9.Label = i9;
                CheckBox9.ShowLabel = false;
                string num = Checked(monday.AddDays(1).ToShortDateString(), ddlLA4.SelectedValue, i9);
                CheckBox9.Checked = (num == "" ? false : true);
                NumberBox9.Text = num;
            }
            else
            {
                CheckBox9.Checked = false;
                NumberBox9.Text = "";
            }
            DataTable dt10 = omlunch(monday.AddDays(2).ToShortDateString(), "簡餐");
            string n10 = ""; string i10 = "";
            if (dt10.Rows.Count > 0)
            {
                n10 = dt10.Rows[0]["LUNCHNAME"].ToString();
                i10 = dt10.Rows[0]["LUNCHID"].ToString();
                CheckBox10.Label = i10;
                CheckBox10.ShowLabel = false;
                string num = Checked(monday.AddDays(2).ToShortDateString(), ddlLA4.SelectedValue, i10);
                CheckBox10.Checked = (num == "" ? false : true);
                NumberBox10.Text = num;
            }
            else
            {
                CheckBox10.Checked = false;
                NumberBox10.Text = "";
            }
            DataTable dt11 = omlunch(monday.AddDays(3).ToShortDateString(), "簡餐");
            string n11 = ""; string i11 = "";
            if (dt11.Rows.Count > 0)
            {
                n11 = dt11.Rows[0]["LUNCHNAME"].ToString();
                i11 = dt11.Rows[0]["LUNCHID"].ToString();
                CheckBox11.Label = i11;
                CheckBox11.ShowLabel = false;
                string num = Checked(monday.AddDays(3).ToShortDateString(), ddlLA4.SelectedValue, i11);
                CheckBox11.Checked = (num == "" ? false : true);
                NumberBox11.Text = num;
            }
            else
            {
                CheckBox11.Checked = false;
                NumberBox11.Text = "";
            }
            DataTable dt12 = omlunch(monday.AddDays(4).ToShortDateString(), "簡餐");
            string n12 = ""; string i12 = "";
            if (dt12.Rows.Count > 0)
            {
                n12 = dt12.Rows[0]["LUNCHNAME"].ToString();
                i12 = dt12.Rows[0]["LUNCHID"].ToString();
                CheckBox12.Label = i12;
                CheckBox12.ShowLabel = false;
                string num = Checked(monday.AddDays(4).ToShortDateString(), ddlLA4.SelectedValue, i12);
                CheckBox12.Checked = (num == "" ? false : true);
                NumberBox12.Text = num;
            }
            else
            {
                CheckBox12.Checked = false;
                NumberBox12.Text = "";
            }
            DataTable dt15 = omlunch(monday.ToShortDateString(), "麵食");
            string n15 = ""; string i15 = "";
            if (dt15.Rows.Count > 0)
            {
                n15 = dt15.Rows[0]["LUNCHNAME"].ToString();
                i15 = dt15.Rows[0]["LUNCHID"].ToString();
                CheckBox15.Label = i15;
                CheckBox15.ShowLabel = false;
                string num = Checked(monday.ToShortDateString(), ddlLA4.SelectedValue, i15);
                CheckBox15.Checked = (num == "" ? false : true);
                NumberBox15.Text = num;
            }
            else
            {
                CheckBox15.Checked = false;
                NumberBox15.Text = "";
            }
            DataTable dt16 = omlunch(monday.AddDays(1).ToShortDateString(), "麵食");
            string n16 = ""; string i16 = "";
            if (dt16.Rows.Count > 0)
            {
                n16 = dt16.Rows[0]["LUNCHNAME"].ToString();
                i16 = dt16.Rows[0]["LUNCHID"].ToString();
                CheckBox16.Label = i16;
                CheckBox16.ShowLabel = false;
                string num = Checked(monday.AddDays(1).ToShortDateString(), ddlLA4.SelectedValue, i16);
                CheckBox16.Checked = (num == "" ? false : true);
                NumberBox16.Text = num;
            }
            else
            {
                CheckBox16.Checked = false;
                NumberBox16.Text = "";
            }
            DataTable dt17 = omlunch(monday.AddDays(2).ToShortDateString(), "麵食");
            string n17 = ""; string i17 = "";
            if (dt17.Rows.Count > 0)
            {
                n17 = dt17.Rows[0]["LUNCHNAME"].ToString();
                i17 = dt17.Rows[0]["LUNCHID"].ToString();
                CheckBox17.Label = i17;
                CheckBox17.ShowLabel = false;
                string num = Checked(monday.AddDays(2).ToShortDateString(), ddlLA4.SelectedValue, i17);
                CheckBox17.Checked = (num == "" ? false : true);
                NumberBox17.Text = num;
            }
            else
            {
                CheckBox17.Checked = false;
                NumberBox17.Text = "";
            }
            DataTable dt18 = omlunch(monday.AddDays(3).ToShortDateString(), "麵食");
            string n18 = ""; string i18 = "";
            if (dt18.Rows.Count > 0)
            {
                n18 = dt18.Rows[0]["LUNCHNAME"].ToString();
                i18 = dt18.Rows[0]["LUNCHID"].ToString();
                CheckBox18.Label = i18;
                CheckBox18.ShowLabel = false;
                string num = Checked(monday.AddDays(3).ToShortDateString(), ddlLA4.SelectedValue, i18);
                CheckBox18.Checked = (num == "" ? false : true);
                NumberBox18.Text = num;
            }
            else
            {
                CheckBox18.Checked = false;
                NumberBox18.Text = "";
            }
            DataTable dt19 = omlunch(monday.AddDays(4).ToShortDateString(), "麵食");
            string n19 = ""; string i19 = "";
            if (dt19.Rows.Count > 0)
            {
                n19 = dt19.Rows[0]["LUNCHNAME"].ToString();
                i19 = dt19.Rows[0]["LUNCHID"].ToString();
                CheckBox19.Label = i19;
                CheckBox19.ShowLabel = false;
                string num = Checked(monday.AddDays(4).ToShortDateString(), ddlLA4.SelectedValue, i19);
                CheckBox19.Checked = (num == "" ? false : true);
                NumberBox19.Text = num;
            }
            else
            {
                CheckBox19.Checked = false;
                NumberBox19.Text = "";
            }
            DataTable dt13 = omlunch(monday.AddDays(1).ToShortDateString(), "輕食餐");
            string n13 = ""; string i13 = "";
            if (dt13.Rows.Count > 0)
            {
                n13 = dt13.Rows[0]["LUNCHNAME"].ToString();
                i13 = dt13.Rows[0]["LUNCHID"].ToString();
                CheckBox13.Label = i13;
                CheckBox13.ShowLabel = false;
                string num = Checked(monday.AddDays(1).ToShortDateString(), ddlLA4.SelectedValue, i13);
                CheckBox13.Checked = (num == "" ? false : true);
                NumberBox13.Text = num;
            }
            else
            {
                CheckBox13.Checked = false;
                NumberBox13.Text = "";
            }
            DataTable dt6 = omlunch(monday.AddDays(2).ToShortDateString(), "輕食餐");
            string n6 = ""; string i6 = "";
            if (dt6.Rows.Count > 0)
            {
                n6 = dt6.Rows[0]["LUNCHNAME"].ToString();
                i6 = dt6.Rows[0]["LUNCHID"].ToString();
                CheckBox6.Label = i6;
                CheckBox6.ShowLabel = false;
                string num = Checked(monday.AddDays(2).ToShortDateString(), ddlLA4.SelectedValue, i6);
                CheckBox6.Checked = (num == "" ? false : true);
                NumberBox6.Text = num;
            }
            else
            {
                CheckBox6.Checked = false;
                NumberBox6.Text = "";
            }
            DataTable dt14 = omlunch(monday.AddDays(3).ToShortDateString(), "輕食餐");
            string n14 = ""; string i14 = "";
            if (dt14.Rows.Count > 0)
            {
                n14 = dt14.Rows[0]["LUNCHNAME"].ToString();
                i14 = dt14.Rows[0]["LUNCHID"].ToString();
                CheckBox14.Label = i14;
                CheckBox14.ShowLabel = false;
                string num = Checked(monday.AddDays(3).ToShortDateString(), ddlLA4.SelectedValue, i14);
                CheckBox14.Checked = (num == "" ? false : true);
                NumberBox14.Text = num;
            }
            else
            {
                CheckBox14.Checked = false;
                NumberBox14.Text = "";
            }
            DataTable dt7 = omlunch(monday.AddDays(4).ToShortDateString(), "輕食餐");
            string n7 = ""; string i7 = "";
            if (dt7.Rows.Count > 0)
            {
                n7 = dt7.Rows[0]["LUNCHNAME"].ToString();
                i7 = dt7.Rows[0]["LUNCHID"].ToString();
                CheckBox7.Label = i7;
                CheckBox7.ShowLabel = false;
                string num = Checked(monday.AddDays(4).ToShortDateString(), ddlLA4.SelectedValue, i7);
                CheckBox7.Checked = (num == "" ? false : true);
                NumberBox7.Text = num;
            }
            else
            {
                CheckBox7.Checked = false;
                NumberBox7.Text = "";
            }

            if (n1 == "" || n1 == "\r\n")
            {
                CheckBox1.Hidden = true;
                NumberBox1.Hidden = true;
            }
            else
            {
                if (monday < today)
                {
                    CheckBox1.Hidden = true;
                    NumberBox1.Readonly = true;
                    NumberBox1.Hidden = false;
                }
                else
                {
                    CheckBox1.Hidden = false;
                    NumberBox1.Hidden = false;
                    NumberBox1.Readonly = false;
                }
            }
            if (n2 == "" || n2 == "\r\n")
            {
                CheckBox2.Hidden = true;
                NumberBox2.Hidden = true;
            }
            else
            {
                if (monday.AddDays(1) < today)
                {
                    CheckBox2.Hidden = true;
                    NumberBox2.Readonly = true;
                    NumberBox2.Hidden = false;
                }
                else
                {
                        CheckBox2.Hidden = false;
                        NumberBox2.Hidden = false;
                        NumberBox2.Readonly = false;
                   
                }
            }
            if (n3 == "" || n3 == "\r\n")
            {
                CheckBox3.Hidden = true;
                NumberBox3.Hidden = true;
            }
            else
            {
                if (monday.AddDays(2) < today)
                {
                    CheckBox3.Hidden = true;
                    NumberBox3.Readonly = true;
                    NumberBox3.Hidden = false;
                }
                else
                {
                    CheckBox3.Hidden = false;
                    NumberBox3.Hidden = false;
                    NumberBox3.Readonly = false;
                }
            }
            if (n4 == "" || n4 == "\r\n")
            {
                CheckBox4.Hidden = true;
                NumberBox4.Hidden = true;
            }
            else
            {
                if (monday.AddDays(3) < today)
                {
                    CheckBox4.Hidden = true;
                    NumberBox4.Readonly = true;
                    NumberBox4.Hidden = false;
                }
                else
                {
                    CheckBox4.Hidden = false;
                    NumberBox4.Hidden = false;
                    NumberBox4.Readonly = false;
                }
            }

            if (n5 == "" || n5 == "\r\n")
            {
                CheckBox5.Hidden = true;
                NumberBox5.Hidden = true;
            }
            else
            {
                if (monday.AddDays(4) < today)
                {
                    CheckBox5.Hidden = true;
                    NumberBox5.Readonly = true;
                    NumberBox5.Hidden = false;
                }
                else
                {
                    CheckBox5.Hidden = false;
                    NumberBox5.Hidden = false;
                    NumberBox5.Readonly = false;
                }
            }
            if (n8 == "" || n8 == "\r\n")
            {
                CheckBox8.Hidden = true;
                NumberBox8.Hidden = true;
            }
            else
            {
                if (monday < today)
                {
                    CheckBox8.Hidden = true;
                    NumberBox8.Readonly = true;
                    NumberBox8.Hidden = false;
                }
                else
                {
                    CheckBox8.Hidden = false;
                    NumberBox8.Hidden = false;
                    NumberBox8.Readonly = false;
                }
            }
            if (n9 == "" || n9 == "\r\n")
            {
                CheckBox9.Hidden = true;
                NumberBox9.Hidden = true;
            }
            else
            {
                if (monday.AddDays(1) < today)
                {
                    CheckBox9.Hidden = true;
                    NumberBox9.Readonly = true;
                    NumberBox9.Hidden = false;
                }
                else
                {
                    CheckBox9.Hidden = false;
                    NumberBox9.Hidden = false;
                    NumberBox9.Readonly = false;
                }
            }
            if (n10 == "" || n10 == "\r\n")
            {
                CheckBox10.Hidden = true;
                NumberBox10.Hidden = true;
            }
            else
            {
                if (monday.AddDays(2) < today)
                {
                    CheckBox10.Hidden = true;
                    NumberBox10.Readonly = true;
                    NumberBox10.Hidden = false;
                }
                else
                {
                    CheckBox10.Hidden = false;
                    NumberBox10.Hidden = false;
                    NumberBox10.Readonly = false;
                }
            }
            if (n11 == "" || n11 == "\r\n")
            {
                CheckBox11.Hidden = true;
                NumberBox11.Hidden = true;
            }
            else
            {
                if (monday.AddDays(3) < today)
                {
                    CheckBox11.Hidden = true;
                    NumberBox11.Readonly = true;
                    NumberBox11.Hidden = false;
                }
                else
                {
                    CheckBox11.Hidden = false;
                    NumberBox11.Hidden = false;
                    NumberBox11.Readonly = false;
                }
            }
            if (n12 == "" || n12 == "\r\n")
            {
                CheckBox12.Hidden = true;
                NumberBox12.Hidden = true;
            }
            else
            {
                if (monday.AddDays(4) < today)
                {
                    CheckBox12.Hidden = true;
                    NumberBox12.Readonly = true;
                    NumberBox12.Hidden = false;
                }
                else
                {
                    CheckBox12.Hidden = false;
                    NumberBox12.Hidden = false;
                    NumberBox12.Readonly = false;
                }
            }

            if (n15 == "" || n15 == "\r\n")
            {
                CheckBox15.Hidden = true;
                NumberBox15.Hidden = true;
            }
            else
            {
                if (monday < today)
                {
                    CheckBox15.Hidden = true;
                    NumberBox15.Readonly = true;
                    NumberBox15.Hidden = false;
                }
                else
                {
                    CheckBox15.Hidden = false;
                    NumberBox15.Hidden = false;
                    NumberBox15.Readonly = false;
                }
            }
            if (n16 == "" || n16 == "\r\n")
            {
                CheckBox16.Hidden = true;
                NumberBox16.Hidden = true;
            }
            else
            {
                if (monday.AddDays(1) < today)
                {
                    CheckBox16.Hidden = true;
                    NumberBox16.Readonly = true;
                    NumberBox16.Hidden = false;
                }
                else
                {
                    CheckBox16.Hidden = false;
                    NumberBox16.Hidden = false;
                    NumberBox16.Readonly = false;
                }
            }
            if (n17 == "" || n17 == "\r\n")
            {
                CheckBox17.Hidden = true;
                NumberBox17.Hidden = true;
            }
            else
            {
                if (monday.AddDays(2) < today)
                {
                    CheckBox17.Hidden = true;
                    NumberBox17.Readonly = true;
                    NumberBox17.Hidden = false;
                }
                else
                {
                    CheckBox17.Hidden = false;
                    NumberBox17.Hidden = false;
                    NumberBox17.Readonly = false;
                }
            }
            if (n18 == "" || n18 == "\r\n")
            {
                CheckBox18.Hidden = true;
                NumberBox18.Hidden = true;
            }
            else
            {
                if (monday.AddDays(3) < today)
                {
                    CheckBox18.Hidden = true;
                    NumberBox18.Readonly = true;
                    NumberBox18.Hidden = false;
                }
                else
                {
                    CheckBox18.Hidden = false;
                    NumberBox18.Hidden = false;
                    NumberBox18.Readonly = false;
                }
            }
            if (n19 == "" || n19 == "\r\n")
            {
                CheckBox19.Hidden = true;
                NumberBox19.Hidden = true;
            }
            else
            {
                if (monday.AddDays(4) < today)
                {
                    CheckBox19.Hidden = true;
                    NumberBox19.Readonly = true;
                    NumberBox19.Hidden = false;
                }
                else
                {
                    CheckBox19.Hidden = false;
                    NumberBox19.Hidden = false;
                    NumberBox19.Readonly = false;
                }
            }
            if (n13 == "" || n13 == "\r\n")
            {
                CheckBox13.Hidden = true;
                NumberBox13.Hidden = true;
            }
            else
            {
                if (monday.AddDays(1) < today)
                {
                    CheckBox13.Hidden = true;
                    NumberBox13.Readonly = true;
                    NumberBox13.Hidden = false;
                }
                else
                {
                    CheckBox13.Hidden = false;
                    NumberBox13.Hidden = false;
                    NumberBox13.Readonly = false;
                }
            }
            if (n6 == "" || n6 == "\r\n")
            {
                CheckBox6.Hidden = true;
                NumberBox6.Hidden = true;
            }
            else
            {
                if (monday.AddDays(2) < today)
                {
                    CheckBox6.Hidden = true;
                    NumberBox6.Readonly = true;
                    NumberBox6.Hidden = false;
                }
                else
                {
                    CheckBox6.Hidden = false;
                    NumberBox6.Hidden = false;
                    NumberBox6.Readonly = false;
                }
            }
            if (n14 == "" || n14 == "\r\n")
            {
                CheckBox14.Hidden = true;
                NumberBox14.Hidden = true;
            }
            else
            {
                if (monday.AddDays(3) < today)
                {
                    CheckBox14.Hidden = true;
                    NumberBox14.Readonly = true;
                    NumberBox14.Hidden = false;
                }
                else
                {
                    CheckBox14.Hidden = false;
                    NumberBox14.Hidden = false;
                    NumberBox14.Readonly = false;
                }
            }
            if (n7 == "" || n7 == "\r\n")
            {
                CheckBox7.Hidden = true;
                NumberBox7.Hidden = true;
            }
            else
            {
                if (monday.AddDays(4) < today)
                {
                    CheckBox7.Hidden = true;
                    NumberBox7.Readonly = true;
                    NumberBox7.Hidden = false;
                }
                else
                {
                    CheckBox7.Hidden = false;
                    NumberBox7.Hidden = false;
                    NumberBox7.Readonly = false;
                }
            }
            string num1 = Checked(monday.ToShortDateString(), ddlLA4.SelectedValue, "0");
            if (monday < today || checksushi(monday.ToShortDateString()))
            {
                CheckBox22.Hidden = true;
                CheckBox22.Checked = ((num1 == "0" || num1 == "") ? false : true);
                NumberBox22.Text = (num1 == "0" ? "" : num1);
                NumberBox22.Readonly = true;
                NumberBox22.Hidden = false;
                CheckBox29.Hidden = true;
                CheckBox29.Checked = ((num1 == "0") ? true : false);

            }
            else
            {
                CheckBox22.Checked = ((num1 == "0" || num1 == "") ? false : true);
                NumberBox22.Text = (num1 == "0" ? "" : num1);
                CheckBox22.Hidden = false;
                NumberBox22.Hidden = false;
                NumberBox22.Readonly = false;
                CheckBox29.Hidden = false;
                CheckBox29.Checked = ((num1 == "0") ? true : false);

            }
            string num2 = Checked(monday.AddDays(1).ToShortDateString(), ddlLA4.SelectedValue, "0");
            if (monday.AddDays(1) < today || checksushi(monday.AddDays(1).ToShortDateString()))
            {
                CheckBox23.Hidden = true;
                CheckBox23.Checked = ((num2 == "0" || num2 == "") ? false : true);
                NumberBox23.Text = (num2 == "0" ? "" : num2);
                NumberBox23.Readonly = true;
                NumberBox23.Hidden = false;
                CheckBox30.Hidden = true;
                CheckBox30.Checked = ((num2 == "0") ? true : false);

            }
            else
            {
                CheckBox23.Checked = ((num2 == "0" || num2 == "") ? false : true);
                NumberBox23.Text = (num2 == "0" ? "" : num2);
                CheckBox23.Hidden = false;
                NumberBox23.Hidden = false;
                NumberBox23.Readonly = false;
                CheckBox30.Hidden = false;
                CheckBox30.Checked = ((num2 == "0") ? true : false);

            }
            string num3 = Checked(monday.AddDays(2).ToShortDateString(), ddlLA4.SelectedValue, "0");
            if (monday.AddDays(2) < today || checksushi(monday.AddDays(2).ToShortDateString()))
            {
                CheckBox24.Hidden = true;
                CheckBox24.Checked = ((num3 == "0" || num3 == "") ? false : true);
                NumberBox24.Text = (num3 == "0" ? "" : num3);
                NumberBox24.Readonly = true;
                NumberBox24.Hidden = false;
                CheckBox31.Hidden = true;
                CheckBox31.Checked = ((num3 == "0") ? true : false);

            }
            else
            {
                CheckBox24.Checked = ((num3 == "0" || num3 == "") ? false : true);
                NumberBox24.Text = (num3 == "0" ? "" : num3);
                CheckBox24.Hidden = false;
                NumberBox24.Hidden = false;
                NumberBox24.Readonly = false;
                CheckBox31.Hidden = false;
                CheckBox31.Checked = ((num3 == "0") ? true : false);

            }
            string num4 = Checked(monday.AddDays(3).ToShortDateString(), ddlLA4.SelectedValue, "0");
            if (monday.AddDays(3) < today || checksushi(monday.AddDays(3).ToShortDateString()))
            {
                CheckBox25.Hidden = true;
                CheckBox25.Checked = ((num4 == "0" || num4 == "") ? false : true);
                NumberBox25.Text = (num4 == "0" ? "" : num4);
                NumberBox25.Readonly = true;
                NumberBox25.Hidden = false;
                CheckBox32.Hidden = true;
                CheckBox32.Checked = ((num4 == "0") ? true : false);

            }
            else
            {
                CheckBox25.Checked = ((num4 == "0" || num4 == "") ? false : true);
                NumberBox25.Text = (num4 == "0" ? "" : num4);
                CheckBox25.Hidden = false;
                NumberBox25.Hidden = false;
                NumberBox25.Readonly = false;
                CheckBox32.Hidden = false;
                CheckBox32.Checked = ((num4 == "0") ? true : false);

            }
            string num5 = Checked(monday.AddDays(4).ToShortDateString(), ddlLA4.SelectedValue, "0");
            if (monday.AddDays(4) < today || checksushi(monday.AddDays(4).ToShortDateString()))
            {
                CheckBox26.Hidden = true;
                CheckBox26.Checked = ((num5 == "0" || num5 == "") ? false : true);
                NumberBox26.Text = (num5 == "0" ? "" : num5);
                NumberBox26.Readonly = true;
                NumberBox26.Hidden = false;
                CheckBox33.Hidden = true;
                CheckBox33.Checked = ((num5 == "0") ? true : false);

            }
            else
            {
                CheckBox26.Checked = ((num5 == "0" || num5 == "") ? false : true);
                NumberBox26.Text = (num5 == "0" ? "" : num5);
                CheckBox26.Hidden = false;
                NumberBox26.Hidden = false;
                NumberBox26.Readonly = false;
                CheckBox33.Hidden = false;
                CheckBox33.Checked = ((num5 == "0") ? true : false);

            }

            Label15.Text = replacebr(n6);
            Label16.Text = replacebr(n7);
            Label23.Text = replacebr(n13);
            Label24.Text = replacebr(n14);

            Label18.Text = replacebr(n1);
            Label19.Text = replacebr(n2);
            Label20.Text = replacebr(n3);
            Label21.Text = replacebr(n4);
            Label22.Text = replacebr(n5);
            Label26.Text = replacebr(n8);
            Label27.Text = replacebr(n9);
            Label28.Text = replacebr(n10);
            Label29.Text = replacebr(n11);
            Label30.Text = replacebr(n12);
            Label34.Text = replacebr(n15);
            Label35.Text = replacebr(n16);
            Label36.Text = replacebr(n17);
            Label37.Text = replacebr(n18);
            Label38.Text = replacebr(n19);

        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            string lunchdate1 = Label2.Label;
            string lunchdate2 = Label3.Label;
            string lunchdate3 = Label4.Label;
            string lunchdate4 = Label5.Label;
            string lunchdate5 = Label6.Label;

            if (CheckBox1.Checked)
            {
                updatelunch(lunchdate1, CheckBox1.Label, NumberBox1.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text , shiftname);
            }
            else
            {
                deletelunch(lunchdate1, CheckBox1.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox8.Checked)
            {
                updatelunch(lunchdate1, CheckBox8.Label, NumberBox8.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate1, CheckBox8.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox15.Checked)
            {
                updatelunch(lunchdate1, CheckBox15.Label, NumberBox15.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate1, CheckBox15.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox22.Checked)
            {
                updatelunch(lunchdate1, "0", NumberBox22.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate1, "0", ddlLA4.SelectedValue);
            }
            if (CheckBox29.Checked)
            {
                cancellunch(lunchdate1, "0", "0");
            }

            if (CheckBox2.Checked)
            {
                updatelunch(lunchdate2, CheckBox2.Label, NumberBox2.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate2, CheckBox2.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox9.Checked)
            {
                updatelunch(lunchdate2, CheckBox9.Label, NumberBox9.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate2, CheckBox9.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox16.Checked)
            {
                updatelunch(lunchdate2, CheckBox16.Label, NumberBox16.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate2, CheckBox16.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox13.Checked)
            {
                updatelunch(lunchdate2, CheckBox13.Label, NumberBox13.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate2, CheckBox13.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox23.Checked)
            {
                updatelunch(lunchdate2, "0", NumberBox23.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate2, "0", ddlLA4.SelectedValue);
            }
            if (CheckBox30.Checked)
            {
                cancellunch(lunchdate2, "0", "0");
            }

            if (CheckBox3.Checked)
            {
                updatelunch(lunchdate3, CheckBox3.Label, NumberBox3.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate3, CheckBox3.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox10.Checked)
            {
                updatelunch(lunchdate3, CheckBox10.Label, NumberBox10.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate3, CheckBox10.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox17.Checked)
            {
                updatelunch(lunchdate3, CheckBox17.Label, NumberBox17.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate3, CheckBox17.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox6.Checked)
            {
                updatelunch(lunchdate3, CheckBox6.Label, NumberBox6.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate3, CheckBox6.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox24.Checked)
            {
                updatelunch(lunchdate3, "0", NumberBox24.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate3, "0", ddlLA4.SelectedValue);
            }
            if (CheckBox31.Checked)
            {
                cancellunch(lunchdate3, "0", "0");
            }

            if (CheckBox4.Checked)
            {
                updatelunch(lunchdate4, CheckBox4.Label, NumberBox4.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate4, CheckBox4.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox11.Checked)
            {
                updatelunch(lunchdate4, CheckBox11.Label, NumberBox11.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate4, CheckBox11.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox18.Checked)
            {
                updatelunch(lunchdate4, CheckBox18.Label, NumberBox18.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate4, CheckBox18.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox14.Checked)
            {
                updatelunch(lunchdate4, CheckBox14.Label, NumberBox14.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate4, CheckBox14.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox25.Checked)
            {
                updatelunch(lunchdate4, "0", NumberBox25.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate4, "0", ddlLA4.SelectedValue);
            }
            if (CheckBox32.Checked)
            {
                cancellunch(lunchdate4, "0", "0");
            }

            if (CheckBox5.Checked)
            {
                updatelunch(lunchdate5, CheckBox5.Label, NumberBox5.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate5, CheckBox5.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox12.Checked)
            {
                updatelunch(lunchdate5, CheckBox12.Label, NumberBox12.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate5, CheckBox12.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox19.Checked)
            {
                updatelunch(lunchdate5, CheckBox19.Label, NumberBox19.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate5, CheckBox19.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox7.Checked)
            {
                updatelunch(lunchdate5, CheckBox7.Label, NumberBox7.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate5, CheckBox7.Label, ddlLA4.SelectedValue);
            }
            if (CheckBox26.Checked)
            {
                updatelunch(lunchdate5, "0", NumberBox26.Text, ddlLA4.SelectedValue, ddlLA4.SelectedItem.Text, shiftname);
            }
            else
            {
                deletelunch(lunchdate5, "0", ddlLA4.SelectedValue);
            }
            if (CheckBox33.Checked)
            {
                cancellunch(lunchdate5, "0", "0");
            }
            BindTable(DateTime.Parse(DatePicker1.Text));
            string msg = "";
            if (today > DateTime.Parse(today.ToShortDateString() + " 09:00:00"))
            {
                msg = "今天已經超過訂餐時間，今天之後";
            }
            ShowNotify(msg + "訂餐成功！", MessageBoxIcon.Success);

        }
#endif
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
                        string up1 = string.Format("update omorder set LUNCHID={0},ordernum={1},updatedate=sysdate, euser='{3}',edate=sysdate where orderid={2}", lunchid, ordernum1, orderid, userno);
                        ltDll.ltClass.ExecuteToMes(up1);
                    }
                }
            }
            else
            {
                string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
                string in1 = string.Format("insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP,updatedate,username,shiftname,euser,edate) values ({0},to_date('{1}', 'yyyy-mm-dd'),{2},{3},'{4}','{5}',sysdate,'{6}','{7}','{8}',sysdate)", maxid, lunchdate, ordernum1, lunchid, userno, ltDll.ltClass.GetIPAddress(), username, shiftname, userno);
                ltDll.ltClass.ExecuteToMes(in1);
            }
        }
        private void cancellunch(string lunchdate, string lunchid, string ordernum1)
        {
            string sql1 = string.Format("select * from omorder where ORDERDATE=to_date('{0}', 'yyyy-mm-dd') and USERNO='{1}' ", lunchdate, userno);
            DataTable dt1 = ltDll.ltClass.SelectFromMes(sql1);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string orderid = dt1.Rows[i]["ORDERID"].ToString();
                    string up1 = string.Format("delete from omorder where orderid={0}", orderid);
                    ltDll.ltClass.ExecuteToMes(up1);
                }
            }
            //else
            //{
            string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
            string in1 = string.Format("insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP,username,shiftname,euser,edate) values ({0},to_date('{1}', 'yyyy-mm-dd'),{2},{3},'{4}','{5}','{6}','{7}','{8}',sysdate)", maxid, lunchdate, ordernum1, lunchid, userno, ltDll.ltClass.GetIPAddress(), username, shiftname, userno);
            ltDll.ltClass.ExecuteToMes(in1);
            //}
        }
        private void deletelunch(string lunchdate, string lunchid, string userno)
        {
            string sql1 = string.Format("select * from omorder where ORDERDATE=to_date('{0}', 'yyyy-mm-dd') and USERNO='{1}' ", lunchdate, userno);
            DataTable dt1 = ltDll.ltClass.SelectFromMes(sql1);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i]["lunchid"].ToString() == lunchid)
                    {
                        string orderid = dt1.Rows[i]["ORDERID"].ToString();
                        string up1 = string.Format("delete from omorder where orderid={0}", orderid);
                        ltDll.ltClass.ExecuteToMes(up1);
                    }
                }
            }
        }
        public static DataTable omlunch(string lunchdate, string lunchtype)
        {
            string sql = "select * from omlunch where LUNCHDATE=to_date('" + lunchdate + "','yyyy-mm-dd') and LUNCHTYPE='" + lunchtype + "'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            return dt;
        }
        public static string Checked(string lunchdate, string userno, string lunchid)
        {
            string sql = "select * from omorder where ORDERDATE=to_date('" + lunchdate + "', 'yyyy-mm-dd') and USERNO='" + userno + "' and lunchid=" + lunchid;
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ORDERNUM"].ToString();
            }
            else
            {
                return "";
            }

        }
        private string omlunchid(string lunchdate, string lunchtype)
        {
            string sql = "select * from omlunch where LUNCHDATE=to_date('" + lunchdate + "','yyyy-mm-dd') and LUNCHTYPE='" + lunchtype + "'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["LUNCHID"].ToString();
            }
            else
            {
                return "";
            }

        }
        public static int DayOfWeek(DateTime dt)
        {
            string strDayOfWeek = dt.DayOfWeek.ToString().ToLower();
            int Week = 0;
            switch (strDayOfWeek)
            {
                case "monday":
                    Week = 1;
                    break;
                case "tuesday":
                    Week = 2;
                    break;
                case "wednesday":
                    Week = 3;
                    break;
                case "thursday":
                    Week = 4;
                    break;
                case "friday":
                    Week = 5;
                    break;
                case "saturday":
                    Week = 6;
                    break;
                case "sunday":
                    Week = 7;
                    break;
            }
            return Week;

        }

        /// <summary>
        /// 获取控件渲染后的HTML源代码
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private string GetRenderedHtmlSource(Control ctrl)
        {
            if (ctrl != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        ctrl.RenderControl(htw);

                        return sw.ToString();
                    }
                }
            }
            return String.Empty;
        }



        private bool addlunchuser()
        {
            string sql = "select * from ltusers,tblusruserbasis where LA1=1 and LUSER='" + userno + "' and luser=userno(+) and lprogram='OrderMeal'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //private bool morelunchuser()
        //{
        //    string sql = "select * from ltusers,tblusruserbasis where LA2=1 and LUSER='" + userno + "' and luser=userno(+) and lprogram='" + lcode + "'";
        //    DataTable dt = ltDll.ltClass.SelectFromMes(sql);
        //    if (dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        private bool reportuser()
        {
            string sql = "select * from ltusers,tblusruserbasis where LA3=1 and LUSER='" + userno + "' and luser=userno(+) and lprogram='OrderMeal'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string replacebr(string str)
        {
            return str.Replace("\r\n", "<br/>");
        }



        protected void btnSend_Click(object sender, EventArgs e)
        {
            BindMailTable(DateTime.Now);
            ShowNotify("发送成功！", MessageBoxIcon.Success);
        }
        protected void btnDateSend_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Parse(DatePicker2.Text);
            DateTime end = DateTime.Parse(DatePicker3.Text);
            if (start.AddDays(22) < end)
            {
                ShowNotify("起始日期和結束日期之間不能超過22天！", MessageBoxIcon.Success);
            }
            else
            {               
                StringBuilder table = new StringBuilder();
                table.Append("<span style='color:#2E75B6'>各位同仁好,</span><br/><br/>");
                table.Append("<span style='color:#2E75B6'>員工團膳歡迎同仁用餐後回饋餐後感, 以期快速調整餐點與品質，讓大家吃的美味又開心， 謝謝。</span><br/><br/>");
                table.Append("<span>1) 請於每日16:00前，進入訂餐系統選訂明天的午餐(或取消)以備足量供餐。為確保您訂餐完成，建議您登出再請重新登入確認。<br/> 訂餐系統 http://192.168.1.41/T100/jing/OrderMeal/login.aspx </span><br/><br/>");
                table.Append("<span>2) 臨時請假不用餐，請立即取消餐(當日時限08:40前)，或請釘釘Angela分機3267協助，以節省資源不造成餐食之耗資浪費,謝謝！</span><br/><br/>");
                table.Append("<span>3) 不用餐的同仁，請勾選 [ 不訂餐 ]。另外若您未勾不訂餐，但收到不訂餐通知，就代表訂餐失效，請重新訂餐。</span><br/><br/>");
                table.Append("<span><< 大家防疫確實做好，保護您我一切安好 >></span><br/><br/>");
                table.Append("<table border='1' cellpadding='1' cellspacing='0'> <tr> ");
                table.Append("<td>日期</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        table.Append("<td>" + dt.Month + "月" + dt.Day + "日" + "</td>");
                    }
                }
                table.Append("</tr> <tr>");
                table.Append("<td>星期</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        if (week == "Monday")
                        {
                            table.Append("<td>星期一</td>");
                        }
                        if (week == "Tuesday")
                        {
                            table.Append("<td>星期二</td>");
                        }
                        if (week == "Wednesday")
                        {
                            table.Append("<td>星期三</td>");
                        }
                        if (week == "Thursday")
                        {
                            table.Append("<td>星期四</td>");
                        }
                        if (week == "Friday")
                        {
                            table.Append("<td>星期五</td>");
                        }
                    }
                }
                table.Append("</tr> <tr>");
                table.Append("<td>自助餐</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        table.Append("<td>" + replacebr(getlunch(dt.ToShortDateString(), "自助餐")) + "</td>");
                    }
                }
                table.Append("</tr> <tr>");
                table.Append("<td>簡餐</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        table.Append("<td>" + replacebr(getlunch(dt.ToShortDateString(), "簡餐")) + "</td>");
                    }
                }
                table.Append("</tr> <tr>");
                table.Append("<td>麵食</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        table.Append("<td>" + replacebr(getlunch(dt.ToShortDateString(), "麵食")) + "</td>");
                    }
                }
                table.Append("</tr> <tr>");
                table.Append("<td>輕食餐</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        if (week == "Monday")
                        {
                            table.Append("<td></td>");
                        }
                        else
                        {
                            table.Append("<td>" + replacebr(getlunch(dt.ToShortDateString(), "輕食餐")) + "</td>");
                        }
                    }
                }
                table.Append("</tr> <tr>");
                table.Append("<td>素食</td>");
                for (int i = 0; i <= (end - start).Days; i++)
                {
                    DateTime dt = start.AddDays(i);
                    string week = dt.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        table.Append("<td>素食盒餐</td>");
                    }
                }
                table.Append("</tr> </table> <br/>");
                //table.Append("<span color='red'>請同仁於今日下班前進系統點選下周餐點.</span>");
                string sql = "select lower(substr(mail,instr(mail,',',3)+1)) mail from ltdll_view3 left join tblusruserbasis_view on substr(mail, instr(mail, '*') + 1, instr(mail, '(') - instr(mail, '*') - 1) = tblusruserbasis_view.username left join ltusers on ltusers.luser = tblusruserbasis_view.userno where tblusruserbasis_view.issuestate = 2 and ltusers.LPROGRAM = 'OrderMeal' and userno not like 'LB%'  and userno not in (select luser from ltusers where  lprogram='OrderMeal' and LB1=1)";
                System.Data.DataTable tb = ltDll.ltClass.SelectFromMes(sql);
                //if (tb.Rows.Count > 0)
                //{
                //    for (int i = 0; i < tb.Rows.Count; i++)
                //    {
                //        if (tb.Rows[i]["mail"].ToString() != "")
                //        {
                //            ltDll.ltClass.SendEmail("音律 午餐訂餐通知", tb.Rows[i]["mail"].ToString(), "", "", table.ToString());
                //        }
                //    }
                //}
                ltDll.ltClass.SendEmail("菜單", "EdwardLin@elytone.com", "", "", table.ToString());
                ShowNotify("发送成功！", MessageBoxIcon.Success);
            }
        }
        public static void BindMailTable(DateTime dt)
        {
            DateTime monday = dt.AddDays(8 - DayOfWeek(dt));
            StringBuilder table = new StringBuilder();
            table.Append("<span style='color:#2E75B6'>各位同仁好,</span><br/><br/>");
            table.Append("<span style='color:#2E75B6'>員工團膳歡迎同仁用餐後回饋餐後感, 以期快速調整餐點與品質，讓大家吃的美味又開心， 謝謝。</span><br/><br/>");
            table.Append("<span>1) 請於每日16:00前，進入訂餐系統選訂明天的午餐(或取消)以備足量供餐。為確保您訂餐完成，建議您登出再請重新登入確認。<br/> 訂餐系統 http://192.168.1.41/T100/jing/OrderMeal/login.aspx </span><br/><br/>");
            table.Append("<span>2) 臨時請假不用餐，請立即取消餐(當日時限08:40前)，或請釘釘Angela分機3267協助，以節省資源不造成餐食之耗資浪費,謝謝！</span><br/><br/>");
            table.Append("<span>3) 不用餐的同仁，請勾選 [ 不訂餐 ]。另外若您未勾不訂餐，但收到不訂餐通知，就代表訂餐失效，請重新訂餐。</span><br/><br/>");
            //table.Append("<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ｂ組單週：業務二處， 研發二處，品保部，會計部，資訊處，管理處。</span><br/><br/>");
            //table.Append("<span>4) 不用餐的同仁，亦請勾選 [ 不訂餐 ]。</span><br/><br/>");
            table.Append("<span><< 大家防疫確實做好，保護您我一切安好 >></span><br/><br/>");

            //table.Append("訂餐方式為每日訂餐系統訂隔天的午餐<br/><br/>");
            //table.Append("<span>1) 請每日16:00進入訂餐系統點選訂隔天的午餐餐盒，以利正確準備您的美味餐盒享用。並請再次確認訂餐傳送是否進入系統 ( 請重新登入確認哦 )。</span><br/><br/>");
            //table.Append("<span>2) 當日若已知無法用餐之請假等臨時情況，請立即上線取消餐盒(時限08:40前)，或釘釘Angela分機3267協助，以節省資源不造成餐食之耗資浪費,謝謝！</span><br/><br/>");
            //table.Append("<span>3) 餐盒依點餐名單提供午餐食用，再次提醒同仁用餐請務必要先記得點餐。</span><br/><br/>");
            //table.Append("<span>4) 不用餐的同仁，亦請勾選 [ 不訂餐 ]。</span><br/><br/>");
            //table.Append("<span><< 大家防疫確實做好，保護您我一切安好 >></span><br/><br/>");

            //table.Append("訂餐方式為每日訂隔天的餐點，並請再次確認訂餐傳送是否成功進入系統 ( 請登出再登入檢查哦 )。<br/><br/>");
            //table.Append("1) 隔日之訂餐，最晚當天早上<span color='red'>8:40分</span>前，要上系統選訂 。以避免點餐未食用之情況。<br/>");
            //table.Append("<span color='red'>例：週二午餐，於週一都可以訂餐，最晚週二早上８：４０分前訂餐………以此類推。</span><br/><br/>");
            //table.Append("<span>2) 當日忘記訂餐者，請勿佔用他人的名額，以避免主餐不夠 ~ ~ 但開放於12:35分後可選用自助餐。</span><br/><br/>");
            //table.Append("<span>3) 自助餐的主菜有標示限取用數量１個或２個（例如 : 雞腿或滷蛋..等)，請不要多取或跨區拿取；避免自助餐晚到的同仁無主菜之情況。</span><br/><br/>");
            //table.Append("<span>4) 不用餐的同仁，請勾選 [ 不訂餐 ]。</span><br/><br/>");
            //table.Append("<span color='red'>4) 當日若已知 : 無法用餐、需外出、出差或請假者，請務必於8:45分前取消訂餐，節省資源不造成餐食之浪費,謝謝配合！！</span><br/><br/>");
            table.Append("<table border='1' cellpadding='1' cellspacing='0'> <tr> ");
            table.Append("<td>日期</td>");
            table.Append("<td>" + monday.Month + "月" + monday.Day + "日" + "</td>");
            table.Append("<td>" + monday.AddDays(1).Month + "月" + monday.AddDays(1).Day + "日" + "</td>");
            table.Append("<td>" + monday.AddDays(2).Month + "月" + monday.AddDays(2).Day + "日" + "</td>");
            table.Append("<td>" + monday.AddDays(3).Month + "月" + monday.AddDays(3).Day + "日" + "</td>");
            table.Append("<td>" + monday.AddDays(4).Month + "月" + monday.AddDays(4).Day + "日" + "</td>");
            table.Append("</tr> <tr>");
            table.Append("<td>星期</td>");
            table.Append("<td>星期一</td>");
            table.Append("<td>星期二</td>");
            table.Append("<td>星期三</td>");
            table.Append("<td>星期四</td>");
            table.Append("<td>星期五</td>");
            table.Append("</tr> <tr>");
            table.Append("<td>自助餐</td>");
            table.Append("<td>" + replacebr(getlunch(monday.ToShortDateString(), "自助餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(1).ToShortDateString(), "自助餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(2).ToShortDateString(), "自助餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(3).ToShortDateString(), "自助餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(4).ToShortDateString(), "自助餐")) + "</td>");
            table.Append("</tr> <tr>");
            table.Append("<td>簡餐</td>");
            table.Append("<td>" + replacebr(getlunch(monday.ToShortDateString(), "簡餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(1).ToShortDateString(), "簡餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(2).ToShortDateString(), "簡餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(3).ToShortDateString(), "簡餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(4).ToShortDateString(), "簡餐")) + "</td>");
            table.Append("</tr> <tr>");
            table.Append("<td>麵食</td>");
            table.Append("<td>" + replacebr(getlunch(monday.ToShortDateString(), "麵食")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(1).ToShortDateString(), "麵食")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(2).ToShortDateString(), "麵食")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(3).ToShortDateString(), "麵食")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(4).ToShortDateString(), "麵食")) + "</td>");
            table.Append("</tr> <tr>");
            table.Append("<td>輕食餐</td>");
            table.Append("<td></td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(1).ToShortDateString(), "輕食餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(2).ToShortDateString(), "輕食餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(3).ToShortDateString(), "輕食餐")) + "</td>");
            table.Append("<td>" + replacebr(getlunch(monday.AddDays(4).ToShortDateString(), "輕食餐")) + "</td>");
            table.Append("</tr> <tr>");
            table.Append("<td>素食</td>");
            table.Append("<td>素食盒餐</td>");
            table.Append("<td>素食盒餐</td>");
            table.Append("<td>素食盒餐</td>");
            table.Append("<td>素食盒餐</td>");
            table.Append("<td>素食盒餐</td>");
            table.Append("</tr> </table> <br/>");
            
            string sql = "select lower(substr(mail,instr(mail,',',3)+1)) mail from ltdll_view3 left join tblusruserbasis_view on substr(mail, instr(mail, '*') + 1, instr(mail, '(') - instr(mail, '*') - 1) = tblusruserbasis_view.username left join ltusers on ltusers.luser = tblusruserbasis_view.userno where tblusruserbasis_view.issuestate = 2 and ltusers.LPROGRAM = 'OrderMeal' and userno not like 'LB%' and userno not in (select luser from ltusers where  lprogram='OrderMeal' and LB1=1)";
            System.Data.DataTable tb = ltDll.ltClass.SelectFromMes(sql);
            //if (tb.Rows.Count > 0)
            //{
            //    for (int i = 0; i < tb.Rows.Count; i++)
            //    {
            //        if (tb.Rows[i]["mail"].ToString() != "")
            //        {
            //            ltDll.ltClass.SendEmail("音律 午餐訂餐通知", tb.Rows[i]["mail"].ToString(), "", "", table.ToString());
            //        }
            //    }
            //}
            ltDll.ltClass.SendEmail("下周菜單", "EdwardLin@elytone.com", "", "", table.ToString());

        }
        public static string getlunch(string lunchdate, string lunchtype)
        {
            string sql = "select * from omlunch where LUNCHDATE=to_date('" + lunchdate + "','yyyy-mm-dd') and LUNCHTYPE='" + lunchtype + "'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["LUNCHNAME"].ToString();
            }
            else
            {
                return "";
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
                    }
                }
                else
                {
                    string maxid = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(lunchid)+1,1) from omlunch");
                    string sql3 = string.Format("insert into omlunch (LUNCHDATE,LUNCHTYPE,LUNCHNAME,LUNCHID) values (to_date('{0}', 'yyyy-mm-dd'),'{1}','{2}',{3})", lunchdate, lunchtype, lunchname, maxid);
                    if (ltDll.ltClass.ExecuteToMes(sql3) == true)
                    {
                        ShowNotify("添加成功！", MessageBoxIcon.Success);
                    }
                }
                DatePicker1.SelectedDate = DateTime.Parse(lunchdate);
            }

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


        private DataTable OtherMonthGrid(string MonthDate)
        {
            string sql = "select * from ommonth where monthdate='" + MonthDate + "'";
            DataTable table = ltDll.ltClass.SelectFromMes(sql);
            //ltDll.ltClass.PrintHideMessage(sql);
            return table;
        }


        public static bool checksushi(string date)
        {
            string sql = "select * from omlunch where lunchdate=to_date('" + date + "','yyyy-MM-dd')";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void getEmp()
        {
            string sql = " select * from  tblusruserbasis_view where issuestate=2 and shiftno like '" + shiftno + "%'";
            DataTable dt = ltDll.ltClass.SelectFromMes(sql);
            ddlLA4.DataSource = dt;
            ddlLA4.DataBind();
        }

        protected void ddlLA4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // BindTable no longer needed - new UI uses AJAX/PageMethods
            // DateTime sd = DateTime.Parse(DatePicker1.Text);
            // BindTable(sd);
        }

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    if (fileLunch.HasFile)
        //    {
        //        string fileName = fileLunch.ShortFileName;

        //        if (!ValidateFileType(fileName))
        //        {
        //            ShowNotify("无效的文件类型！");
        //            return;
        //        }
        //        fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
        //        fileName = DateTime.Now.Ticks.ToString() + "_" + fileName;
        //        string path = Server.MapPath("~/upload/" + fileName);
        //        fileLunch.SaveAs(path);
        //        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Workbook wbook = app.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

        //        //将数据读入到DataTable中——Start    
        //        //Microsoft.Office.Interop.Excel.Sheets sheets = wbook.Worksheets;     
        //        Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)wbook.Worksheets[1];
        //        //Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.;//读取第一张表  

        //        var range = ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[2, 2]);
        //        DateTime dt=DateTime.Parse(range.Value2.ToString());
        //        DateTime dt1 = DateTime.FromOADate(double.Parse(range.Value2.ToString()));
        //        var temp = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("B5", "B12");

        //        wbook.Close(false, Type.Missing, Type.Missing);
        //        app.Workbooks.Close();
        //        app.Quit();

        //        fileLunch.Reset();
        //    }
        //}

    }
}

