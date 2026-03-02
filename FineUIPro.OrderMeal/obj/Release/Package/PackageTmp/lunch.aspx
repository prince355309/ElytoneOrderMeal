<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lunch.aspx.cs" Inherits="FineUIPro.OrderMeal.lunch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="SimpleForm1" runat="server" />
        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
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
                            Label="日期" AutoPostBack="true" OnTextChanged="dpdate_TextChanged">
                        </f:DatePicker>
                        <f:DropDownList ID="ddltype" runat="server" Label="类型"　Required="True" ShowRedStar="true" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                            <f:ListItem Text="自助餐" Value="自助餐" />
                            <f:ListItem Text="簡餐" Value="簡餐" />
                            <f:ListItem Text="麵食" Value="麵食" />
                        </f:DropDownList>                      
                    </Items>
                </f:FormRow>
                <f:FormRow>
                    <Items>
                        <f:TextArea ID="taname" Height="200px" Label="描述" runat="server" />
                    </Items>
                </f:FormRow>
                
            </Rows>
        </f:Form>
    </form>
</body>
</html>
