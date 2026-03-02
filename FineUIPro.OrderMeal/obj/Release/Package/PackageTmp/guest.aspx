<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guest.aspx.cs" Inherits="FineUIPro.OrderMeal.guest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>團膳員工點餐系統</title>
    <link href="res/css/default.css" rel="stylesheet" />
    <link href="res/css/tablehtml.css" rel="stylesheet" />
        <style>
         .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }  
        .customlabel span {
            color: red;
            font-weight: bold;
        } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" Layout="Region" ShowBorder="false" ShowHeader="false" runat="server">
            <Items>
                <f:ContentPanel ID="topPanel" CssClass="topregion" RegionPosition="Top" ShowBorder="false" ShowHeader="false" EnableCollapse="true" runat="server">
                    <div id="header" class="ui-widget-header f-mainheader">
                        <table>
                            <tr>
                                <td>
                                    <f:Button runat="server" CssClass="icononlyaction" ID="btnHomePage" IconAlign="Top" IconFont="Home"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false" >
                                    </f:Button>
                                    <a class="logo" href="./guest.aspx" title="團膳員工點餐系統">團膳員工點餐系統
                                    </a>
                                </td>
                                <td style="text-align: right;">  
                                   <h2>您好 !&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</h2>
                                </td>
                            </tr>
                        </table>
                    </div>
                </f:ContentPanel>                
                <f:Panel ID="mainPanel" ShowHeader="false" Layout="Fit" RegionPosition="Center" runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                <f:Tab ID="Tab1" Title="菜單" EnableClose="false" BodyPadding="5px" runat="server" AutoScroll="true">
                    <Items>
                        <f:Panel ID="Panel2" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:DatePicker ID="DatePicker1" runat="server" Label="日期" LabelWidth="50px" Width="200px" CssClass="marginr" AutoPostBack="true" OnTextChanged="DatePicker1_TextChanged">
                        </f:DatePicker> 
                        <f:Button ID="btnOrder" Text="訂餐" CssClass="marginr" runat="server" >
                        </f:Button>        
                    </Items>
                </f:Panel>
                  <f:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" ShowHeader="false" MinHeight="700px">
              <table class="tablehtml">
                <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label1" Text="日期" runat="server" />   
                    </td>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label2" runat="server" />   
                    </td>
                    <td class="ui-widget-header label">
                     <f:Label ID="Label3" runat="server" />   
                    </td>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label4"  runat="server" /> 
                    </td>
                    <td class="ui-widget-header label">
                        <f:Label ID="Label5"  runat="server" /> 
                    </td>
                    <td class="ui-widget-header label">
                        <f:Label ID="Label6" runat="server" /> 
                    </td>                   
                </tr>
                 <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label9" Text="星期" runat="server" />   
                    </td>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label10" Text="星期一"  runat="server" />   
                    </td>
                    <td class="ui-widget-header label">
                     <f:Label ID="Label11" Text="星期二"  runat="server" />   
                    </td>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label12" Text="星期三"  runat="server" /> 
                    </td>
                    <td class="ui-widget-header label">
                        <f:Label ID="Label13" Text="星期四" runat="server" /> 
                    </td>
                    <td class="ui-widget-header label">
                        <f:Label ID="Label14" Text="星期五" runat="server" /> 
                    </td>                    
                </tr>
                <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label17" Text="自助餐"  runat="server" />   
                    </td>
                    <td class="ui-widget-content content">
                        <f:Label ID="Label18" EncodeText="false" runat="server"   /> <br />
                        <f:NumberBox ID="NumberBox1" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                        <f:Label ID="Label19" EncodeText="false"  runat="server" />  <br />                           
                        <f:NumberBox ID="NumberBox2" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label20" EncodeText="false"  runat="server" /> <br />                        
                        <f:NumberBox ID="NumberBox3" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                        <f:Label ID="Label21" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox4" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                        <f:Label ID="Label22" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox5" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>                    
                    
                </tr>
                <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label25" Text="簡餐" runat="server" />   
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label26" EncodeText="false" runat="server" /> <br />  
                        <f:NumberBox ID="NumberBox8" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label27" EncodeText="false"  runat="server" />  <br /> 
                        <f:NumberBox ID="NumberBox9" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                          <f:Label ID="Label28" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox10" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label29" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox11" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label30" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox12" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>                    
                   
                </tr>
                <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label33" Text="麵食" runat="server" />   
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label34" EncodeText="false"  runat="server" />   <br /> 
                        <f:NumberBox ID="NumberBox15" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label35" EncodeText="false"  runat="server" />   <br /> 
                        <f:NumberBox ID="NumberBox16" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label36" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox17" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label37" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox18" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label38" EncodeText="false"  runat="server" /> <br /> 
                        <f:NumberBox ID="NumberBox19" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>                   
                    
                </tr>
                  <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label7" Text="輕食餐" runat="server" />   
                    </td>
                    <td class="ui-widget-content content">
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label23" EncodeText="false"  runat="server" />   <br /> 
                        <f:NumberBox ID="NumberBox13" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label15" EncodeText="false"  runat="server" />   <br /> 
                        <f:NumberBox ID="NumberBox6" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label24" EncodeText="false"  runat="server" />   <br /> 
                        <f:NumberBox ID="NumberBox14" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label16" EncodeText="false"  runat="server" />   <br /> 
                        <f:NumberBox ID="NumberBox7" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                </tr>
                <tr>
                    <td class="ui-widget-header label">
                         <f:Label ID="Label41" Text="素食"  runat="server"  />   
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label42"  runat="server" Text="素食盒餐" /> <br /> 
                        <f:NumberBox ID="NumberBox22" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                        <f:Label ID="Label43"  runat="server" Text="素食盒餐" /> <br /> 
                        <f:NumberBox ID="NumberBox23" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label44"  runat="server" Text="素食盒餐" /> <br /> 
                        <f:NumberBox ID="NumberBox24" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                         <f:Label ID="Label45"  runat="server" Text="素食盒餐" /> <br />
                        <f:NumberBox ID="NumberBox25" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr"  Readonly="true" /> 
                    </td>
                    <td class="ui-widget-content content">
                        <f:Label ID="Label46"  runat="server" Text="素食盒餐" /> <br /> 
                        <f:NumberBox ID="NumberBox26" runat="server" ShowLabel="false" Width="40px"  CssClass="marginr" Readonly="true" /> 
                    </td>                  
                   
                </tr>             
            </table>
        </f:ContentPanel>                   
                  <f:Window ID="Window1"  runat="server" Hidden="true"
            WindowPosition="Center" IsModal="false" Title="菜单" EnableMaximize="true"
            EnableResize="true" Height="350px" Width="500px" >
                      <Items>
                           <f:Form ID="Form2" ShowBorder="false" ShowHeader="false"
            AutoScroll="true" BodyPadding="10px" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>                       
                        <f:Button ID="btnSave" Text="保存" runat="server" Icon="SystemSave"　OnClick="btnSave_Click" >
                        </f:Button>                       
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Rows>              
                <f:FormRow>
                    <Items>                        
                        <f:DatePicker ID="dpdate" Required="True" ShowRedStar="true" runat="server"
                            Label="日期" LabelWidth="50" AutoPostBack="true" OnTextChanged="dpdate_TextChanged">
                        </f:DatePicker>
                        <f:DropDownList ID="ddltype" runat="server" Label="类型" LabelWidth="50"　Required="True" ShowRedStar="true" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                            <f:ListItem Text="自助餐" Value="自助餐" />
                            <f:ListItem Text="簡餐" Value="簡餐" />
                            <f:ListItem Text="麵食" Value="麵食" />
                            <f:ListItem Text="素食" Value="素食" />
                            <f:ListItem Text="輕食餐" Value="輕食餐" />
                        </f:DropDownList>
                        <f:NumberBox ID="nbxnum" runat="server" ShowLabel="true" Label="份" LabelWidth="50" Required="True" ShowRedStar="true" MinValue="0" NoDecimal="true">
                        </f:NumberBox> 
                    </Items>
                </f:FormRow>
                <f:FormRow>
                    <Items>
                        <f:TextArea ID="taname" Height="200px" Label="描述" runat="server" Readonly="true" />
                        <f:Label ID="lblunchid" runat="server" Hidden="true" Text=""></f:Label>
                    </Items>
                </f:FormRow>                
            </Rows>
        </f:Form>
                      </Items>
        </f:Window>
                    </Items>
                </f:Tab>


            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>     

    </form>

</body>
</html>
