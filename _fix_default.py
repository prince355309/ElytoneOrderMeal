filepath = r'c:\ElytoneGit\OrderMeal_Old\FineUIPro.OrderMeal\default.aspx'

with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

content = content.replace('\r\n', '\n')

# ===========================================================
# 1. Add notice CSS after the last existing .modal-overlay.active .modal-content block
# ===========================================================
old_css_end = '''            .f-panel,\n            .f-contentpanel {\n                position: static !important;\n                width: auto !important;\n                height: auto !important;\n            }\n        </style>\n    </head>'''

notice_css = '''
            /* Notice Settings Modal */
            .notice-modal-content { max-width: 420px; }
            .notice-header { padding: 20px 24px; border-bottom: 1px solid #f3f4f6; display: flex; align-items: center; justify-content: space-between; }
            .notice-header h3 { font-size: 1.05rem; font-weight: 700; color: #1f2937; margin: 0; display: flex; align-items: center; gap: 8px; }
            .notice-close-btn { width: 32px; height: 32px; border-radius: 50%; border: none; background: #f3f4f6; cursor: pointer; display: flex; align-items: center; justify-content: center; font-size: 1rem; color: #6b7280; transition: all 0.2s; }
            .notice-close-btn:hover { background: #fee2e2; color: #ef4444; }
            .notice-body { padding: 24px; }
            .notice-item { display: flex; align-items: center; justify-content: space-between; padding: 16px; border-radius: 12px; background: #fafafa; border: 1.5px solid #e5e7eb; margin-bottom: 12px; transition: border-color 0.2s, background 0.2s; }
            .notice-item.enabled { background: #fef9f3; border-color: #e78232; }
            .notice-item-info { display: flex; align-items: center; gap: 12px; }
            .notice-item-icon { width: 40px; height: 40px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-size: 1.3rem; background: #e78232; color: white; }
            .notice-item-icon.dingtalk { background: #1677ff; }
            .notice-item-text h4 { font-size: 0.9rem; font-weight: 700; color: #1f2937; margin: 0 0 2px 0; }
            .notice-item-text p { font-size: 0.78rem; color: #6b7280; margin: 0; }
            .toggle-switch { position: relative; width: 48px; height: 26px; flex-shrink: 0; }
            .toggle-switch input { opacity: 0; width: 0; height: 0; }
            .toggle-slider { position: absolute; cursor: pointer; top: 0; left: 0; right: 0; bottom: 0; background: #d1d5db; border-radius: 26px; transition: background 0.3s; }
            .toggle-slider:before { position: absolute; content: ""; height: 20px; width: 20px; left: 3px; bottom: 3px; background: white; border-radius: 50%; transition: transform 0.3s; box-shadow: 0 1px 3px rgba(0,0,0,0.2); }
            .toggle-switch input:checked + .toggle-slider { background: #e78232; }
            .toggle-switch input:checked + .toggle-slider:before { transform: translateX(22px); }
            .notice-footer { padding: 16px 24px; border-top: 1px solid #f3f4f6; display: flex; justify-content: flex-end; gap: 10px; }
            .notice-btn { padding: 9px 22px; border-radius: 10px; font-size: 0.88rem; font-weight: 600; cursor: pointer; border: none; transition: all 0.2s; font-family: inherit; }
            .notice-btn-cancel { background: #f3f4f6; color: #6b7280; }
            .notice-btn-cancel:hover { background: #e5e7eb; }
            .notice-btn-save { background: linear-gradient(135deg, #e78232, #f59e0b); color: white; box-shadow: 0 2px 8px rgba(231,130,50,0.3); }
            .notice-btn-save:hover { transform: translateY(-1px); box-shadow: 0 4px 12px rgba(231,130,50,0.4); }
            .notice-bell-btn { display: inline-flex; align-items: center; gap: 6px; cursor: pointer; background: none; border: none; padding: 4px 8px; border-radius: 8px; transition: background 0.2s; font-family: inherit; }
            .notice-bell-btn:hover { background: rgba(231,130,50,0.1); }
        </style>
    </head>'''

new_css_end = old_css_end.replace(
    '        </style>\n    </head>',
    notice_css
)

if old_css_end in content:
    content = content.replace(old_css_end, new_css_end, 1)
    print('CSS added OK')
else:
    print('CSS target not found')
    print(repr(content[content.find('.f-contentpanel {'):content.find('.f-contentpanel {')+200]))

# ===========================================================
# 2. Replace static "管理員" header with clickable bell button
# ===========================================================
old_header = ('                                    <div class="flex items-center space-x-4">\n'
              '                                        <span class="text-gray-600 hidden sm:inline">您好，<span\n'
              '                                                class="font-semibold text-primary">管理員</span></span>\n'
              '                                        <a href="login.aspx"\n'
              '                                            class="text-gray-500 hover:text-primary transition-colors">登出</a>\n'
              '                                    </div>')

new_header = ('                                    <div class="flex items-center space-x-4">\n'
              '                                        <button type="button" id="noticeSettingsBtn" class="notice-bell-btn" onclick="openNoticeModal()" title="通知設定">\n'
              '                                            <svg class="w-5 h-5 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">\n'
              '                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9">\n'
              '                                                </path>\n'
              '                                            </svg>\n'
              '                                            <span class="text-gray-600 hidden sm:inline">您好，<span class="font-semibold text-primary"><%=username %></span></span>\n'
              '                                        </button>\n'
              '                                        <a href="login.aspx" class="text-gray-500 hover:text-primary transition-colors">登出</a>\n'
              '                                    </div>')

