<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="orders.aspx.cs" Inherits="FineUIPro.OrderMeal.orders" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />  
        <f:Panel ID="Panel1" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
            <Items>
                <f:DatePicker ID="dpdate" LabelWidth="80px" Width="200px" runat="server"
                            Label="訂餐日期" CssClass="marginr">
                        </f:DatePicker>
                        <f:TextBox ID="tbxUserno" LabelWidth="50px" Width="200px"  runat="server" Label="工号" Text=""　CssClass="marginr">
                        </f:TextBox>
                        <f:Button ID="btnFind" Text="查詢" CssClass="marginr" runat="server" OnClick="btnFind_Click">
                        </f:Button> 
                        <f:Button ID="btnNew" Text="訂餐" CssClass="marginr" Icon="Add" EnablePostBack="false" runat="server">
                            <Listeners>
                                <f:Listener Event="click" Handler="onNewButtonClick" />
                            </Listeners>
                        </f:Button>
                        <f:Button ID="btnEdit" Text="编辑" CssClass="marginr" Icon="Pencil" EnablePostBack="false" runat="server">
                            <Listeners>
                                <f:Listener Event="click" Handler="onEditButtonClick" />
                            </Listeners>
                        </f:Button>     
                </Items>
            </f:Panel>
        <f:Grid ID="Grid1" ShowBorder="true"  EnableCollapse="true"
            runat="server" DataKeyNames="ORDERID,USERNO" AllowCellEditing="true" ClicksToEdit="2"
            DataIDField="ORDERID"  Width="800px"  >            
            <Columns>
                <f:TemplateField Width="60px">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                    </ItemTemplate>
                </f:TemplateField>
               <f:RenderField Width="120px" ColumnID="ORDERDATE" DataField="ORDERDATE" FieldType="Date"
                    Renderer="Date" RendererArgument="yyyy-MM-dd" HeaderText="訂餐日期" >
                </f:RenderField>
                <f:RenderField ColumnID="LUNCHTYPE" DataField="LUNCHTYPE"
                    HeaderText="類型">
                </f:RenderField>                
                <f:RenderField Width="80px" ColumnID="ORDERNUM" DataField="ORDERNUM" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>
                <f:RenderField  ColumnID="USERNO" DataField="USERNO"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="LUNCHNAME" DataField="LUNCHNAME" ExpandUnusedSpace="true"
                    HeaderText="名稱">
                </f:RenderField>
                                
            </Columns>
        </f:Grid>

        <f:Window ID="Window1" Title="彈出窗體" Hidden="true" EnableIFrame="false"
            EnableMaximize="true" Target="Self" EnableResize="true" runat="server"
            IsModal="true" Width="600px">
            <Items>
                <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="80px" BodyPadding="10px">
                    <Items>
                        <f:HiddenField ID="hfFormID" runat="server"></f:HiddenField>
                       <f:Panel ID="Panel4" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                    <Items>
                        <f:DatePicker ID="dpFormdate" LabelWidth="50px" Width="200px" Required="True" ShowRedStar="true" runat="server"
                            Label="日期" AutoPostBack="true" OnTextChanged="dpFormdate_TextChanged" CssClass="marginr"  >
                        </f:DatePicker>
                        <f:DropDownList ID="ddlFormtype" LabelWidth="50px" Width="150px" CssClass="marginr" runat="server" Label="類型"　Required="True" ShowRedStar="true" AutoPostBack="true" OnSelectedIndexChanged="ddlFormtype_SelectedIndexChanged">
                            <f:ListItem Text="自助餐" Value="自助餐" />
                            <f:ListItem Text="簡餐" Value="簡餐" />
                            <f:ListItem Text="麵食" Value="麵食" />
                            <f:ListItem Text="素食" Value="素食" />
                            <f:ListItem Text="不訂餐" Value="不訂餐" />
                        </f:DropDownList>
                        <f:NumberBox ID="nbxFromnum" runat="server" Text="1" ShowTrigger="false" Width="40px" CssClass="marginr" Required="True"></f:NumberBox>
                        <f:Label ID="Label2" runat="server" Text="份" Width="40px" CssClass="marginr"></f:Label>
                        <f:TextBox ID="tbxFormUserno" LabelWidth="50px" Width="150px"  runat="server" Label="工號" Text=""　Required="True" ShowRedStar="true" CssClass="marginr"  ></f:TextBox>
                        </Items>
                            </f:Panel>
                        <f:TextArea ID="taname" Height="200px" Label="菜單" runat="server" Readonly="true" />                       
                    </Items>
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Bottom" ToolbarAlign="Right" runat="server">
                            <Items>
                                <f:Button ID="btnSave"
                                    Icon="SystemSave" runat="server" Text="保存數據" ValidateForms="SimpleForm1" OnClick="btnSave_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:SimpleForm>
            </Items>
        </f:Window>
        <br />
        <br />
        <br />
    </form>
    <script>   


        var windowClientID = '<%= Window1.ClientID %>';
        var gridClientID = '<%= Grid1.ClientID %>';
        var btnSaveClientID = '<%= btnSave.ClientID %>';

        var formClientID = '<%= SimpleForm1.ClientID %>';
        var hfFormIDClientID = '<%= hfFormID.ClientID %>';
        var dpFormdateClientID = '<%= dpFormdate.ClientID %>';
        var ddlFormtypeClientID = '<%= ddlFormtype.ClientID %>';
        var nbxFromnumClientID = '<%= nbxFromnum.ClientID %>';
        var tbxFormUsernoClientID = '<%= tbxFormUserno.ClientID %>';
        var lbtanameClientID = '<%= taname.ClientID %>';


        function onNewButtonClick(event) {
            // 重置表单字段
            F(formClientID).reset();
            F(dpFormdateClientID).setReadonly(false);
            F(tbxFormUsernoClientID).setReadonly(false);
            // 弹出窗体
            F(windowClientID).show();
            F(windowClientID).setTitle('訂餐');
        }

        function onEditButtonClick(event) {
            F(dpFormdateClientID).setReadonly(true);
            F(tbxFormUsernoClientID).setReadonly(true);
            showEditWindow();
        }

        function showEditWindow(rowId) {
            var grid = F(gridClientID);

            // 如果传入参数为空，则获取当前选中行
            if (!rowId) {
                var selectedRowIds = grid.getSelectedRows();
                if (!selectedRowIds.length) {
                    F.alert('請至少選擇一項！');
                    return;
                }

                rowId = selectedRowIds[0];
            }

            // 弹出新增窗体
            F(windowClientID).show();
            F(windowClientID).setTitle('編輯數據');

            // 当前行数据
            var rowValue = grid.getRowValue(rowId);

            // 使用当前行数据填充表单字段
            F(hfFormIDClientID).setValue(rowId);
            F(dpFormdateClientID).setValue(rowValue['ORDERDATE']);            
            F(ddlFormtypeClientID).setValue(rowValue['LUNCHTYPE']);
            F(nbxFromnumClientID).setValue(rowValue['ORDERNUM']);
            F(tbxFormUsernoClientID).setValue(rowValue['USERNO']);           
            F(lbtanameClientID).setValue(rowValue['LUNCHNAME']);
        }

    </script>
</body>
</html>

