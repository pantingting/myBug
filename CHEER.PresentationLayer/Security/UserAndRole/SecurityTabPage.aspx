<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.SecurityTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel1"/>
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="5px">
            <Items>
                <c:TabStrip ID="mainTabStrip" runat="server">
                    <Tabs>
                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
