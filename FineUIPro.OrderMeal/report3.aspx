<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report3.aspx.cs" Inherits="FineUIPro.OrderMeal.report3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
            <f:SimpleForm ID="SimpleForm3" CssClass="mysimpleform" Width="600px" BodyPadding="5px" ShowHeader="false" ShowBorder="false"  runat="server">
                    <Items>
       <f:Panel ID="Panel5" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                       <f:Label ID="Label7" runat="server" Text="訂餐日期："></f:Label>
                        <f:DatePicker runat="server" Width="150px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                            ShowLabel="false" ID="dpOrderDate" EnableEdit="false">
                        </f:DatePicker>                            
                        <f:Button runat="server" ID="btnZZ" Text="查詢" ValidateForms="SimpleForm2" CssClass="marginr" OnClick="btnZZSubmit_Click"></f:Button>  
                        <f:Button runat="server" ID="btnNotorder" Text="導出為Excel文件" CssClass="marginr" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnNotorderExcel_Click"></f:Button> 
                    </Items>
                </f:Panel>   
                         </Items>
                </f:SimpleForm>
                     <f:Grid ID="Grid6" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" runat="server" AllowCellEditing="false" DataIDField="userno"  Width="500px" DataKeyNames="userno" ForceFit="true"  >                     
            <Columns>                
                <f:BoundField DataField="username" ColumnID="username" HeaderText="姓名" />
                <f:BoundField DataField="userno" ColumnID="userno" HeaderText="工號" />
                <f:BoundField DataField="shiftname" ColumnID="shiftname" HeaderText="部門" />
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
