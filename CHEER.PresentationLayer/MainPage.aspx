<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="CHEER.PresentationLayer.MainPage" EnableEventValidation="false" %>

<%@ Register Assembly="CheerUI" Namespace="CheerUI" TagPrefix="c" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style>
       
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="MainPanel"></c:PageManager>
        <c:Panel AutoScroll="true" runat="server" ID="MainPanel" ShowHeader="false" ShowBorder="false" BodyPadding="5px">
            <Items>
                
            </Items>
        </c:Panel>
    </form>
</body>
</html>