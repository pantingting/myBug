<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CHEER.PresentationLayer.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager ID="PageManager1" runat="server" Theme="Bootstrap" />
        <c:Window ID="Window1" runat="server" Title="修改密码" IsModal="false" EnableClose="false"
            WindowPosition="GoldenSection" Width="350px">
            <Items>
                <c:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px"
                    LabelWidth="80px" ShowHeader="false">
                    <Items>
                        <c:TextBox runat="server" ID="txtUserName" Label="登录名" Required="true" ShowRedStar="true"></c:TextBox>
                        <c:TextBox ID="tbxOldPwd" Label="原始密码" TextMode="Password" runat="server">
                        </c:TextBox>
                        <c:TextBox ID="tbxNewPwd" Label="新密码" TextMode="Password" runat="server">
                        </c:TextBox>
                        <c:TextBox ID="tbxConfirmPwd" Label="确认密码" TextMode="Password" runat="server">
                        </c:TextBox>
                    </Items>
                </c:SimpleForm>
            </Items>
            <Toolbars>
                <c:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right" Position="Bottom">
                    <Items>
                        <c:Button ID="btnModify" Text="修改" Type="Submit" ValidateForms="SimpleForm1" ValidateTarget="Top"
                            runat="server" OnClick="btnModify_Click">
                        </c:Button>
                        <c:Button runat="server" ID="btnClose" Text="返回"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
        </c:Window>
    </form>
</body>
</html>
