import sys

filepath = r'c:\ElytoneGit\OrderMeal_Old\FineUIPro.OrderMeal\index.aspx'

with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Normalize line endings
content_normalized = content.replace('\r\n', '\n')

old = ('                        <div class="flex items-center space-x-4">\n'
       '                            <span class="text-gray-600">您好，<span id="userName" class="font-semibold text-primary">\n'
       '                                    <%=username %>\n'
       '                                </span></span>\n'
       '                            <a href="login.aspx" class="text-gray-500 hover:text-primary transition-colors">登出</a>\n'
       '                        </div>')

new = ('                        <div class="flex items-center space-x-4">\n'
       '                            <button type="button" id="noticeSettingsBtn" class="notice-bell-btn" onclick="openNoticeModal()" title="通知設定">\n'
       '                                <svg class="w-5 h-5 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">\n'
       '                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9">\n'
       '                                    </path>\n'
       '                                </svg>\n'
       '                                <span class="text-gray-600">您好，<span class="font-semibold text-primary"><%=username %></span></span>\n'
       '                            </button>\n'
       '                            <a href="login.aspx" class="text-gray-500 hover:text-primary transition-colors">登出</a>\n'
       '                        </div>')

if old in content_normalized:
    result = content_normalized.replace(old, new, 1)
    result = result.replace('\n', '\r\n')
    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(result)
    print('SUCCESS: header replaced')
else:
    # Find approximate location
    idx = content_normalized.find('space-x-4')
    print('NOT FOUND, snippet around space-x-4:')
    print(repr(content_normalized[idx:idx+400]))
    sys.exit(1)
