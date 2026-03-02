<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report.aspx.cs"
    Inherits="FineUIPro.OrderMeal.report" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
         .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }     
    </style>
</head>
<body>
    <form id="form1" runat="server">       
       <f:PageManager ID="PageManager1" runat="server" />  
        <f:SimpleForm ID="SimpleForm1" CssClass="mysimpleform" Width="600px" BodyPadding="5px" ShowHeader="false" ShowBorder="false"  runat="server">
                    <Items>
       <f:Panel ID="Panel2" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                       <f:Label ID="Label1" runat="server" Text="訂餐日期："></f:Label>
                        <f:DatePicker runat="server" Width="150px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                            ShowLabel="false" ID="dpStartDate" EnableEdit="false">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" Width="150px" CssClass="marginr" Readonly="false" Required="true"
                            CompareControl="dpStartDate" DateFormatString="yyyy-MM-dd" CompareOperator="GreaterThanEqual" CompareMessage="結束日期應該大於或等於開始日期" 
                            ShowLabel="false" runat="server" EnableEdit="false">
                        </f:DatePicker>                       
                        <f:Button runat="server" ID="btnSubmit" Text="查詢" ValidateForms="SimpleForm1" OnClick="btnSubmit_Click"></f:Button>
                    </Items>
                </f:Panel>   
                         </Items>
                </f:SimpleForm>
        <f:Grid ID="Grid1" ShowBorder="true"  EnableCollapse="true" EnableSummary="true"  SummaryPosition="Flow"
            runat="server" DataKeyNames="userno,username" AllowCellEditing="true" ClicksToEdit="2"
            DataIDField="userno"  Width="700px"  >                     
            <Columns>                            
                <f:RenderField ColumnID="shiftname" DataField="shiftname"
                    HeaderText="部門">
                </f:RenderField>   
                <f:RenderField  ColumnID="userno" DataField="userno"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="username" DataField="username"  ExpandUnusedSpace="true"
                    HeaderText="姓名">
                </f:RenderField>
                <f:RenderField  ColumnID="lunchtype" DataField="lunchtype"
                    HeaderText="類型">
                </f:RenderField>
                <f:RenderField Width="80px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>                
            </Columns>
        </f:Grid>               
            
    </form>
</body>
</html>
