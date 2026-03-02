<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report5.aspx.cs" Inherits="FineUIPro.OrderMeal.report5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
            <f:SimpleForm ID="SimpleForm5" CssClass="mysimpleform" Width="800px" BodyPadding="5px" ShowHeader="false" ShowBorder="false"  runat="server">
                    <Items>
                        <f:Panel ID="Panel6" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                        <Items>
                            <f:Label ID="Label15" runat="server" Text="訂餐日期："></f:Label>
                            <f:DatePicker runat="server" Width="110px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                                ShowLabel="false" ID="dpMonthDateS" EnableEdit="false">
                            </f:DatePicker>
                            <f:DatePicker runat="server" Width="110px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                                ShowLabel="false" ID="dpMonthDateE" EnableEdit="false">
                            </f:DatePicker>
                            <f:Button runat="server" ID="btnMonthSave" Text="查詢" CssClass="marginr"  ValidateForms="SimpleForm1" OnClick="btnMonthSubmit_Click"></f:Button>
                            <f:Button runat="server" ID="btnMonthExcel" Text="導出為Excel文件" CssClass="marginr" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnMonthExcel_Click"></f:Button> 
                        </Items>
                    </f:Panel>   
                    </Items>
                </f:SimpleForm>
                     <f:Grid ID="Grid7" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" Width="500px"  >                     
                        <Columns>                            
                            <f:RenderField Width="180px" ColumnID="shiftname" DataField="shiftname"
                                HeaderText="部門">
                            </f:RenderField>   
                            <f:RenderField  ColumnID="userno" DataField="userno"
                                HeaderText="工號">
                            </f:RenderField>
                            <f:RenderField  ColumnID="username" DataField="username"  ExpandUnusedSpace="true"
                                HeaderText="姓名">
                            </f:RenderField>                
                            <f:RenderField Width="80px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                                HeaderText="份">
                            </f:RenderField>                
                        </Columns>
                    </f:Grid> 
    </form>
</body>
</html>