if old_header in content:
    content = content.replace(old_header, new_header, 1)
    print('Header replaced OK')
else:
    print('Header target not found')
    idx = content.find('管理員')
    print(repr(content[idx-300:idx+100]))

# ===========================================================
# 3. Add Notice Modal HTML before </form>
# ===========================================================
old_form_close = '        </form>\n\n        <!-- Toast -->'

notice_modal = '''        <!-- Notice Settings Modal -->
        <div id="noticeModal" class="modal-overlay" onclick="closeNoticeModal(event)">
            <div class="modal-content notice-modal-content" onclick="event.stopPropagation()">
                <div class="notice-header">
                    <h3>
                        <svg class="w-5 h-5 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9">
                            </path>
                        </svg>
                        通知設定
                    </h3>
                    <button type="button" class="notice-close-btn" onclick="closeNoticeModal()">✕</button>
                </div>
                <div class="notice-body">
                    <p class="text-sm text-gray-500 mb-4">設定您希望接收的通知方式，系統將依據設定發送訂餐相關通知。</p>
                    <!-- Mail -->
                    <div class="notice-item" id="noticeItemMail">
                        <div class="notice-item-info">
                            <div class="notice-item-icon">
                                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z">
                                    </path>
                                </svg>
                            </div>
                            <div class="notice-item-text">
                                <h4>電子郵件通知</h4>
                                <p>接收訂餐確認及菜單通知 Email</p>
                            </div>
                        </div>
                        <label class="toggle-switch">
                            <input type="checkbox" id="toggleMail" onchange="updateNoticeItemStyle('Mail', this.checked)">
                            <span class="toggle-slider"></span>
                        </label>
                    </div>
                    <!-- DingTalk -->
                    <div class="notice-item" id="noticeItemDingTalk">
                        <div class="notice-item-info">
                            <div class="notice-item-icon dingtalk">🔔</div>
                            <div class="notice-item-text">
                                <h4>釘釘通知</h4>
                                <p>接收釘釘訂餐提醒及菜單推送</p>
                            </div>
                        </div>
                        <label class="toggle-switch">
                            <input type="checkbox" id="toggleDingTalk" onchange="updateNoticeItemStyle('DingTalk', this.checked)">
                            <span class="toggle-slider"></span>
                        </label>
                    </div>
                </div>
                <div class="notice-footer">
                    <button type="button" class="notice-btn notice-btn-cancel" onclick="closeNoticeModal()">取消</button>
                    <button type="button" class="notice-btn notice-btn-save" onclick="saveNoticeSettings()">💾 儲存設定</button>
                </div>
            </div>
        </div>
        </form>

        <!-- Toast -->'''

if old_form_close in content:
    content = content.replace(old_form_close, notice_modal, 1)
    print('Notice modal added OK')
else:
    print('Form close not found')
    idx = content.find('</form>')
    print(repr(content[idx-50:idx+50]))

# ===========================================================
# 4. Add Notice JS functions at the beginning of the <script> block
# ===========================================================
old_script_start = '        <script>\n            // === Global state (same as index.aspx) ==='

notice_js = '''        <script>
            // ===== Notice Settings Functions =====
            function openNoticeModal() {
                document.getElementById('noticeModal').classList.add('active');
                loadNoticeSettings();
            }
            function closeNoticeModal(e) {
                if (e && e.target && e.target.id !== 'noticeModal') return;
                document.getElementById('noticeModal').classList.remove('active');
            }
            function updateNoticeItemStyle(type, checked) {
                var itemId = 'noticeItem' + (type === 'Mail' ? 'Mail' : 'DingTalk');
                var el = document.getElementById(itemId);
                if (el) { if (checked) el.classList.add('enabled'); else el.classList.remove('enabled'); }
            }
            function loadNoticeSettings() {
                PageMethods.GetNoticeSettings(function (result) {
                    if (result && result.success) {
                        document.getElementById('toggleMail').checked = result.mailEnabled;
                        document.getElementById('toggleDingTalk').checked = result.dingTalkEnabled;
                        updateNoticeItemStyle('Mail', result.mailEnabled);
                        updateNoticeItemStyle('DingTalk', result.dingTalkEnabled);
                    }
                }, function (err) { console.error('GetNoticeSettings error:', err); });
            }
            function saveNoticeSettings() {
                var mailEnabled = document.getElementById('toggleMail').checked ? 1 : 0;
                var dingTalkEnabled = document.getElementById('toggleDingTalk').checked ? 1 : 0;
                PageMethods.SaveNoticeSettings(mailEnabled, dingTalkEnabled, function (result) {
                    if (result && result.success) { showToast('通知設定已儲存！'); closeNoticeModal(); }
                    else showToast(result.message || '儲存失敗', false);
                }, function (err) { showToast('發生錯誤，請稍後再試', false); });
            }

            // === Global state (same as index.aspx) ==='''

if old_script_start in content:
    content = content.replace(old_script_start, notice_js, 1)
    print('JS functions added OK')
else:
    print('Script start not found')
    idx = content.find('<script>')
    print(repr(content[idx:idx+200]))

# ===========================================================
# Write output
# ===========================================================
content = content.replace('\n', '\r\n')
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

print('default.aspx DONE')
