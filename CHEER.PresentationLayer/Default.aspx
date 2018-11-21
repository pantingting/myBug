<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CHEER.PresentationLayer._Default" %>

<!DOCTYPE HTML>

<html>
<head id="he" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MyBug</title>
    <link rel="Shortcut Icon" href="favicon.ico">
    <style>
        .c-field-body {
            height: 38px;
            font-size: 18px;
            border: none;
        }

        .ui-state-focus {
            background: none !important;
        }

        .center {
            position: absolute;
            width: 373px;
            height: 259px;
            left: 50%;
            top: 50%;
            margin-left: -186.5px;
            margin-top: -129.5px;
            background-image: url('image/xxx-04.png');
        }
    </style>
</head>
<body style="background: url('image/xxx-05.jpg'); background-size: cover;">
    <form id="form1" runat="server">
        <c:PageManager  runat="server"></c:PageManager>
        <div class="center">
            <div style="position:fixed;top:30px;right:40px"><a href="javascript:void(0)" style="text-decoration:none;color:white" runat="server" id="aChangePassword" onclick="document.location.replace('ChangePassWord.aspx');return false;">修改密码</a></div>
            <div style="color: white; font-size: 18px; margin-top: 25px; margin-left: 50px;">MyBug</div>
            <div style="height: 15px;"></div>
            <div style="border-radius: 4px; background-color: white; margin-left: 48px; width: 280px;">
                <div style="height: 50px; width: 100%; vertical-align: central">
                    <c:Image IconFont="User" runat="server" Height="40px" Width="40px" ImageCssStyle="  height: 40px;line-height: 40px;margin-left:8px;font-size:28px;"></c:Image>
                    <c:TextBox ID="txtUserName" EmptyText="用户名" Width="220px" runat="server"></c:TextBox>
                </div>
                <div style="height: 50px; width: 100%; border-top: 1px solid #ccc;">
                    <c:Image IconFont="Key" runat="server" Height="40px" Width="40px" ImageCssStyle="height: 40px;line-height: 40px;margin-left:8px;font-size:26px;">
                    </c:Image>
                    <c:TextBox ID="txtPassword" TextMode="Password" EmptyText="密码" Width="220px" runat="server"></c:TextBox>
                </div>
            </div>
            <div style="height: 20px;"></div>
            <asp:Button runat="server" onmouseover="this.style.backgroundColor='rgb(211, 99, 17)'" onmouseout="this.style.backgroundColor='#ff964a'" OnClick="btnLogin_Click" Text="登录" Style="font-size: 16px; color: #FFF; margin-left: 48px; width: 280px; height: 47px; border: none; cursor: pointer; line-height: 47px; border-radius: 4px; background-color: #ff964a; outline: none" />
        </div>
    </form>
    <script>
        C.ready(function () {
            C('<%= txtUserName.ClientID %>').bodyEl.attr('spellcheck', false);
            C('<%= txtPassword.ClientID %>').bodyEl.attr('spellcheck', false);
        });
    </script>
</body>
</html>

