using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class index : PageBase
    {
        //string userno = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
        //string username = ltDll.ltClass.SessionRead("OrderMeal", "USERNAME");
        //string shiftname = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNAME");
        string userno = "EB237003";
        protected string username;
        string shiftname = "";
        DateTime today = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _DomainName = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).Host;
            if (_DomainName != "localhost")
            {
                userno = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
                username = ltDll.ltClass.SessionRead("OrderMeal", "USERNAME");
                shiftname = ltDll.ltClass.SessionRead("OrderMeal", "SHIFTNAME");
            }
            else
            {
                userno = "EB237003";
                username = "";
                shiftname = "";
            }

            if (!IsPostBack)
            {
                // Set user info
                hfUserNo.Value = userno;
                hfUserName.Value = username;

                // Load week data
                DateTime monday = GetMonday(today);
                var weekData = GetWeekMealsData(monday);
                var orderData = GetUserOrdersData(monday, userno);

                hfWeekData.Value = JsonConvert.SerializeObject(weekData);
                hfOrderData.Value = JsonConvert.SerializeObject(orderData);
            }
        }

        private DateTime GetMonday(DateTime dt)
        {
            int dayOfWeek = DayOfWeek(dt);
            return DateTime.Parse(dt.AddDays(1 - dayOfWeek).ToShortDateString() + " 09:00:00");
        }

        private Dictionary<string, Dictionary<string, object>> GetWeekMealsData(DateTime monday)
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            string[] mealTypes = { "自助餐", "簡餐", "麵食", "輕食餐", "素食" };

            for (int i = 0; i < 5; i++)
            {
                DateTime currentDate = monday.AddDays(i);
                string dateKey = currentDate.ToString("yyyy-MM-dd");
                var dayMeals = new Dictionary<string, object>();

                foreach (string mealType in mealTypes)
                {
                    DataTable dt = _default.omlunch(currentDate.ToShortDateString(), mealType);
                    if (dt.Rows.Count > 0)
                    {
                        string lunchName = dt.Rows[0]["LUNCHNAME"].ToString();
                        string lunchId = dt.Rows[0]["LUNCHID"].ToString();

                        if (!string.IsNullOrEmpty(lunchName) && lunchName != "\r\n")
                        {
                            dayMeals[mealType] = new
                            {
                                id = lunchId,
                                name = _default.replacebr(lunchName),
                                available = true
                            };
                        }
                    }
                }

                // Always add vegetarian option
                if (!dayMeals.ContainsKey("素食"))
                {
                    dayMeals["素食"] = new
                    {
                        id = "0",
                        name = "素食盒餐",
                        available = !_default.checksushi(currentDate.ToShortDateString())
                    };
                }

                result[dateKey] = dayMeals;
            }

            return result;
        }

        private Dictionary<string, object> GetUserOrdersData(DateTime monday, string userNo)
        {
            var result = new Dictionary<string, object>();

            for (int i = 0; i < 5; i++)
            {
                DateTime currentDate = monday.AddDays(i);
                string dateKey = currentDate.ToString("yyyy-MM-dd");

                // Check all meal types for this day
                string[] mealTypes = { "自助餐", "簡餐", "麵食", "輕食餐" };
                bool found = false;

                foreach (string mealType in mealTypes)
                {
                    DataTable dt = _default.omlunch(currentDate.ToShortDateString(), mealType);
                    if (dt.Rows.Count > 0)
                    {
                        string lunchId = dt.Rows[0]["LUNCHID"].ToString();
                        string checkedResult = _default.Checked(currentDate.ToShortDateString(), userNo, lunchId);
                        if (!string.IsNullOrEmpty(checkedResult))
                        {
                            result[dateKey] = new
                            {
                                lunchId = lunchId,
                                type = mealType,
                                orderNum = "1"
                            };
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    // Check for vegetarian or no-meal
                    string vegetarianCheck = _default.Checked(currentDate.ToShortDateString(), userNo, "0");
                    if (vegetarianCheck == "0")
                    {
                        result[dateKey] = new
                        {
                            lunchId = "0",
                            type = "不訂餐",
                            orderNum = "1"
                        };
                    }
                    else if (!string.IsNullOrEmpty(vegetarianCheck))
                    {
                        result[dateKey] = new
                        {
                            lunchId = "0",
                            type = "素食",
                            orderNum = "1"
                        };
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// WebMethod 專用：取得當前使用者的 UserNo。
        /// 邏輯與 Page_Load 相同 — localhost 回傳測試帳號，正式環境從 Session 讀取。
        /// </summary>
        private static string GetCurrentUserNo()
        {
            string host = HttpContext.Current.Request.Url.Host;
            if (host == "localhost")
                return "EB237003";
            return ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
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

                // Create a temporary instance to call non-static methods
                index instance = new index();
                var mealsData = instance.GetWeekMealsData(monday);
                var ordersData = instance.GetUserOrdersData(monday, userNo);

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

                DateTime today = DateTime.Now;

                if (string.IsNullOrEmpty(userNo))
                {
                    return new { success = false, message = "請重新登入" };
                }

                var orders = JsonConvert.DeserializeObject<Dictionary<string, OrderInfo>>(ordersJson);
                DateTime dtime = GetLatestDate(today);

                foreach (var kvp in orders)
                {
                    string dateStr = kvp.Key;
                    OrderInfo order = kvp.Value;

                    DateTime orderDate = DateTime.Parse(dateStr + " 09:00:00");

                    // Allow same-day ordering before 09:00; otherwise allow today plus the next 2 workdays
                    DateTime todayCutoff = today.Date.AddHours(9);

                    if (orderDate.Date < today.Date)
                        continue;

                    if (orderDate.Date == today.Date && today >= todayCutoff)
                        continue;

                    if (IsWeekend(orderDate))
                        continue;

                    if (orderDate.Date > dtime.Date)
                        continue;

                    // Get existing order
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
                    else if (orderNum == "1")
                    {
                        string maxId = ltDll.ltClass.SelectFromMesFirstRow("select nvl(max(orderid)+1,1) from omorder");
                        string insertSql = string.Format(
                            "insert into omorder (ORDERID,ORDERDATE,ORDERNUM,LUNCHID,USERNO,ORDERIP,updatedate,username,shiftname) values ({0},to_date('{1}', 'yyyy-mm-dd'),{2},{3},'{4}','{5}',sysdate,'{6}','{7}')",
                            maxId, orderDate.ToShortDateString(), orderNum, lunchId, userNo, ltDll.ltClass.GetIPAddress(), userName, shiftName);
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

        public class OrderInfo
        {
            public string lunchId { get; set; }
            public string type { get; set; }
            public string orderNum { get; set; }
        }

        public static int DayOfWeek(DateTime dt)
        {
            string strDayOfWeek = dt.DayOfWeek.ToString().ToLower();
            int Week = 0;
            switch (strDayOfWeek)
            {
                case "monday": Week = 1; break;
                case "tuesday": Week = 2; break;
                case "wednesday": Week = 3; break;
                case "thursday": Week = 4; break;
                case "friday": Week = 5; break;
                case "saturday": Week = 6; break;
                case "sunday": Week = 7; break;
            }
            return Week;
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == System.DayOfWeek.Saturday || date.DayOfWeek == System.DayOfWeek.Sunday;
        }

        public static DateTime GetLatestDate(DateTime dt)
        {
            DateTime latestDate = dt.Date;
            int workdayCount = 0;

            while (workdayCount < 2)
            {
                latestDate = latestDate.AddDays(1);

                if (IsWeekend(latestDate))
                {
                    continue;
                }

                workdayCount++;
            }

            return latestDate.AddHours(9);
        }
    }
}

