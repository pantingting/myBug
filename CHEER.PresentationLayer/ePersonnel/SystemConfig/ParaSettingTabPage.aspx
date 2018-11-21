<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParaSettingTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.SystemConfig.ParaSettingTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager ID="PageManager1" runat="server" AutoSizePanelID="PageTabs" />
        <c:TabStrip ID="PageTabs" runat="server" BoxFlex="100" ActiveTabIndex="1" ShowBorder="false"
            EnableTabCloseMenu="false" EnableFrame="false" AutoPostBack="true">
        </c:TabStrip>
    </form>
</body>
</html>
