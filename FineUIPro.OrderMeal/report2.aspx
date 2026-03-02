<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report2.aspx.cs" Inherits="FineUIPro.OrderMeal.report2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
            <f:SimpleForm ID="SimpleForm2" CssClass="mysimpleform" Width="600px" BodyPadding="5px" ShowHeader="false" ShowBorder="false"  runat="server">
                    <Items>
       <f:Panel ID="Panel4" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                       <f:Label ID="Label58" runat="server" Text="訂餐日期："></f:Label>
                        <f:DatePicker runat="server" Width="150px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                            ShowLabel="false" ID="dpRepotStartDate" EnableEdit="false">
                        </f:DatePicker>
                        <f:DatePicker ID="deReportEndDate" Width="150px" CssClass="marginr" Readonly="false" Required="true"
                            CompareControl="dpRepotStartDate" DateFormatString="yyyy-MM-dd" CompareOperator="GreaterThanEqual" CompareMessage="結束日期應該大於或等於開始日期" 
                            ShowLabel="false" runat="server" EnableEdit="false">
                        </f:DatePicker>                       
                        <f:Button runat="server" ID="btnReport" Text="查詢" ValidateForms="SimpleForm2" CssClass="marginr" OnClick="btnReportSubmit_Click"></f:Button>
                        <f:Button runat="server" ID="btnExcel" Text="導出為Excel文件" CssClass="marginr" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnExcel_Click"></f:Button> 
                    </Items>
                </f:Panel>   
                         </Items>
                </f:SimpleForm>
                     <f:Grid ID="Grid5" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" AllowCellEditing="false" DataIDField="orderdate"  Width="600px" DataKeyNames="orderdate" ForceFit="true"  >                     
            <Columns> 
                <f:BoundField DataField="orderdate" ColumnID="orderdate" HeaderText="日期" DataFormatString="{0:yyyy-MM-dd}" />
                <f:BoundField DataField="zzcnum" ColumnID="zzcnum" HeaderText="自助餐" />
                <f:BoundField DataField="jcnum" ColumnID="jcnum" HeaderText="簡餐" />
                <f:BoundField DataField="msnum" ColumnID="msnum" HeaderText="麵食" />
                <f:BoundField DataField="ssnum" ColumnID="ssnum" HeaderText="素食" />
                <f:BoundField DataField="qsnum" ColumnID="qsnum" HeaderText="輕食餐" />
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
