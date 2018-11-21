<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.RoleTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="panelMain" />
        <c:Panel runat="server" ID="panelMain" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="5px">
            <Items>
                <c:TabStrip runat="server" ID="mainTabStrip" AutoPostBack="true" >
                    <Tabs>
                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="txtRoleID"></c:HiddenField>
    <script>
       C.ready(function () {
            C('<%=mainTabStrip.ClientID%>').on('tabchange', function (event) {
                var index = this.c_getActiveTabIndex();
                if (index !== 0) {
                    var val = C('<%=txtRoleID.ClientID%>').getValue();
                    if (val === '') {
                        top.C.alert('<%="请先维护用户基本资料!"%>');
						//this.setActiveTab(0);
						window.location.reload();
                        return false;
                    }
                }
            });
        });

        function sendBack(val) {
            C('<%=txtRoleID.ClientID%>').setValue(val);
            var tab = C('<%=mainTabStrip.ClientID%>');
            var roleTab = tab.getTab('<%=mainTabStrip.ClientID%>' + '_RoleUser');
            roleTab.setIFrameUrl('RoleUserPage.aspx?ROLEID=' + val);
            setTimeout(function () {
                tab.setActiveTab(roleTab);
            }, 2000);
        }
    </script>
</body>
</html>
