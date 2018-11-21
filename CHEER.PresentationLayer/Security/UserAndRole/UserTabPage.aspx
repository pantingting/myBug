<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.UserTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" Layout="Fit" ShowHeader="false" BodyPadding="5px">
            <Items>
                <c:TabStrip runat="server" ShowBorder="true" ID="mainTabStrip" AutoPostBack="true" >
                    <Tabs>
                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="txtUserID"></c:HiddenField>
    <script>
        C.ready(function () {
            C('<%=mainTabStrip.ClientID%>').on('tabchange', function (event) {
                var index = this.c_getActiveTabIndex();
                if (index !== 0) {
                    var val = C('<%=txtUserID.ClientID%>').getValue();
                    if (val === '') {
						top.C.alert('<%="请先维护用户基本资料!"%>');
						window.location.reload();
                        return false;
                    }
                }
            });
        });
        function sendBack(val) {
            C('<%=txtUserID.ClientID%>').setValue(val);
            var tab = C('<%=mainTabStrip.ClientID%>');
            var roleTab = tab.getTab('<%=mainTabStrip.ClientID%>' + '_UserRole');
            roleTab.setIFrameUrl('UserRolePage.aspx?USERID=' + val);
            setTimeout(function () {
                tab.setActiveTab(roleTab);
            }, 2000);
        }
    </script>
</body>
</html>
