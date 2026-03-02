<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="FineUIPro.OrderMeal.index" %>

    <!DOCTYPE html>
    <html lang="zh-TW">

    <head runat="server">
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>企業團膳點餐系統</title>
        <script src="https://cdn.tailwindcss.com"></script>
        <script>
            tailwind.config = {
                theme: {
                    extend: {
                        colors: {
                            primary: {
                                DEFAULT: '#e78232',
                                light: '#f5a862',
                                dark: '#c66a1f',
                                50: '#fef9f3',
                                100: '#fdf0e3',
                                200: '#fbe0c7',
                                500: '#e78232',
                                600: '#c66a1f',
                                700: '#a5571a'
                            },
                            success: {
                                DEFAULT: '#2E7D32',
                                light: '#4CAF50'
                            }
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

            /* Modal Styles */
            .modal-overlay {
                position: fixed;
                inset: 0;
                background-color: rgba(0, 0, 0, 0.5);
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
                box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
            }

            .modal-overlay.active .modal-content {
                transform: scale(1) translateY(0);
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

            /* Toggle switch */
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
                box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
            }

            .toggle-switch input:checked+.toggle-slider {
                background: #e78232;
            }

            .toggle-switch input:checked+.toggle-slider:before {
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
                box-shadow: 0 2px 8px rgba(231, 130, 50, 0.3);
            }

            .notice-btn-save:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(231, 130, 50, 0.4);
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
                background: rgba(231, 130, 50, 0.1);
            }
        </style>
    </head>

    <body class="font-sans">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

            <!-- Hidden fields for server data -->
            <asp:HiddenField ID="hfUserNo" runat="server" />
            <asp:HiddenField ID="hfUserName" runat="server" />
            <asp:HiddenField ID="hfWeekData" runat="server" />
            <asp:HiddenField ID="hfOrderData" runat="server" />

            <!-- Header -->
            <header class="bg-white shadow-sm sticky top-0 z-50">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div class="flex justify-between items-center h-16">
                        <div class="flex items-center space-x-3">
                            <div class="w-10 h-10 bg-primary rounded-lg flex items-center justify-center">
                                <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253">
                                    </path>
                                </svg>
                            </div>
                            <h1 class="text-xl font-bold text-gray-800">企業團膳點餐系統</h1>
                        </div>
                        <div class="flex items-center space-x-4">
                            <button type="button" id="noticeSettingsBtn" class="notice-bell-btn" onclick="openNoticeModal()" title="通知設定">
                                <svg class="w-5 h-5 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9">
                                    </path>
                                </svg>
                                <span class="text-gray-600">您好，<span class="font-semibold text-primary"><%=username %></span></span>
                            </button>
                            <a href="login.aspx" class="text-gray-500 hover:text-primary transition-colors">登出</a>
                        </div>
                    </div>
                </div>
            </header>

            <!-- Date Tabs -->
            <div class="bg-white border-b sticky top-16 z-40">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div class="flex items-center py-3 space-x-3">
                        <!-- Previous Week Button -->
                        <button type="button" onclick="changeWeek(-1)"
                            class="flex-shrink-0 w-10 h-10 flex items-center justify-center rounded-full bg-gray-100 text-gray-600 hover:bg-primary hover:text-white transition-all duration-300">
                            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M15 19l-7-7 7-7"></path>
                            </svg>
                        </button>

                        <!-- Date Tabs Container -->
                        <div class="flex-1 overflow-x-auto scrollbar-hide">
                            <div class="flex space-x-2" id="dateTabs">
                                <!-- Date tabs will be generated by JavaScript -->
                            </div>
                        </div>

                        <!-- Next Week Button -->
                        <button type="button" onclick="changeWeek(1)"
                            class="flex-shrink-0 w-10 h-10 flex items-center justify-center rounded-full bg-gray-100 text-gray-600 hover:bg-primary hover:text-white transition-all duration-300">
                            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7">
                                </path>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>

            <!-- Alert Message -->
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-4">
                <div class="bg-primary-50 border-l-4 border-primary p-4 rounded-r-lg">
                    <div class="flex">
                        <div class="flex-shrink-0">
                            <svg class="h-5 w-5 text-primary" viewBox="0 0 20 20" fill="currentColor">
                                <path fill-rule="evenodd"
                                    d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z"
                                    clip-rule="evenodd" />
                            </svg>
                        </div>
                        <div class="ml-3">
                            <p class="text-sm text-primary-700">
                                點餐需於<strong>前一日 16:00 前</strong>完成選訂，取消訂餐可於<strong>當天 08:40 前</strong>取消。請節省資源，避免餐食浪費！
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Main Content -->
            <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6 pb-96 lg:pb-6">
                <div class="lg:flex lg:space-x-6">
                    <!-- Meal Cards Section -->
                    <div class="lg:flex-1">
                        <div class="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-4" id="mealCards">
                            <!-- Meal cards will be generated by JavaScript -->
                        </div>
                    </div>

                    <!-- Weekly Summary Sidebar - Desktop -->
                    <div class="hidden lg:block lg:w-80 lg:mt-0">
                        <div class="bg-white rounded-2xl shadow-lg p-6 sticky top-36">
                            <h3 class="text-lg font-bold text-gray-800 mb-4 flex items-center">
                                <svg class="w-5 h-5 mr-2 text-primary" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4">
                                    </path>
                                </svg>
                                本週登記概況
                            </h3>
                            <div id="weeklySummaryDesktop" class="space-y-3">
                                <!-- Summary items will be generated by JavaScript -->
                            </div>

                            <!-- Save Button -->
                            <button type="button" onclick="saveAllOrders()"
                                class="w-full mt-6 bg-primary hover:bg-primary-dark text-white font-semibold py-3 px-6 rounded-xl transition-all duration-300 flex items-center justify-center space-x-2 shadow-lg hover:shadow-xl">
                                <span>保存本週登記</span>
                                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M14 5l7 7m0 0l-7 7m7-7H3"></path>
                                </svg>
                            </button>
                        </div>
                    </div>

                    <!-- Weekly Summary - Mobile (Bottom Fixed) -->
                    <div class="lg:hidden fixed bottom-0 left-0 right-0 z-40 bg-white border-t-2 border-gray-200 shadow-2xl transition-all duration-300"
                        id="mobileSummary">
                        <!-- Collapsed View (Default) -->
                        <div id="summaryCollapsed" class="p-4 cursor-pointer" onclick="toggleMobileSummary()">
                            <div class="flex items-center justify-between">
                                <div class="flex items-center space-x-3">
                                    <div class="w-10 h-10 bg-primary rounded-full flex items-center justify-center">
                                        <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor"
                                            viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
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
                                    id="expandIcon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M5 15l7-7 7 7"></path>
                                </svg>
                            </div>
                        </div>

                        <!-- Expanded View -->
                        <div id="summaryExpanded" class="hidden max-h-96 overflow-y-auto">
                            <div class="p-4 border-t border-gray-200">
                                <div class="flex items-center justify-between mb-4 cursor-pointer"
                                    onclick="toggleMobileSummary()">
                                    <h3 class="text-lg font-bold text-gray-800 flex items-center">
                                        <svg class="w-5 h-5 mr-2 text-primary" fill="none" stroke="currentColor"
                                            viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                                d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4">
                                            </path>
                                        </svg>
                                        本週登記概況
                                    </h3>
                                    <svg class="w-6 h-6 text-gray-400" fill="none" stroke="currentColor"
                                        viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                            d="M19 9l-7 7-7-7"></path>
                                    </svg>
                                </div>
                                <div id="weeklySummaryMobile" class="space-y-3 mb-4">
                                    <!-- Summary items will be generated by JavaScript -->
                                </div>

                                <!-- Save Button -->
                                <button type="button" onclick="saveAllOrders()"
                                    class="w-full bg-primary hover:bg-primary-dark text-white font-semibold py-3 px-6 rounded-xl transition-all duration-300 flex items-center justify-center space-x-2 shadow-lg">
                                    <span>查看概況 &amp; 保存</span>
                                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                            d="M14 5l7 7m0 0l-7 7m7-7H3"></path>
                                    </svg>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </main>

            <!-- Toast Notification -->
            <div id="toast"
                class="fixed bottom-4 right-4 transform translate-y-20 opacity-0 transition-all duration-300 z-50">
                <div class="bg-success text-white px-6 py-3 rounded-lg shadow-lg flex items-center space-x-2">
                    <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
                    </svg>
                    <span id="toastMessage">操作成功！</span>
                </div>
            </div>

            <!-- Loading Overlay -->
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
                    <!-- Modal Header -->
                    <div
                        class="sticky top-0 bg-white border-b px-6 py-4 flex items-center justify-between rounded-t-2xl">
                        <div class="flex items-center space-x-3">
                            <div class="text-4xl" id="modalIcon">🍱</div>
                            <div>
                                <h2 class="text-xl font-bold text-gray-800" id="modalTitle">餐點名稱</h2>
                                <p class="text-sm text-gray-500" id="modalSubtitle">Meal Name</p>
                            </div>
                        </div>
                        <button type="button" onclick="closeModal()"
                            class="w-10 h-10 flex items-center justify-center rounded-full hover:bg-gray-100 transition-colors">
                            <svg class="w-6 h-6 text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M6 18L18 6M6 6l12 12"></path>
                            </svg>
                        </button>
                    </div>

                    <!-- Modal Body -->
                    <div class="p-6">
                        <!-- Image -->
                        <div class="img-placeholder rounded-xl overflow-hidden mb-4" style="height: 240px;"
                            id="modalImage">
                            🍱
                        </div>

                        <!-- Date Info -->
                        <div class="bg-primary-50 rounded-lg p-4 mb-4">
                            <div class="flex items-center space-x-2 text-primary-700">
                                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z">
                                    </path>
                                </svg>
                                <span class="font-medium" id="modalDate">日期</span>
                            </div>
                        </div>

                        <!-- Description -->
                        <div class="mb-6">
                            <h3 class="text-sm font-semibold text-gray-700 mb-2">餐點說明</h3>
                            <p class="text-gray-600 leading-relaxed whitespace-pre-line" id="modalDescription">
                            </p>
                        </div>

                        <!-- Action Buttons -->
                        <div class="flex space-x-3" id="modalActions">
                            <!-- Buttons will be generated by JavaScript -->
                        </div>
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
                        <!-- Mail Notice Item -->
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
                                <input type="checkbox" id="toggleMail"
                                    onchange="updateNoticeItemStyle('Mail', this.checked)">
                                <span class="toggle-slider"></span>
                            </label>
                        </div>
                        <!-- DingTalk Notice Item -->
                        <div class="notice-item" id="noticeItemDingTalk">
                            <div class="notice-item-info">
                                <div class="notice-item-icon dingtalk">🔔</div>
                                <div class="notice-item-text">
                                    <h4>釘釘通知</h4>
                                    <p>接收釘釘訂餐提醒及菜單推送</p>
                                </div>
                            </div>
                            <label class="toggle-switch">
                                <input type="checkbox" id="toggleDingTalk"
                                    onchange="updateNoticeItemStyle('DingTalk', this.checked)">
                                <span class="toggle-slider"></span>
                            </label>
                        </div>
                    </div>
                    <div class="notice-footer">
                        <button type="button" class="notice-btn notice-btn-cancel"
                            onclick="closeNoticeModal()">取消</button>
                        <button type="button" class="notice-btn notice-btn-save" onclick="saveNoticeSettings()">💾
                            儲存設定</button>
                    </div>
                </div>
            </div>
        </form>

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
                if (el) {
                    if (checked) el.classList.add('enabled');
                    else el.classList.remove('enabled');
                }
            }

            function loadNoticeSettings() {
                PageMethods.GetNoticeSettings(function (result) {
                    if (result && result.success) {
                        var mailEnabled = result.mailEnabled;
                        var dingTalkEnabled = result.dingTalkEnabled;
                        document.getElementById('toggleMail').checked = mailEnabled;
                        document.getElementById('toggleDingTalk').checked = dingTalkEnabled;
                        updateNoticeItemStyle('Mail', mailEnabled);
                        updateNoticeItemStyle('DingTalk', dingTalkEnabled);
                    }
                }, function (err) {
                    console.error('LoadNoticeSettings error:', err);
                });
            }

            function saveNoticeSettings() {
                var mailEnabled = document.getElementById('toggleMail').checked ? 1 : 0;
                var dingTalkEnabled = document.getElementById('toggleDingTalk').checked ? 1 : 0;
                PageMethods.SaveNoticeSettings(mailEnabled, dingTalkEnabled, function (result) {
                    if (result && result.success) {
                        showToast('通知設定已儲存！');
                        closeNoticeModal();
                    } else {
                        showToast(result.message || '儲存失敗', false);
                    }
                }, function (err) {
                    showToast('發生錯誤，請稍後再試', false);
                    console.error(err);
                });
            }

            // Global state
            let currentDate = new Date();
            let currentWeekOffset = 0;
            let selectedDateIndex = 0;
            let weekDates = [];
            let mealsData = {};
            let ordersData = {};
            const orderWindowConfig = <%= orderWindowConfigJson %>;
            let mealTypes = [
                { id: 'buffet', name: '本日自助餐', nameEn: 'Buffet', type: '自助餐', icon: '🍱', desc: '' },
                { id: 'simple', name: '精選簡餐', nameEn: 'Set Meal', type: '簡餐', icon: '🍛', desc: '' },
                { id: 'noodles', name: '精選麵食', nameEn: 'Noodles', type: '麵食', icon: '🍜', desc: '' },
                { id: 'light', name: '輕食餐', nameEn: 'Light Meal', type: '輕食餐', icon: '🥗', desc: '' },
                { id: 'vegetarian', name: '健康素食', nameEn: 'Vegetarian', type: '素食', icon: '🥬', desc: '素食套餐 (全素/蛋奶素)' },
                { id: 'none', name: '今日不訂餐', nameEn: 'Skip', type: '不訂餐', icon: '🚫', desc: '登記不訂餐' }
            ];

            // Initialize on page load
            document.addEventListener('DOMContentLoaded', function () {
                initializeWeekDates();
                loadInitialData();
            });

            function changeWeek(direction) {
                currentWeekOffset += direction;
                initializeWeekDates();

                // Reset selection to first day of the new week
                selectedDateIndex = 0;

                // Load data for the new week
                loadWeekData();
            }

            function loadWeekData() {
                // Get the date range for the current week
                const startDate = weekDates[0].dateStr;
                const endDate = weekDates[4].dateStr;

                showLoading(true);

                // Call server-side method to load meals and orders for the new week
                PageMethods.LoadWeekData(startDate, endDate, function (result) {
                    showLoading(false);
                    if (result.success) {
                        try {
                            if (result.mealsData) mealsData = JSON.parse(result.mealsData);
                            if (result.ordersData) ordersData = JSON.parse(result.ordersData);
                        } catch (e) {
                            console.error('Error parsing week data:', e);
                        }

                        renderMealCards();
                        updateWeeklySummary();
                    } else {
                        showToast(result.message || '載入失敗', false);
                    }
                }, function (error) {
                    showLoading(false);
                    showToast('發生錯誤，請稍後再試', false);
                    console.error(error);
                });
            }

            function initializeWeekDates() {
                const today = new Date();

                // Calculate the Monday of the target week based on offset
                const dayOfWeek = today.getDay();
                const monday = new Date(today);
                monday.setDate(today.getDate() - (dayOfWeek === 0 ? 6 : dayOfWeek - 1) + (currentWeekOffset * 7));

                weekDates = [];
                const dayNames = ['週一', '週二', '週三', '週四', '週五'];

                for (let i = 0; i < 5; i++) {
                    const date = new Date(monday);
                    date.setDate(monday.getDate() + i);
                    weekDates.push({
                        date: date,
                        dateStr: formatDate(date),
                        display: `${date.getMonth() + 1}月${date.getDate()}日 ${dayNames[i]}`,
                        shortDisplay: `${date.getMonth() + 1}/${date.getDate()}`,
                        dayName: dayNames[i]
                    });
                }

                // If viewing current week, find today or next available day
                if (currentWeekOffset === 0) {
                    const todayStr = formatDate(today);
                    selectedDateIndex = weekDates.findIndex(d => d.dateStr === todayStr);
                    if (selectedDateIndex === -1) selectedDateIndex = 0;
                } else {
                    selectedDateIndex = 0;
                }

                renderDateTabs();
            }


            function formatDate(date) {
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                return `${year}-${month}-${day}`;
            }

            function normalizeDate(date) {
                const normalized = new Date(date);
                normalized.setHours(0, 0, 0, 0);
                return normalized;
            }

            function getEarliestOrderDate(now = new Date()) {
                const earliest = normalizeDate(now);
                const todayCutoff = new Date(earliest);
                todayCutoff.setHours(9, 0, 0, 0);

                if (now >= todayCutoff) {
                    earliest.setDate(earliest.getDate() + 1);
                }

                return earliest;
            }

            function getSpecialOrderEndDate(now = new Date()) {
                if (!orderWindowConfig.enableSpecialOrderWindow) return null;

                const specialStart = normalizeDate(orderWindowConfig.specialOrderWindowStartDate);
                const specialEnd = normalizeDate(orderWindowConfig.specialOrderWindowEndDate);
                if (normalizeDate(now) < specialStart) return null;

                const earliest = getEarliestOrderDate(now);
                if (earliest > specialEnd) return null;

                return specialEnd;
            }

            function getLatestOrderDate(now = new Date()) {
                const specialEnd = getSpecialOrderEndDate(now);
                if (specialEnd) return specialEnd;

                const latest = normalizeDate(now);
                let workdayCount = 0;

                while (workdayCount < 2) {
                    latest.setDate(latest.getDate() + 1);
                    const dayOfWeek = latest.getDay();
                    if (dayOfWeek === 0 || dayOfWeek === 6) {
                        continue;
                    }
                    workdayCount++;
                }

                return latest;
            }

            function isDateOrderable(date) {
                const now = new Date();
                const earliest = getEarliestOrderDate(now);
                const selected = normalizeDate(date);
                if (selected < earliest) return false;

                const specialEnd = getSpecialOrderEndDate(now);
                if (specialEnd) return selected <= specialEnd;

                const dayOfWeek = selected.getDay();
                if (dayOfWeek === 0 || dayOfWeek === 6) return false;

                return selected <= getLatestOrderDate(now);
            }

            function renderDateTabs() {
                const container = document.getElementById('dateTabs');
                container.innerHTML = weekDates.map((d, index) => `
                <button type="button" 
                    class="date-tab flex-shrink-0 px-4 py-2 rounded-full font-medium transition-all duration-300 ${index === selectedDateIndex ? 'active' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'}"
                    onclick="selectDate(${index})">
                    <span class="hidden sm:inline">${d.display}</span>
                    <span class="sm:hidden">${d.shortDisplay} ${d.dayName}</span>
                </button>
            `).join('');
            }

            function selectDate(index) {
                selectedDateIndex = index;
                renderDateTabs();
                renderMealCards();
                updateWeeklySummary();
            }

            function loadInitialData() {
                // Get data from hidden fields (populated by server)
                const weekDataStr = document.getElementById('<%= hfWeekData.ClientID %>').value;
                const orderDataStr = document.getElementById('<%= hfOrderData.ClientID %>').value;

                try {
                    if (weekDataStr) mealsData = JSON.parse(weekDataStr);
                    if (orderDataStr) ordersData = JSON.parse(orderDataStr);
                } catch (e) {
                    console.error('Error parsing initial data:', e);
                }

                renderMealCards();
                updateWeeklySummary();
            }

            function renderMealCards() {
                const container = document.getElementById('mealCards');
                const currentDateStr = weekDates[selectedDateIndex].dateStr;
                const dayMeals = mealsData[currentDateStr] || {};
                const dayOrder = ordersData[currentDateStr] || { lunchId: null, type: null };
                const canOrder = isDateOrderable(weekDates[selectedDateIndex].date);

                let cardsHtml = '';

                mealTypes.forEach(meal => {
                    const mealInfo = dayMeals[meal.type] || {};
                    const isAvailable = mealInfo.available !== false && (meal.type === '不訂餐' || meal.type === '素食' || mealInfo.name);
                    const isSelected = dayOrder.type === meal.type || (meal.type === '不訂餐' && dayOrder.lunchId === '0' && dayOrder.orderNum === '1');
                    const mealName = mealInfo.name || meal.desc || '';

                    const isClickable = isAvailable && canOrder;

                    if (meal.type === '不訂餐' || meal.type === '素食' || mealInfo.name) {
                        cardsHtml += `
                        <div class="card-hover bg-white rounded-2xl shadow-md overflow-hidden border-2 transition-all duration-300 cursor-pointer ${isSelected ? 'meal-selected border-success' : 'border-transparent'} ${!canOrder ? 'opacity-60' : ''}" 
                             data-meal-type="${meal.type}" 
                             data-lunch-id="${mealInfo.id || (meal.type === '素食' ? '0' : '')}"
                             onclick="openMealModal('${meal.type}', '${mealInfo.id || '0'}', ${isClickable})">
                            
                            <!-- Image Placeholder -->
                            <div class="img-placeholder h-32 sm:h-40 text-4xl">
                                ${meal.icon}
                            </div>
                            
                            <!-- Card Content -->
                            <div class="p-4">
                                <div class="flex items-start justify-between">
                                    <div class="flex-1">
                                        <h3 class="font-bold text-gray-800">${meal.name}</h3>
                                        <p class="text-sm text-gray-500">${meal.nameEn}</p>
                                    </div>
                                    ${isSelected ? `
                                        <div class="w-6 h-6 bg-success rounded-full flex items-center justify-center">
                                            <svg class="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
                                            </svg>
                                        </div>
                                    ` : ''}
                                </div>
                                
                                <p class="mt-2 text-sm text-gray-600 line-clamp-2">${mealName || meal.desc}</p>
                                
                                ${!canOrder ? `
                                    <div class="mt-2 text-xs text-orange-600 flex items-center">
                                        <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path>
                                        </svg>
                                        已截止，無法訂餐
                                    </div>
                                ` : ''}
                                
                                <button type="button" 
                                    class="select-btn w-full mt-3 py-2 px-4 rounded-lg font-medium transition-all duration-300 ${isSelected ? 'bg-success text-white' : !canOrder ? 'bg-gray-200 text-gray-500 cursor-not-allowed' : 'bg-primary-50 text-primary hover:bg-primary hover:text-white'}"
                                    onclick="event.stopPropagation(); ${isClickable ? `selectMeal('${meal.type}', '${mealInfo.id || '0'}')` : ''}"
                                    ${!isClickable ? 'disabled' : ''}>
                                    ${!canOrder ? '無法訂餐' : isSelected ? '✓ 已選擇' : (meal.type === '不訂餐' ? '登記不訂餐' : '選擇此餐點')}
                                </button>
                            </div>
                        </div>
                    `;
                    }
                });

                container.innerHTML = cardsHtml || '<div class="col-span-full text-center py-12 text-gray-500">今日無可選餐點</div>';
            }

            function selectMeal(mealType, lunchId) {
                const currentDateStr = weekDates[selectedDateIndex].dateStr;
                if (!isDateOrderable(weekDates[selectedDateIndex].date)) return;

                // Update local state
                if (mealType === '不訂餐') {
                    ordersData[currentDateStr] = { lunchId: '0', type: mealType, orderNum: '1' };
                } else if (mealType === '素食') {
                    ordersData[currentDateStr] = { lunchId: '0', type: mealType, orderNum: '1' };
                } else {
                    ordersData[currentDateStr] = { lunchId: lunchId, type: mealType, orderNum: '1' };
                }

                renderMealCards();
                updateWeeklySummary();
            }

            function updateWeeklySummary() {
                const containerDesktop = document.getElementById('weeklySummaryDesktop');
                const containerMobile = document.getElementById('weeklySummaryMobile');
                const summaryCountEl = document.getElementById('summaryCount');
                const dayNames = ['週一', '週二', '週三', '週四', '週五'];

                let orderedCount = 0;

                let summaryHtml = weekDates.map((d, index) => {
                    const order = ordersData[d.dateStr];
                    let statusText = '未選擇';
                    let statusClass = 'text-gray-400';
                    let iconHtml = '<span class="text-gray-400">○</span>';

                    if (order && order.orderNum === '1') {
                        orderedCount++;
                        if (order.type === '不訂餐' || order.lunchId === '0') {
                            statusText = order.type === '素食' ? '素食' : '不訂餐';
                            statusClass = order.type === '素食' ? 'text-success font-medium' : 'text-gray-500';
                            iconHtml = order.type === '素食' ? '<span class="text-success">✓</span>' : '<span class="text-gray-500">✗</span>';
                        } else {
                            statusText = order.type || '已選擇';
                            statusClass = 'text-success font-medium';
                            iconHtml = '<span class="text-success">✓</span>';
                        }
                    }

                    return `
                    <div class="flex items-center justify-between py-2 ${index === selectedDateIndex ? 'bg-primary-50 -mx-2 px-2 rounded-lg' : ''}">
                        <div class="flex items-center space-x-2">
                            ${iconHtml}
                            <span class="text-gray-700">${d.display}</span>
                        </div>
                        <span class="${statusClass}">${statusText}</span>
                    </div>
                `;
                }).join('');

                // Update desktop version
                if (containerDesktop) {
                    containerDesktop.innerHTML = summaryHtml;
                }

                // Update mobile version
                if (containerMobile) {
                    containerMobile.innerHTML = summaryHtml;
                }

                // Update mobile collapsed summary count
                if (summaryCountEl) {
                    summaryCountEl.textContent = `${orderedCount}/5 已登記`;
                }
            }

            function saveAllOrders() {
                showLoading(true);

                const ordersToSave = JSON.stringify(ordersData);

                PageMethods.SaveWeekOrders(ordersToSave, function (result) {
                    showLoading(false);
                    if (result.success) {
                        showToast('訂餐成功！');
                    } else {
                        showToast(result.message || '儲存失敗，請稍後再試', false);
                    }
                }, function (error) {
                    showLoading(false);
                    showToast('發生錯誤，請稍後再試', false);
                    console.error(error);
                });
            }

            function toggleMobileSummary() {
                const collapsed = document.getElementById('summaryCollapsed');
                const expanded = document.getElementById('summaryExpanded');
                const expandIcon = document.getElementById('expandIcon');

                if (expanded.classList.contains('hidden')) {
                    // Show expanded view
                    collapsed.classList.add('hidden');
                    expanded.classList.remove('hidden');
                    expandIcon.classList.add('rotate-180');
                } else {
                    // Show collapsed view
                    expanded.classList.add('hidden');
                    collapsed.classList.remove('hidden');
                    expandIcon.classList.remove('rotate-180');
                }
            }

            function showLoading(show) {
                const overlay = document.getElementById('loadingOverlay');
                if (show) {
                    overlay.classList.remove('hidden');
                } else {
                    overlay.classList.add('hidden');
                }
            }

            function showToast(message, isSuccess = true) {
                const toast = document.getElementById('toast');
                const toastMessage = document.getElementById('toastMessage');

                toastMessage.textContent = message;
                toast.querySelector('div').className = `${isSuccess ? 'bg-success' : 'bg-red-500'} text-white px-6 py-3 rounded-lg shadow-lg flex items-center space-x-2`;

                toast.classList.remove('translate-y-20', 'opacity-0');
                toast.classList.add('translate-y-0', 'opacity-100');

                setTimeout(() => {
                    toast.classList.add('translate-y-20', 'opacity-0');
                    toast.classList.remove('translate-y-0', 'opacity-100');
                }, 3000);
            }

            // Modal Functions
            function openMealModal(mealType, lunchId, isClickable) {
                const currentDateStr = weekDates[selectedDateIndex].dateStr;
                const dayMeals = mealsData[currentDateStr] || {};
                const dayOrder = ordersData[currentDateStr] || { lunchId: null, type: null };

                // Find meal info
                const meal = mealTypes.find(m => m.type === mealType);
                const mealInfo = dayMeals[mealType] || {};
                const isSelected = dayOrder.type === mealType || (mealType === '不訂餐' && dayOrder.lunchId === '0' && dayOrder.orderNum === '1');
                const mealName = mealInfo.name || meal.desc || '';

                const canOrder = isDateOrderable(weekDates[selectedDateIndex].date);

                // Update modal content
                document.getElementById('modalIcon').textContent = meal.icon;
                document.getElementById('modalTitle').textContent = meal.name;
                document.getElementById('modalSubtitle').textContent = meal.nameEn;
                document.getElementById('modalImage').innerHTML = `<div class="text-6xl h-full flex items-center justify-center">${meal.icon}</div>`;
                document.getElementById('modalDate').textContent = weekDates[selectedDateIndex].display;
                document.getElementById('modalDescription').innerHTML = mealName || meal.desc || '本餐點為' + meal.name;

                // Update action buttons
                const actionsHtml = `
                    ${isClickable && canOrder ? `
                        <button type="button" 
                            onclick="selectMealFromModal('${mealType}', '${lunchId}')"
                            class="flex-1 py-3 px-6 rounded-xl font-semibold transition-all duration-300 ${isSelected ? 'bg-success text-white' : 'bg-primary hover:bg-primary-dark text-white'} shadow-lg hover:shadow-xl">
                            ${isSelected ? '✓ 已選擇' : (mealType === '不訂餐' ? '登記不訂餐' : '選擇此餐點')}
                        </button>
                    ` : !canOrder ? `
                        <div class="flex-1 py-3 px-6 rounded-xl bg-gray-200 text-gray-500 text-center font-semibold">
                            無法訂餐
                        </div>
                    ` : ''}
                    <button type="button" 
                        onclick="closeModal()"
                        class="py-3 px-6 rounded-xl border-2 border-gray-300 text-gray-700 font-semibold hover:bg-gray-50 transition-all duration-300">
                        關閉
                    </button>
                `;
                document.getElementById('modalActions').innerHTML = actionsHtml;

                // Show modal
                const modal = document.getElementById('mealModal');
                modal.classList.add('active');
                document.body.style.overflow = 'hidden';
            }

            function closeModal(event) {
                if (event && event.target.id !== 'mealModal') return;

                const modal = document.getElementById('mealModal');
                modal.classList.remove('active');
                document.body.style.overflow = '';
            }

            function selectMealFromModal(mealType, lunchId) {
                selectMeal(mealType, lunchId);
                closeModal();
            }

            // Close modal on ESC key
            document.addEventListener('keydown', function (e) {
                if (e.key === 'Escape') {
                    closeModal();
                }
            });
        </script>
    </body>

    </html>

