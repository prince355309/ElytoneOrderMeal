<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report1.aspx.cs" Inherits="FineUIPro.OrderMeal.report1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" CssClass="mysimpleform" Width="800px" BodyPadding="5px" ShowHeader="false" ShowBorder="false"  runat="server">
                    <Items>
       <f:Panel ID="Panel3" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                       <f:Label ID="Label57" runat="server" Text="訂餐日期："></f:Label>
                        <f:DatePicker runat="server" Width="140px" CssClass="marginr" Required="true" DateFormatString="yyyy-MM-dd" 
                            ShowLabel="false" ID="dpStartDate" EnableEdit="false">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" Width="140px" CssClass="marginr" Readonly="false" Required="true"
                            CompareControl="dpStartDate" DateFormatString="yyyy-MM-dd" CompareOperator="GreaterThanEqual" CompareMessage="結束日期應該大於或等於開始日期" 
                            ShowLabel="false" runat="server" EnableEdit="false">
                        </f:DatePicker> 
                        <f:DropDownList ID="ddltype" runat="server" Label="类型" LabelWidth="50px" Width="150px" >
                            <f:ListItem Text="全部" Value="" />
                            <f:ListItem Text="自助餐" Value="自助餐" />
                            <f:ListItem Text="簡餐" Value="簡餐" />
                            <f:ListItem Text="麵食" Value="麵食" />
                            <f:ListItem Text="素食" Value="素食" />
                            <f:ListItem Text="輕食餐" Value="輕食餐" />
                        </f:DropDownList>
                        <f:Button runat="server" ID="btnSubmit" Text="查詢" CssClass="marginr"  ValidateForms="SimpleForm1" OnClick="btnSubmit_Click"></f:Button>
                        <f:Button runat="server" ID="btnReportExcel" Text="導出Excel文件1" CssClass="marginr" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnReportExcel_Click"></f:Button>
                        <f:Button runat="server" ID="btnReportExcel2" Text="導出Excel文件2" CssClass="marginr" EnableAjax="false" DisableControlBeforePostBack="false" OnClick="btnReportExcel2_Click"></f:Button>
                    </Items>
                </f:Panel>   
                         </Items>
                </f:SimpleForm>
                        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" Width="700px"  >                     
            <Columns>                            
                <f:RenderField Width="170px" ColumnID="shiftname" DataField="shiftname"
                    HeaderText="部門">
                </f:RenderField>   
                <f:RenderField  ColumnID="userno" DataField="userno"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="username" DataField="username"
                    HeaderText="姓名">
                </f:RenderField>
                <f:RenderField  ColumnID="lunchtype" DataField="lunchtype"
                    HeaderText="類型">
                </f:RenderField>
                 <f:RenderField  ColumnID="updatedate" DataField="updatedate"  ExpandUnusedSpace="true"
                    HeaderText="日期">
                </f:RenderField>
                <f:RenderField Width="60px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>                
            </Columns>
        </f:Grid>
                        <f:Grid ID="Grid2" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" Width="700px"  >                     
            <Columns>                            
                <f:RenderField Width="170px" ColumnID="shiftname" DataField="shiftname"
                    HeaderText="部門">
                </f:RenderField>   
                <f:RenderField  ColumnID="userno" DataField="userno"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="username" DataField="username"
                    HeaderText="姓名">
                </f:RenderField>
                <f:RenderField  ColumnID="lunchtype" DataField="lunchtype"
                    HeaderText="類型">
                </f:RenderField>
                <f:RenderField  ColumnID="updatedate" DataField="updatedate"  ExpandUnusedSpace="true"
                    HeaderText="日期">
                </f:RenderField>
                <f:RenderField Width="60px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>                
            </Columns>
        </f:Grid>
                        <f:Grid ID="Grid3" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" Width="700px"  >                     
            <Columns>                            
                <f:RenderField Width="170px" ColumnID="shiftname" DataField="shiftname"
                    HeaderText="部門">
                </f:RenderField>   
                <f:RenderField  ColumnID="userno" DataField="userno"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="username" DataField="username"
                    HeaderText="姓名">
                </f:RenderField>
                <f:RenderField  ColumnID="lunchtype" DataField="lunchtype"
                    HeaderText="類型">
                </f:RenderField>
                <f:RenderField  ColumnID="updatedate" DataField="updatedate"  ExpandUnusedSpace="true"
                    HeaderText="日期">
                </f:RenderField>
                <f:RenderField Width="60px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>                
            </Columns>
        </f:Grid>
                        <f:Grid ID="Grid4" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" Width="700px"  >                     
            <Columns>                            
                <f:RenderField Width="170px" ColumnID="shiftname" DataField="shiftname"
                    HeaderText="部門">
                </f:RenderField>   
                <f:RenderField  ColumnID="userno" DataField="userno"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="username" DataField="username"
                    HeaderText="姓名">
                </f:RenderField>
                <f:RenderField  ColumnID="lunchtype" DataField="lunchtype"
                    HeaderText="類型">
                </f:RenderField>
                <f:RenderField  ColumnID="updatedate" DataField="updatedate"  ExpandUnusedSpace="true"
                    HeaderText="日期">
                </f:RenderField>
                <f:RenderField Width="60px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>                
            </Columns>
        </f:Grid>
            <f:Grid ID="Grid5" ShowBorder="true" ShowHeader="false"  EnableCollapse="false" EnableSummary="true"  SummaryPosition="Flow" runat="server" Width="700px"  >                     
            <Columns>                            
                <f:RenderField Width="170px" ColumnID="shiftname" DataField="shiftname"
                    HeaderText="部門">
                </f:RenderField>   
                <f:RenderField  ColumnID="userno" DataField="userno"
                    HeaderText="工號">
                </f:RenderField>
                <f:RenderField  ColumnID="username" DataField="username"
                    HeaderText="姓名">
                </f:RenderField>
                <f:RenderField  ColumnID="lunchtype" DataField="lunchtype"
                    HeaderText="類型">
                </f:RenderField>
                <f:RenderField  ColumnID="updatedate" DataField="updatedate"  ExpandUnusedSpace="true"
                    HeaderText="日期">
                </f:RenderField>
                <f:RenderField Width="60px" ColumnID="ordernum" DataField="ordernum" FieldType="Int"
                    HeaderText="份">
                </f:RenderField>                
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
