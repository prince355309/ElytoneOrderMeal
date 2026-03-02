notice_webmethods = '''
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetNoticeSettings()
        {
            try
            {
                string userNo = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
                if (string.IsNullOrEmpty(userNo)) userNo = "EB237003";

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
                string userNo = ltDll.ltClass.SessionRead("OrderMeal", "USERNO");
                if (string.IsNullOrEmpty(userNo)) userNo = "EB237003";

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

'''

# ---- index.aspx.cs ----
filepath_index = r'c:\ElytoneGit\OrderMeal_Old\FineUIPro.OrderMeal\index.aspx.cs'
with open(filepath_index, 'r', encoding='utf-8') as f:
    content = f.read()
content = content.replace('\r\n', '\n')

marker = '\n        [WebMethod]\n        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]\n        public static object LoadWeekData(string startDate, string endDate)\n'
if marker in content:
    content = content.replace(marker, notice_webmethods + '        [WebMethod]\n        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]\n        public static object LoadWeekData(string startDate, string endDate)\n', 1)
    content = content.replace('\n', '\r\n')
    with open(filepath_index, 'w', encoding='utf-8') as f:
        f.write(content)
    print('index.aspx.cs: WebMethods added OK')
else:
    print('index.aspx.cs: marker not found')

# ---- default.aspx.cs ----
filepath_default = r'c:\ElytoneGit\OrderMeal_Old\FineUIPro.OrderMeal\default.aspx.cs'
with open(filepath_default, 'r', encoding='utf-8') as f:
    content = f.read()
content = content.replace('\r\n', '\n')

# In default.aspx.cs, insert before the GetMenuData method (already exists) - 
# Actually insert before LoadWeekData method
marker2 = '\n        [WebMethod]\n        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]\n        public static object LoadWeekData(string startDate, string endDate)\n'
if marker2 in content:
    content = content.replace(marker2, notice_webmethods + '        [WebMethod]\n        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]\n        public static object LoadWeekData(string startDate, string endDate)\n', 1)
    content = content.replace('\n', '\r\n')
    with open(filepath_default, 'w', encoding='utf-8') as f:
        f.write(content)
    print('default.aspx.cs: WebMethods added OK')
else:
    print('default.aspx.cs: marker not found')
    idx = content.find('LoadWeekData')
    print(repr(content[idx-100:idx+50]))
