<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="FineUIPro.OrderMeal.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <style>
        .imgcaptcha .f-field-label {
            margin: 0;
        }

        .login-image {
            border-width: 0 1px 0 0;
            width: 116px;
            height: 116px;
        }

            .login-image .ui-icon {
                font-size: 96px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <f:PageManager ID="PageManager1" runat="server" />
        <f:Window ID="Window1" runat="server" Title="團膳員工點餐系統登录" IsModal="false" EnableClose="false"
            WindowPosition="GoldenSection" Width="350px">
            <Items>
                <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px"
                    LabelWidth="80px" ShowHeader="false">
                    <Items>
                        <f:TextBox ID="tbUserNo" Label="工號" Required="true" ShowRedStar="true" runat="server" >
                        </f:TextBox>  
                        <f:TextBox ID="tbPassword" TextMode="Password" Label="密碼" runat="server" >
                        </f:TextBox> 
                    </Items>
                </f:SimpleForm>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right" Position="Bottom">
                    <Items>
                        <f:Button ID="btnLogin" Text="登錄" Type="Submit" ValidateForms="SimpleForm1" ValidateTarget="Top"
                            runat="server" OnClick="btnLogin_Click">
                        </f:Button>
                        <f:Button ID="btnReset" Text="重置" Type="Reset" EnablePostBack="false"
                            runat="server">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
    </form>
</body>
</html>
