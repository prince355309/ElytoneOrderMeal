<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report4.aspx.cs" Inherits="FineUIPro.OrderMeal.report4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
            <f:SimpleForm ID="SimpleForm4" CssClass="mysimpleform" Width="600px" BodyPadding="5px" ShowHeader="false" ShowBorder="false"  runat="server">
                    <Items>
       <f:Panel ID="Panel7" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                       <f:Label ID="lbNoorder" runat="server" Text="不訂餐日期："></f:Label>
                        <f:DatePicker runat="server" Width="150px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                            ShowLabel="false" ID="dpNoorderdate" EnableEdit="false">
                        </f:DatePicker>                            
                        <f:Button runat="server" ID="btnNoorderSubmit" Text="查詢" ValidateForms="SimpleForm2" CssClass="marginr" OnClick="btnNoorderSubmit_Click"></f:Button>  
                        <f:Button runat="server" ID="btnNoorderExcel" Text="導出為Excel文件" CssClass="marginr" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnNoorderExcel_Click"></f:Button> 
                    </Items>
                </f:Panel>   
                         </Items>
                </f:SimpleForm>
                     <f:Grid ID="Grid8" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" runat="server" AllowCellEditing="false" DataIDField="userno"  Width="600px" DataKeyNames="userno"   >                     
            <Columns>                
                <f:BoundField DataField="username" ColumnID="username" HeaderText="姓名" Width="100px" />
                <f:BoundField DataField="userno" ColumnID="userno" HeaderText="工號" />
                <f:BoundField DataField="shiftname" ColumnID="shiftname" HeaderText="部門"  Width="170px"/>
                <f:RenderField DataField="updatedate"  ColumnID="updatedate" HeaderText="日期" ExpandUnusedSpace="true" />               
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
