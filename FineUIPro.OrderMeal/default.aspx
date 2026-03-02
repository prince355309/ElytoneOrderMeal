<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="FineUIPro.OrderMeal._default" %>

<!DOCTYPE html>
<html lang="zh-TW">

<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>團膳管理系統</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        primary: { DEFAULT: '#e78232', light: '#f5a862', dark: '#c66a1f', 50: '#fef9f3', 100: '#fdf0e3', 200: '#fbe0c7', 500: '#e78232', 600: '#c66a1f', 700: '#a5571a' },
                        success: { DEFAULT: '#2E7D32', light: '#4CAF50' }
                    }
                }
            }
        }
    </script>
    <style>
        body {
            background: linear-gradient(135deg, #fef9f3 0%, #f8f4f0 100%);
            min-height: 100vh;
        }



        .card-hover {
            transition: all 0.3s ease;
        }



            .card-hover:hover {
                transform: translateY(-4px);
                box-shadow: 0 12px 24px -8px rgba(231, 130, 50, 0.25);
            }



        .meal-selected {
            border-color: #2E7D32 !important;
            background: linear-gradient(135deg, #f0fdf0 0%, #e8f5e9 100%);
        }



            .meal-selected .select-btn {
                background-color: #2E7D32 !important;
                color: white !important;
            }



        .date-tab.active {
            background-color: #e78232;
            color: white;
        }



        .img-placeholder {
            background: linear-gradient(135deg, #fef9f3 0%, #fbe0c7 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 3rem;
        }



        .scrollbar-hide::-webkit-scrollbar {
            display: none;
        }



        .scrollbar-hide {
            -ms-overflow-style: none;
            scrollbar-width: none;
        }



        .modal-overlay {
            position: fixed;
            inset: 0;
            background: rgba(0, 0, 0, 0.5);
            z-index: 100;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 1rem;
            opacity: 0;
            visibility: hidden;
            transition: all 0.3s ease;
        }



            .modal-overlay.active {
                opacity: 1;
                visibility: visible;
            }



        .modal-content {
            background: white;
            border-radius: 1.5rem;
            max-width: 600px;
            width: 100%;
            max-height: 90vh;
            overflow-y: auto;
            transform: scale(0.9) translateY(20px);
            transition: all 0.3s ease;
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
        }



        .modal-overlay.active .modal-content {
            transform: scale(1) translateY(0);
        }



        /* Admin modal form styling */

        .admin-modal .modal-content {
            max-width: 520px;
        }



        .admin-modal-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 20px 24px;
            border-bottom: 1px solid #f3f4f6;
        }



            .admin-modal-header h3 {
                font-size: 1.1rem;
                font-weight: 700;
                color: #1f2937;
                margin: 0;
            }



        .admin-modal-close {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            border: none;
            background: #f3f4f6;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.1rem;
            color: #6b7280;
            transition: all 0.2s;
        }



            .admin-modal-close:hover {
                background: #fee2e2;
                color: #ef4444;
            }



        .admin-modal-body {
            padding: 24px;
        }



        .admin-form-group {
            margin-bottom: 16px;
        }



            .admin-form-group label {
                display: block;
                font-size: 0.85rem;
                font-weight: 600;
                color: #374151;
                margin-bottom: 6px;
            }



                .admin-form-group label .required {
                    color: #ef4444;
                    margin-left: 2px;
                }



            .admin-form-group input,
            .admin-form-group select,
            .admin-form-group textarea {
                width: 100%;
                padding: 10px 14px;
                border: 1.5px solid #e5e7eb;
                border-radius: 10px;
                font-size: 0.9rem;
                color: #1f2937;
                background: #fafafa;
                transition: border-color 0.2s, box-shadow 0.2s;
                font-family: inherit;
                box-sizing: border-box;
            }



                .admin-form-group input:focus,
                .admin-form-group select:focus,
                .admin-form-group textarea:focus {
                    outline: none;
                    border-color: #e78232;
                    box-shadow: 0 0 0 3px rgba(231, 130, 50, 0.1);
                    background: white;
                }



            .admin-form-group textarea {
                resize: vertical;
                min-height: 140px;
            }



        .admin-form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 16px;
        }



        .admin-modal-footer {
            padding: 16px 24px;
            border-top: 1px solid #f3f4f6;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }



        .admin-btn {
            padding: 10px 24px;
            border-radius: 10px;
            font-size: 0.9rem;
            font-weight: 600;
            cursor: pointer;
            border: none;
            transition: all 0.2s;
            font-family: inherit;
        }



        .admin-btn-cancel {
            background: #f3f4f6;
            color: #6b7280;
        }



            .admin-btn-cancel:hover {
                background: #e5e7eb;
            }



        .admin-btn-primary {
            background: linear-gradient(135deg, #e78232, #f59e0b);
            color: white;
            box-shadow: 0 2px 8px rgba(231, 130, 50, 0.3);
        }



            .admin-btn-primary:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(231, 130, 50, 0.4);
            }



        /* Sidebar */

        .admin-sidebar {
            position: fixed;
            top: 64px;
            left: 0;
            bottom: 0;
            width: 280px;
            z-index: 90;
            background: white;
            border-right: 1px solid rgba(0, 0, 0, 0.06);
            box-shadow: 4px 0 24px rgba(0, 0, 0, 0.04);
            transform: translateX(-100%);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            overflow-y: auto;
            padding: 16px 0;
        }



        .sidebar-open .admin-sidebar {
            transform: translateX(0);
        }



        .sidebar-overlay {
            position: fixed;
            inset: 0;
            top: 64px;
            z-index: 85;
            background: rgba(0, 0, 0, 0.3);
            opacity: 0;
            visibility: hidden;
            transition: all 0.3s ease;
        }



        .sidebar-open .sidebar-overlay {
            opacity: 1;
            visibility: visible;
        }



        .sidebar-section-title {
            padding: 12px 24px 8px;
            font-size: 0.7rem;
            font-weight: 600;
            color: #9ca3af;
            text-transform: uppercase;
            letter-spacing: 0.08em;
        }



        .sidebar-nav {
            list-style: none;
            padding: 0;
            margin: 0;
        }



            .sidebar-nav li button {
                display: flex;
                align-items: center;
                gap: 12px;
                padding: 12px 24px;
                width: 100%;
                font-size: 0.9rem;
                color: #374151;
                border: none;
                background: none;
                cursor: pointer;
                text-align: left;
                transition: all 0.2s;
                border-left: 3px solid transparent;
                font-family: inherit;
            }



                .sidebar-nav li button:hover {
                    background: #fef9f3;
                    color: #e78232;
                }



                .sidebar-nav li button.active {
                    background: #fef9f3;
                    color: #e78232;
                    border-left-color: #e78232;
                    font-weight: 600;
                }



        .sidebar-divider {
            height: 1px;
            background: #f3f4f6;
            margin: 8px 16px;
        }



        .hamburger-btn span {
            display: block;
            width: 20px;
            height: 2px;
            background: white;
            border-radius: 2px;
            transition: all 0.3s ease;
        }



        .sidebar-open .hamburger-btn span:nth-child(1) {
            transform: translateY(7px) rotate(45deg);
        }



        .sidebar-open .hamburger-btn span:nth-child(2) {
            opacity: 0;
        }



        .sidebar-open .hamburger-btn span:nth-child(3) {
            transform: translateY(-7px) rotate(-45deg);
        }



        /* Quantity Stepper */

        .qty-stepper {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            background: #f3f4f6;
            border-radius: 10px;
            padding: 2px;
        }



            .qty-stepper button {
                width: 28px;
                height: 28px;
                border: none;
                border-radius: 8px;
                background: white;
                font-size: 1rem;
                font-weight: 700;
                cursor: pointer;
                display: flex;
                align-items: center;
                justify-content: center;
                color: #374151;
                transition: all 0.2s;
                box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
            }



                .qty-stepper button:hover {
                    background: #e78232;
                    color: white;
                }



            .qty-stepper .qty-val {
                min-width: 28px;
                text-align: center;
                font-weight: 700;
                font-size: 0.9rem;
                color: #1f2937;
            }



        /* Report iframe */

        .report-frame {
            background: white;
            border-radius: 1rem;
            overflow: hidden;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
            height: calc(100vh - 140px);
        }



            .report-frame iframe {
                width: 100%;
                height: calc(100% - 56px);
                border: none;
            }



        .content-section {
            display: none;
        }



            .content-section.active {
                display: block;
            }



        /* FineUI overrides */

        .f-mainheader,
        .f-panel-header,
        .f-tab-header,
        .f-region-proxy,
        .f-panel-border {
            display: none !important;
        }



        .f-panel-body,
        .f-contentpanel-body {
            border: none !important;
            background: transparent !important;
            padding: 0 !important;
            overflow: visible !important;
        }



        .f-panel,
        .f-contentpanel {
            position: static !important;
            width: auto !important;
            height: auto !important;
        }



        /* Notice Settings Modal */

        .notice-modal-content {
            max-width: 420px;
        }

        .notice-header {
            padding: 20px 24px;
            border-bottom: 1px solid #f3f4f6;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

            .notice-header h3 {
                font-size: 1.05rem;
                font-weight: 700;
                color: #1f2937;
                margin: 0;
                display: flex;
                align-items: center;
                gap: 8px;
            }

        .notice-close-btn {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            border: none;
            background: #f3f4f6;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            color: #6b7280;
            transition: all 0.2s;
        }

            .notice-close-btn:hover {
                background: #fee2e2;
                color: #ef4444;
            }

        .notice-body {
            padding: 24px;
        }

        .notice-item {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 16px;
            border-radius: 12px;
            background: #fafafa;
            border: 1.5px solid #e5e7eb;
            margin-bottom: 12px;
            transition: border-color 0.2s, background 0.2s;
        }

            .notice-item.enabled {
                background: #fef9f3;
                border-color: #e78232;
            }

        .notice-item-info {
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .notice-item-icon {
            width: 40px;
            height: 40px;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.3rem;
            background: #e78232;
            color: white;
        }

            .notice-item-icon.dingtalk {
                background: #1677ff;
            }

        .notice-item-text h4 {
            font-size: 0.9rem;
            font-weight: 700;
            color: #1f2937;
            margin: 0 0 2px 0;
        }

        .notice-item-text p {
            font-size: 0.78rem;
            color: #6b7280;
            margin: 0;
        }

        .toggle-switch {
            position: relative;
            width: 48px;
            height: 26px;
            flex-shrink: 0;
        }

            .toggle-switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .toggle-slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: #d1d5db;
            border-radius: 26px;
            transition: background 0.3s;
        }

            .toggle-slider:before {
                position: absolute;
                content: "";
                height: 20px;
                width: 20px;
                left: 3px;
                bottom: 3px;
                background: white;
                border-radius: 50%;
                transition: transform 0.3s;
                box-shadow: 0 1px 3px rgba(0,0,0,0.2);
            }

        .toggle-switch input:checked + .toggle-slider {
            background: #e78232;
        }

            .toggle-switch input:checked + .toggle-slider:before {
                transform: translateX(22px);
            }

        .notice-footer {
            padding: 16px 24px;
            border-top: 1px solid #f3f4f6;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .notice-btn {
            padding: 9px 22px;
            border-radius: 10px;
            font-size: 0.88rem;
            font-weight: 600;
            cursor: pointer;
            border: none;
            transition: all 0.2s;
            font-family: inherit;
        }

        .notice-btn-cancel {
            background: #f3f4f6;
            color: #6b7280;
        }

            .notice-btn-cancel:hover {
                background: #e5e7eb;
            }

        .notice-btn-save {
            background: linear-gradient(135deg, #e78232, #f59e0b);
            color: white;
            box-shadow: 0 2px 8px rgba(231,130,50,0.3);
        }

            .notice-btn-save:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(231,130,50,0.4);
            }

        .notice-bell-btn {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            cursor: pointer;
            background: none;
            border: none;
            padding: 4px 8px;
            border-radius: 8px;
            transition: background 0.2s;
            font-family: inherit;
        }

            .notice-bell-btn:hover {
                background: rgba(231,130,50,0.1);
            }
    </style>
</head>

<body class="font-sans">
    <div class="sidebar-overlay" onclick="toggleSidebar()"></div>
    <!-- Sidebar -->
    <nav class="admin-sidebar">
        <div class="sidebar-section-title">訂餐管理</div>
        <ul class="sidebar-nav">
            <li>
                <button type="button" class="active" onclick="showSection('menu',this)"><span>📋</span>菜單訂餐</button>
            </li>
        </ul>
        <div class="sidebar-divider"></div>
        <div class="sidebar-section-title">管理功能</div>
        <ul class="sidebar-nav">
            <li>
                <button type="button" id="sidebarBtnAdd"
                    onclick="openMenuEditWindow(); closeSidebar();">
                    <span>✏️</span>添加/修改菜單</button></li>
            <li>
                <button type="button" onclick="doSendNextWeek()"><span>📧</span>發送下周菜單</button></li>
            <li>
                <button type="button" onclick="showDateSendUI()"><span>📅</span>選擇日期發送</button></li>
        </ul>
        <div class="sidebar-divider"></div>
        <div class="sidebar-section-title">報表查詢</div>
        <ul class="sidebar-nav">
            <li>
                <button type="button" onclick="showSection('report1',this)"><span>📊</span>報表 1</button></li>
            <li>
                <button type="button" onclick="showSection('report2',this)"><span>📈</span>報表 2</button></li>
            <li>
                <button type="button" onclick="showSection('report3',this)"><span>👤</span>沒有訂餐的員工</button></li>
            <li>
                <button type="button" onclick="showSection('report4',this)"><span>🚫</span>不訂餐的員工</button></li>
            <li>
                <button type="button" onclick="showSection('report5',this)"><span>🏢</span>部門訂餐統計</button></li>
        </ul>
    </nav>

    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server"></f:PageManager>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:HiddenField ID="hfUserNo" runat="server" />
        <asp:HiddenField ID="hfUserName" runat="server" />
        <asp:HiddenField ID="hfWeekData" runat="server" />
        <asp:HiddenField ID="hfOrderData" runat="server" />

        <%-- All FineUI controls hidden - we use custom modals for UI --%>
        <div style="display: none;">
            <f:Button ID="btnSend" Text="發送" runat="server" OnClick="btnSend_Click"></f:Button>
            <f:Button ID="btnAdd" Text="添加" runat="server" Hidden="true"></f:Button>
            <f:Button ID="btnSave" Text="保存" runat="server" OnClick="btnSave_Click"></f:Button>
            <f:Button ID="btnDateSend" Text="發送" runat="server" OnClick="btnDateSend_Click"></f:Button>
            <f:DatePicker ID="DatePicker1" runat="server" AutoPostBack="true"
                OnTextChanged="DatePicker1_TextChanged">
            </f:DatePicker>
            <f:DatePicker ID="dpdate" runat="server" AutoPostBack="true" OnTextChanged="dpdate_TextChanged">
            </f:DatePicker>
            <f:DatePicker ID="DatePicker2" runat="server"></f:DatePicker>
            <f:DatePicker ID="DatePicker3" runat="server" CompareControl="DatePicker2"
                CompareOperator="GreaterThanEqual" CompareMessage="結束日期應大於開始日期">
            </f:DatePicker>
            <f:DropDownList ID="ddltype" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                <f:ListItem Text="自助餐" Value="自助餐" />
                <f:ListItem Text="簡餐" Value="簡餐" />
                <f:ListItem Text="麵食" Value="麵食" />
                <f:ListItem Text="輕食餐" Value="輕食餐" />
            </f:DropDownList>
            <f:TextArea ID="taname" runat="server" />
            <f:Panel ID="PLA4" ShowHeader="false" runat="server">
                <Items>
                    <f:DropDownList runat="server" DataTextField="username" DataValueField="userno" ID="ddlLA4"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlLA4_SelectedIndexChanged" Hidden="true"
                        Label="員工">
                    </f:DropDownList>
                </Items>
            </f:Panel>
        </div>

        <%-- Custom Modal: 菜單編輯 --%>
        <div class="modal-overlay admin-modal" id="menuEditModal">
            <div class="modal-content">
                <div class="admin-modal-header">
                    <h3>✈️ 添加 / 修改菜單</h3>
                    <button type="button" class="admin-modal-close"
                        onclick="closeMenuEditModal()">
                        ✕</button>
                </div>
                <div class="admin-modal-body">
                    <div class="admin-form-row">
                        <div class="admin-form-group">
                            <label>日期 <span class="required">*</span></label>
                            <input type="date" id="menuDate" />
                        </div>
                        <div class="admin-form-group">
                            <label>類型 <span class="required">*</span></label>
                            <select id="menuType">
                                <option value="自助餐">自助餐</option>
                                <option value="簡餐">簡餐</option>
                                <option value="麵食">麵食</option>
                                <option value="輕食餐">輕食餐</option>
                            </select>
                        </div>
                    </div>
                    <div class="admin-form-group">
                        <label>描述 / 菜單內容</label>
                        <textarea id="menuDesc" placeholder="請輸入菜單描述..."></textarea>
                    </div>
                </div>
                <div class="admin-modal-footer">
                    <button type="button" class="admin-btn admin-btn-cancel"
                        onclick="closeMenuEditModal()">
                        取消</button>
                    <button type="button" class="admin-btn admin-btn-primary" onclick="submitMenuEdit()">
                        💾
                                    保存</button>
                </div>
            </div>
        </div>

        <%-- Custom Modal: 選擇日期發送 --%>
        <div class="modal-overlay admin-modal" id="dateSendModal">
            <div class="modal-content" style="max-width: 420px">
                <div class="admin-modal-header">
                    <h3>📧 選擇日期發送菜單</h3>
                    <button type="button" class="admin-modal-close"
                        onclick="closeDateSendModal()">
                        ✕</button>
                </div>
                <div class="admin-modal-body">
                    <div class="admin-form-row">
                        <div class="admin-form-group">
                            <label>起始日期 <span class="required">*</span></label>
                            <input type="date" id="sendStartDate" />
                        </div>
                        <div class="admin-form-group">
                            <label>結束日期 <span class="required">*</span></label>
                            <input type="date" id="sendEndDate" />
                        </div>
                    </div>
                </div>
                <div class="admin-modal-footer">
                    <button type="button" class="admin-btn admin-btn-cancel"
                        onclick="closeDateSendModal()">
                        取消</button>
                    <button type="button" class="admin-btn admin-btn-primary"
                        onclick="submitDateSend()">
                        📧 發送</button>
                </div>
            </div>
        </div>

        <!-- Header -->
        <header class="bg-white shadow-sm sticky top-0 z-50">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div class="flex justify-between items-center h-16">
                    <div class="flex items-center space-x-3">
                        <button type="button"
                            class="hamburger-btn w-10 h-10 bg-primary rounded-lg flex flex-col items-center justify-center gap-1.5 hover:bg-primary-dark transition-all"
                            onclick="toggleSidebar()">
                            <span></span><span></span><span></span>
                        </button>
                        <h1 class="text-xl font-bold text-gray-800">企業團膳點餐系統</h1>
                    </div>
                    <div class="flex items-center space-x-4">
                        <button type="button" id="noticeSettingsBtn" class="notice-bell-btn" onclick="openNoticeModal()" title="通知設定">
                            <svg class="w-5 h-5 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9">
                                </path>
                            </svg>
                            <span class="text-gray-600 hidden sm:inline">您好，<span class="font-semibold text-primary"><%=username %></span></span>
                        </button>
                        <a href="login.aspx" class="text-gray-500 hover:text-primary transition-colors">登出</a>
                    </div>
                </div>
            </div>
        </header>

        <!-- Section: Menu (main content, same as index.aspx) -->
        <div id="sectionMenu" class="content-section active">
            <!-- Date Tabs -->
            <div class="bg-white border-b sticky top-16 z-40">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div class="flex items-center py-3 space-x-3">
                        <button type="button" onclick="changeWeek(-1)"
                            class="flex-shrink-0 w-10 h-10 flex items-center justify-center rounded-full bg-gray-100 text-gray-600 hover:bg-primary hover:text-white transition-all duration-300">
                            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M15 19l-7-7 7-7">
                                </path>
                            </svg>
                        </button>
                        <div class="flex-1 overflow-x-auto scrollbar-hide">
                            <div class="flex space-x-2" id="dateTabs"></div>
                        </div>
                        <button type="button" onclick="changeWeek(1)"
                            class="flex-shrink-0 w-10 h-10 flex items-center justify-center rounded-full bg-gray-100 text-gray-600 hover:bg-primary hover:text-white transition-all duration-300">
                            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M9 5l7 7-7 7">
                                </path>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
            <!-- Alert -->
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="bg-primary-50 border-l-4 border-primary p-4 rounded-r-lg">
                    <div class="flex">
                        <div class="flex-shrink-0">
                            <svg class="h-5 w-5 text-primary" viewBox="0 0 20 20"
                                fill="currentColor">
                                <path fill-rule="evenodd"
                                    d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z"
                                    clip-rule="evenodd" />
                            </svg>
                        </div>
                        <div class="ml-3">
                            <p class="text-sm text-primary-700">
                                點餐需於<strong>前一日 16:00
                                                    前</strong>完成選訂，取消訂餐可於<strong>當天 08:40 前</strong>取消。請節省資源，避免餐食浪費！
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Main Content -->
            <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6 pb-96 lg:pb-6">
                <div class="lg:flex lg:space-x-6">
                    <div class="lg:flex-1">
                        <div class="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-4"
                            id="mealCards">
                        </div>
                    </div>
                    <!-- Weekly Summary Desktop -->
                    <div class="hidden lg:block lg:w-80">
                        <div class="bg-white rounded-2xl shadow-lg p-6 sticky top-36">
                            <h3 class="text-lg font-bold text-gray-800 mb-4 flex items-center">
                                <svg class="w-5 h-5 mr-2 text-primary" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round"
                                        stroke-width="2"
                                        d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4">
                                    </path>
                                </svg>
                                本週登記概況
                            </h3>
                            <div id="weeklySummaryDesktop" class="space-y-3"></div>
                            <button type="button" onclick="saveAllOrders()"
                                class="w-full mt-6 bg-primary hover:bg-primary-dark text-white font-semibold py-3 px-6 rounded-xl transition-all duration-300 flex items-center justify-center space-x-2 shadow-lg hover:shadow-xl">
                                <span>保存本週登記</span>
                                <svg class="w-5 h-5" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round"
                                        stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3">
                                    </path>
                                </svg>
                            </button>
                        </div>
                    </div>
                    <!-- Weekly Summary Mobile -->
                    <div class="lg:hidden fixed bottom-0 left-0 right-0 z-40 bg-white border-t-2 border-gray-200 shadow-2xl transition-all duration-300"
                        id="mobileSummary">
                        <div id="summaryCollapsed" class="p-4 cursor-pointer"
                            onclick="toggleMobileSummary()">
                            <div class="flex items-center justify-between">
                                <div class="flex items-center space-x-3">
                                    <div
                                        class="w-10 h-10 bg-primary rounded-full flex items-center justify-center">
                                        <svg class="w-5 h-5 text-white" fill="none"
                                            stroke="currentColor" viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round"
                                                stroke-width="2"
                                                d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4">
                                            </path>
                                        </svg>
                                    </div>
                                    <div>
                                        <p class="font-bold text-gray-800">本週概況</p>
                                        <p class="text-sm text-gray-600" id="summaryCount">0/5 已登記</p>
                                    </div>
                                </div>
                                <svg class="w-6 h-6 text-gray-400 transform transition-transform duration-300"
                                    id="expandIcon" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round"
                                        stroke-width="2" d="M5 15l7-7 7 7">
                                    </path>
                                </svg>
                            </div>
                        </div>
                        <div id="summaryExpanded" class="hidden max-h-96 overflow-y-auto">
                            <div class="p-4 border-t border-gray-200">
                                <div class="flex items-center justify-between mb-4 cursor-pointer"
                                    onclick="toggleMobileSummary()">
                                    <h3 class="text-lg font-bold text-gray-800">本週登記概況</h3>
                                    <svg class="w-6 h-6 text-gray-400" fill="none" stroke="currentColor"
                                        viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round"
                                            stroke-width="2" d="M19 9l-7 7-7-7">
                                        </path>
                                    </svg>
                                </div>
                                <div id="weeklySummaryMobile" class="space-y-3 mb-4"></div>
                                <button type="button" onclick="saveAllOrders()"
                                    class="w-full bg-primary hover:bg-primary-dark text-white font-semibold py-3 px-6 rounded-xl transition-all duration-300 flex items-center justify-center space-x-2 shadow-lg">
                                    <span>查看概況 &amp; 保存</span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </div>

        <!-- Report Sections -->
        <div id="sectionReport1" class="content-section">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="report-frame">
                    <div class="p-4 border-b flex items-center gap-2">
                        <span>📊</span>
                        <h3 class="font-bold text-gray-800">報表 1</h3>
                    </div>
                    <iframe id="iframeReport1" src="about:blank"></iframe>
                </div>
            </div>
        </div>
        <div id="sectionReport2" class="content-section">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="report-frame">
                    <div class="p-4 border-b flex items-center gap-2">
                        <span>📈</span>
                        <h3 class="font-bold text-gray-800">報表 2</h3>
                    </div>
                    <iframe id="iframeReport2" src="about:blank"></iframe>
                </div>
            </div>
        </div>
        <div id="sectionReport3" class="content-section">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="report-frame">
                    <div class="p-4 border-b flex items-center gap-2">
                        <span>👤</span>
                        <h3 class="font-bold text-gray-800">沒有訂餐的員工</h3>
                    </div>
                    <iframe id="iframeReport3" src="about:blank"></iframe>
                </div>
            </div>
        </div>
        <div id="sectionReport4" class="content-section">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="report-frame">
                    <div class="p-4 border-b flex items-center gap-2">
                        <span>🚫</span>
                        <h3 class="font-bold text-gray-800">不訂餐的員工</h3>
                    </div>
                    <iframe id="iframeReport4" src="about:blank"></iframe>
                </div>
            </div>
        </div>
        <div id="sectionReport5" class="content-section">
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="report-frame">
                    <div class="p-4 border-b flex items-center gap-2">
                        <span>🏢</span>
                        <h3 class="font-bold text-gray-800">部門訂餐統計</h3>
                    </div>
                    <iframe id="iframeReport5" src="about:blank"></iframe>
                </div>
            </div>
        </div>
        <!-- Notice Settings Modal -->
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

    <!-- Toast -->
    <div id="toast"
        class="fixed bottom-4 right-4 transform translate-y-20 opacity-0 transition-all duration-300 z-50">
        <div class="bg-success text-white px-6 py-3 rounded-lg shadow-lg flex items-center space-x-2">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
            </svg>
            <span id="toastMessage">操作成功！</span>
        </div>
    </div>
    <!-- Loading -->
    <div id="loadingOverlay"
        class="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50 hidden">
        <div class="bg-white rounded-2xl p-8 shadow-2xl flex flex-col items-center">
            <div class="w-12 h-12 border-4 border-primary border-t-transparent rounded-full animate-spin"></div>
            <p class="mt-4 text-gray-600">處理中...</p>
        </div>
    </div>
    <!-- Meal Detail Modal -->
    <div id="mealModal" class="modal-overlay" onclick="closeModal(event)">
        <div class="modal-content" onclick="event.stopPropagation()">
            <div class="sticky top-0 bg-white border-b px-6 py-4 flex items-center justify-between rounded-t-2xl">
                <div class="flex items-center space-x-3">
                    <div class="text-4xl" id="modalIcon">🍱</div>
                    <div>
                        <h2 class="text-xl font-bold text-gray-800" id="modalTitle">餐點</h2>
                        <p class="text-sm text-gray-500" id="modalSubtitle">Meal</p>
                    </div>
                </div>
                <button type="button" onclick="closeModal()"
                    class="w-10 h-10 flex items-center justify-center rounded-full hover:bg-gray-100">
                    <svg
                        class="w-6 h-6 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                            d="M6 18L18 6M6 6l12 12">
                        </path>
                    </svg></button>
            </div>
            <div class="p-6">
                <div class="img-placeholder rounded-xl overflow-hidden mb-4" style="height: 200px" id="modalImage">
                    🍱
                </div>
                <div class="bg-primary-50 rounded-lg p-4 mb-4">
                    <div class="flex items-center space-x-2 text-primary-700">
                        <svg class="w-5 h-5" fill="none"
                            stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z">
                            </path>
                        </svg><span class="font-medium" id="modalDate">日期</span>
                    </div>
                </div>
                <div class="mb-6">
                    <h3 class="text-sm font-semibold text-gray-700 mb-2">餐點說明</h3>
                    <p class="text-gray-600 leading-relaxed whitespace-pre-line" id="modalDescription"></p>
                </div>
                <div class="flex space-x-3" id="modalActions"></div>
            </div>
        </div>
    </div>

    <script>
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

        // === Global state (same as index.aspx) ===
        let currentWeekOffset = 0, selectedDateIndex = 0, weekDates = [], mealsData = {}, ordersData = {};
        let mealTypes = [
            { id: 'buffet', name: '本日自助餐', nameEn: 'Buffet', type: '自助餐', icon: '🍱', desc: '' },
            { id: 'simple', name: '精選簡餐', nameEn: 'Set Meal', type: '簡餐', icon: '🍛', desc: '' },
            { id: 'noodles', name: '精選麵食', nameEn: 'Noodles', type: '麵食', icon: '🍜', desc: '' },
            { id: 'light', name: '輕食餐', nameEn: 'Light Meal', type: '輕食餐', icon: '🥗', desc: '' },
            { id: 'vegetarian', name: '健康素食', nameEn: 'Vegetarian', type: '素食', icon: '🥬', desc: '素食套餐 (全素/蛋奶素)' },
            { id: 'none', name: '今日不訂餐', nameEn: 'Skip', type: '不訂餐', icon: '🚫', desc: '登記不訂餐' }
        ];

        document.addEventListener('DOMContentLoaded', function () {
            initializeWeekDates(); loadInitialData();
        });

        // === Sidebar ===
        function toggleSidebar() { document.body.classList.toggle('sidebar-open'); }
        function closeSidebar() { document.body.classList.remove('sidebar-open'); }

        // === Section switching ===
        var reportUrls = { report1: 'report1.aspx', report2: 'report2.aspx', report3: 'report3.aspx', report4: 'report4.aspx', report5: 'report5.aspx' };
        var loadedReports = {};
        function showSection(name, btn) {
            document.querySelectorAll('.content-section').forEach(s => s.classList.remove('active'));
            document.querySelectorAll('.sidebar-nav button').forEach(b => b.classList.remove('active'));
            if (btn) btn.classList.add('active');
            if (name === 'menu') { document.getElementById('sectionMenu').classList.add('active'); }
            else {
                var capName = name.charAt(0).toUpperCase() + name.slice(1);
                var sec = document.getElementById('section' + capName);
                if (sec) { sec.classList.add('active'); if (!loadedReports[name] && reportUrls[name]) { document.getElementById('iframe' + capName).src = reportUrls[name]; loadedReports[name] = true; } }
            }
            if (window.innerWidth < 1024) closeSidebar();
        }

        // === Admin actions ===
        function openMenuEditWindow() {
            // Only set default date if not already populated by Page_Load
            var menuDateEl = document.getElementById('menuDate');
            if (!menuDateEl.value) {
                var d = new Date(); d.setDate(d.getDate() + 4);
                menuDateEl.value = formatDate(d);
                document.getElementById('menuType').value = '自助餐';
            }
            document.getElementById('menuEditModal').classList.add('active');
            // Refresh data from DB for current date/type
            fetchMenuData();
        }
        function closeMenuEditModal() { document.getElementById('menuEditModal').classList.remove('active'); }

        // Fetch existing menu data from DB when date/type changes
        function fetchMenuData() {
            var dateVal = document.getElementById('menuDate').value;
            var typeVal = document.getElementById('menuType').value;
            if (!dateVal) return;
            PageMethods.GetMenuData(dateVal, typeVal, function (result) {
                if (result && result.success) {
                    document.getElementById('menuDesc').value = result.lunchName || '';
                }
            }, function (err) {
                console.error('GetMenuData error:', err);
            });
        }

        // Wire up change events
        document.addEventListener('DOMContentLoaded', function () {
            var menuDateEl = document.getElementById('menuDate');
            var menuTypeEl = document.getElementById('menuType');
            if (menuDateEl) menuDateEl.addEventListener('change', fetchMenuData);
            if (menuTypeEl) menuTypeEl.addEventListener('change', fetchMenuData);
        });

        function submitMenuEdit() {
            var dateVal = document.getElementById('menuDate').value;
            var typeVal = document.getElementById('menuType').value;
            var descVal = document.getElementById('menuDesc').value;
            if (!dateVal) { alert('請選擇日期'); return; }
            // Sync values to hidden FineUI controls then trigger save
            var fDpdate = F && F('<%= dpdate.ClientID %>'); if (fDpdate) fDpdate.setValue(dateVal);
                var fDdltype = F && F('<%= ddltype.ClientID %>'); if (fDdltype) fDdltype.setValue(typeVal);
                var fTaname = F && F('<%= taname.ClientID %>'); if (fTaname) fTaname.setValue(descVal);
        // Trigger server-side save
        document.querySelector('[id$="btnSave"]').click();
        closeMenuEditModal();
    }

    function doSendNextWeek() { document.querySelector('[id$="btnSend"]').click(); closeSidebar(); }

    function showDateSendUI() {
        document.getElementById('dateSendModal').classList.add('active');
        closeSidebar();
    }
    function closeDateSendModal() { document.getElementById('dateSendModal').classList.remove('active'); }
    function submitDateSend() {
        var startVal = document.getElementById('sendStartDate').value;
        var endVal = document.getElementById('sendEndDate').value;
        if (!startVal || !endVal) { alert('請選擇起始和結束日期'); return; }
        if (endVal < startVal) { alert('結束日期應大於開始日期'); return; }
        // Sync to hidden FineUI DatePickers then trigger send
        var fDp2 = F && F('<%= DatePicker2.ClientID %>'); if (fDp2) fDp2.setValue(startVal);
                var fDp3 = F && F('<%= DatePicker3.ClientID %>'); if (fDp3) fDp3.setValue(endVal);
                document.querySelector('[id$="btnDateSend"]').click();
                closeDateSendModal();
            }

            // === Date/Week logic (identical to index.aspx) ===
            function changeWeek(dir) { currentWeekOffset += dir; initializeWeekDates(); selectedDateIndex = 0; loadWeekData(); }
            function formatDate(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
            function normalizeDate(date) {
                var normalized = new Date(date);
                normalized.setHours(0, 0, 0, 0);
                return normalized;
            }
            function getLatestOrderDate() {
                var latest = normalizeDate(new Date());
                var workdayCount = 0;
                while (workdayCount < 2) {
                    latest.setDate(latest.getDate() + 1);
                    var dayOfWeek = latest.getDay();
                    if (dayOfWeek === 0 || dayOfWeek === 6) continue;
                    workdayCount++;
                }
                return latest;
            }
            function isDateOrderable(date) {
                var now = new Date();
                var today = normalizeDate(now);
                var selected = normalizeDate(date);

                var dayOfWeek = selected.getDay();
                if (dayOfWeek === 0 || dayOfWeek === 6) return false;

                if (selected < today) return false;

                if (selected.getTime() === today.getTime()) {
                    var todayCutoff = new Date(today);
                    todayCutoff.setHours(9, 0, 0, 0);
                    return now < todayCutoff;
                }

                return selected <= getLatestOrderDate();
            }
            function initializeWeekDates() {
                var today = new Date(), dow = today.getDay(), monday = new Date(today);
                monday.setDate(today.getDate() - (dow === 0 ? 6 : dow - 1) + currentWeekOffset * 7);
                weekDates = []; var dayNames = ['週一', '週二', '週三', '週四', '週五'];
                for (var i = 0; i < 5; i++) { var d = new Date(monday); d.setDate(monday.getDate() + i); weekDates.push({ date: d, dateStr: formatDate(d), display: (d.getMonth() + 1) + '月' + d.getDate() + '日 ' + dayNames[i], shortDisplay: (d.getMonth() + 1) + '/' + d.getDate(), dayName: dayNames[i] }); }
                if (currentWeekOffset === 0) { var ts = formatDate(today); selectedDateIndex = weekDates.findIndex(d => d.dateStr === ts); if (selectedDateIndex === -1) selectedDateIndex = 0; } else selectedDateIndex = 0;
                renderDateTabs();
            }
            function renderDateTabs() {
                document.getElementById('dateTabs').innerHTML = weekDates.map((d, i) =>
                    `<button type="button" class="date-tab flex-shrink-0 px-4 py-2 rounded-full font-medium transition-all duration-300 ${i === selectedDateIndex ? 'active' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'}" onclick="selectDate(${i})"><span class="hidden sm:inline">${d.display}</span><span class="sm:hidden">${d.shortDisplay} ${d.dayName}</span></button>`
                ).join('');
            }
            function selectDate(i) { selectedDateIndex = i; renderDateTabs(); renderMealCards(); updateWeeklySummary(); }
            function loadInitialData() {
                var wd = document.getElementById('<%= hfWeekData.ClientID %>').value;
                var od = document.getElementById('<%= hfOrderData.ClientID %>').value;
          try { if (wd) mealsData = JSON.parse(wd); if (od) ordersData = JSON.parse(od); } catch (e) { }
          renderMealCards(); updateWeeklySummary();
      }
      function loadWeekData() {
          showLoading(true);
          PageMethods.LoadWeekData(weekDates[0].dateStr, weekDates[4].dateStr, function (r) {
              showLoading(false);
              if (r.success) { try { if (r.mealsData) mealsData = JSON.parse(r.mealsData); if (r.ordersData) ordersData = JSON.parse(r.ordersData); } catch (e) { } renderMealCards(); updateWeeklySummary(); }
              else showToast(r.message || '載入失敗', false);
          }, function (e) { showLoading(false); showToast('發生錯誤', false); });
      }

      // === Render Meal Cards (same as index.aspx + quantity stepper) ===
      function renderMealCards() {
          var container = document.getElementById('mealCards');
          var dateStr = weekDates[selectedDateIndex].dateStr;
          var dayMeals = mealsData[dateStr] || {}, dayOrder = ordersData[dateStr] || { lunchId: null, type: null };
          var canOrder = isDateOrderable(weekDates[selectedDateIndex].date);
          var html = '';
          mealTypes.forEach(meal => {
              var info = dayMeals[meal.type] || {};
              var avail = info.available !== false && (meal.type === '不訂餐' || meal.type === '素食' || info.name);
              var isSel = dayOrder.type === meal.type || (meal.type === '不訂餐' && dayOrder.lunchId === '0' && dayOrder.type === '不訂餐');
              var mealName = info.name || meal.desc || '';
              var clickable = avail && canOrder;
              var orderNum = parseInt(dayOrder.orderNum) || 1;
              if (meal.type === '不訂餐' || meal.type === '素食' || info.name) {
                  // Quantity stepper HTML (admin feature, not on 不訂餐)
                  var qtyHtml = '';
                  if (meal.type !== '不訂餐' && isSel) {
                      qtyHtml = `<div class="flex items-center justify-between mt-3 pt-3 border-t border-gray-100"><span class="text-xs text-gray-500 font-medium">訂餐份數</span><div class="qty-stepper"><button type="button" onclick="event.stopPropagation();changeQty(-1)">−</button><span class="qty-val" id="qtyVal">${orderNum}</span><button type="button" onclick="event.stopPropagation();changeQty(1)">+</button></div></div>`;
                  }
                  html += `<div class="card-hover bg-white rounded-2xl shadow-md overflow-hidden border-2 transition-all duration-300 cursor-pointer ${isSel ? 'meal-selected border-success' : 'border-transparent'} ${!canOrder ? 'opacity-60' : ''}" onclick="openMealModal('${meal.type}','${info.id || '0'}',${clickable})">
                        <div class="img-placeholder h-32 sm:h-40 text-4xl">${meal.icon}</div>
                        <div class="p-4"><div class="flex items-start justify-between"><div class="flex-1"><h3 class="font-bold text-gray-800">${meal.name}</h3><p class="text-sm text-gray-500">${meal.nameEn}</p></div>${isSel ? '<div class="w-6 h-6 bg-success rounded-full flex items-center justify-center"><svg class="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path></svg></div>' : ''}</div>
                        <p class="mt-2 text-sm text-gray-600 line-clamp-2">${mealName || meal.desc}</p>
                        ${!canOrder ? '<div class="mt-2 text-xs text-orange-600 flex items-center"><svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path></svg>僅供查看</div>' : ''}
                        ${qtyHtml}
                        <button type="button" class="select-btn w-full mt-3 py-2 px-4 rounded-lg font-medium transition-all duration-300 ${isSel ? 'bg-success text-white' : !canOrder ? 'bg-gray-200 text-gray-500 cursor-not-allowed' : 'bg-primary-50 text-primary hover:bg-primary hover:text-white'}" onclick="event.stopPropagation();${clickable && canOrder ? `selectMeal('${meal.type}','${info.id || '0'}')` : ''}" ${!clickable ? 'disabled' : ''}>${!canOrder ? '無法訂餐' : isSel ? '✓ 已選擇' : meal.type === '不訂餐' ? '登記不訂餐' : '選擇此餐點'}</button></div></div>`;
              }
          });
          container.innerHTML = html || '<div class="col-span-full text-center py-12 text-gray-500">今日無可選餐點</div>';
      }

      // === Quantity change ===
      function changeQty(delta) {
          var dateStr = weekDates[selectedDateIndex].dateStr;
          var order = ordersData[dateStr]; if (!order) return;
          var cur = parseInt(order.orderNum) || 1;
          var nv = Math.max(1, cur + delta);
          order.orderNum = String(nv);
          renderMealCards(); updateWeeklySummary();
      }

      // === Select meal (same as index.aspx, with orderNum) ===
      function selectMeal(mealType, lunchId) {

          var dateStr = weekDates[selectedDateIndex].dateStr;
          if (!isDateOrderable(weekDates[selectedDateIndex].date)) return;

          if (mealType === '不訂餐') ordersData[dateStr] = { lunchId: '0', type: mealType, orderNum: '0' };

          else if (mealType === '素食') ordersData[dateStr] = { lunchId: '0', type: mealType, orderNum: '1' };

          else ordersData[dateStr] = { lunchId: lunchId, type: mealType, orderNum: '1' };

          renderMealCards(); updateWeeklySummary();

      }

      // === Weekly Summary (same as index.aspx + shows qty) ===
      function updateWeeklySummary() {
          var desktopEl = document.getElementById('weeklySummaryDesktop'), mobileEl = document.getElementById('weeklySummaryMobile'), countEl = document.getElementById('summaryCount');
          var ordered = 0;
          var html = weekDates.map((d, i) => {
              var order = ordersData[d.dateStr]; var st = '未選擇', sc = 'text-gray-400', ic = '<span class="text-gray-400">○</span>';
              if (order && (order.orderNum !== '0' || order.type === '不訂餐')) {
                  ordered++;
                  if (order.type === '不訂餐') { st = '不訂餐'; sc = 'text-gray-500'; ic = '<span class="text-gray-500">✗</span>'; }
                  else { var q = parseInt(order.orderNum) || 1; st = order.type + (q > 1 ? ' ×' + q : ''); sc = 'text-success font-medium'; ic = '<span class="text-success">✓</span>'; }
              }
              return `<div class="flex items-center justify-between py-2 ${i === selectedDateIndex ? 'bg-primary-50 -mx-2 px-2 rounded-lg' : ''}"><div class="flex items-center space-x-2">${ic}<span class="text-gray-700">${d.display}</span></div><span class="${sc}">${st}</span></div>`;
          }).join('');
          if (desktopEl) desktopEl.innerHTML = html; if (mobileEl) mobileEl.innerHTML = html;
          if (countEl) countEl.textContent = ordered + '/5 已登記';
      }

      // === Save (same as index.aspx) ===
      function saveAllOrders() {
          showLoading(true);
          PageMethods.SaveWeekOrders(JSON.stringify(ordersData), function (r) {
              showLoading(false);
              if (r.success) showToast('訂餐成功！'); else showToast(r.message || '儲存失敗', false);
          }, function (e) { showLoading(false); showToast('發生錯誤', false); });
      }

      // === Mobile summary, loading, toast, modal (same as index.aspx) ===
      function toggleMobileSummary() { var c = document.getElementById('summaryCollapsed'), e = document.getElementById('summaryExpanded'); if (e.classList.contains('hidden')) { c.classList.add('hidden'); e.classList.remove('hidden'); } else { e.classList.add('hidden'); c.classList.remove('hidden'); } }
      function showLoading(show) { document.getElementById('loadingOverlay').classList.toggle('hidden', !show); }
      function showToast(msg, ok = true) { var t = document.getElementById('toast'), m = document.getElementById('toastMessage'); m.textContent = msg; t.querySelector('div').className = (ok ? 'bg-success' : 'bg-red-500') + ' text-white px-6 py-3 rounded-lg shadow-lg flex items-center space-x-2'; t.classList.remove('translate-y-20', 'opacity-0'); t.classList.add('translate-y-0', 'opacity-100'); setTimeout(() => { t.classList.add('translate-y-20', 'opacity-0'); t.classList.remove('translate-y-0', 'opacity-100'); }, 3000); }
      function openMealModal(mealType, lunchId, clickable) {
          var dateStr = weekDates[selectedDateIndex].dateStr, dayMeals = mealsData[dateStr] || {}, dayOrder = ordersData[dateStr] || {};

          var canOrder = isDateOrderable(weekDates[selectedDateIndex].date);
          var meal = mealTypes.find(m => m.type === mealType), info = dayMeals[mealType] || {};
          var isSel = dayOrder.type === mealType, mealName = info.name || meal.desc || '';
          document.getElementById('modalIcon').textContent = meal.icon;
          document.getElementById('modalTitle').textContent = meal.name;
          document.getElementById('modalSubtitle').textContent = meal.nameEn;
          document.getElementById('modalImage').innerHTML = `<div class="text-6xl h-full flex items-center justify-center">${meal.icon}</div>`;
          document.getElementById('modalDate').textContent = weekDates[selectedDateIndex].display;
          document.getElementById('modalDescription').innerHTML = mealName || '本餐點為' + meal.name;
          document.getElementById('modalActions').innerHTML = `${clickable && canOrder ? `<button type="button" onclick="selectMealFromModal('${mealType}','${lunchId}')" class="flex-1 py-3 px-6 rounded-xl font-semibold transition-all duration-300 ${isSel ? 'bg-success' : 'bg-primary hover:bg-primary-dark'} text-white shadow-lg">${isSel ? '✓ 已選擇' : mealType === '不訂餐' ? '登記不訂餐' : '選擇此餐點'}</button>` : '<div class="flex-1 py-3 px-6 rounded-xl bg-gray-200 text-gray-500 text-center font-semibold">無法訂餐</div>'}<button type="button" onclick="closeModal()" class="py-3 px-6 rounded-xl border-2 border-gray-300 text-gray-700 font-semibold hover:bg-gray-50">關閉</button>`;
          document.getElementById('mealModal').classList.add('active'); document.body.style.overflow = 'hidden';
      }
      function closeModal(e) { if (e && e.target.id !== 'mealModal') return; document.getElementById('mealModal').classList.remove('active'); document.body.style.overflow = ''; }
      function selectMealFromModal(t, id) { selectMeal(t, id); closeModal(); }
      document.addEventListener('keydown', e => { if (e.key === 'Escape') closeModal(); });
    </script >
</body>

</html>





