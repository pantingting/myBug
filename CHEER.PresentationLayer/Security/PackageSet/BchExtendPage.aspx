<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BchExtendPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.BchExtendPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" BodyPadding="5px" ShowHeader="false" Layout="Fit">
            <Items>
                <c:Grid runat="server" ID="UlAbstractGrid" ShowBorder="true" ShowHeader="false" EnableColumnLines="true" AllowPaging="true" OnRowCommand="UlAbstractGrid_RowCommand" >

                </c:Grid> 
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="txtPackageID"></c:HiddenField>
    <c:Window runat="server" ID="windowMain" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="380px" Width="450px" CloseAction="HidePostBack" OnClose="windowMain_Close"></c:Window>
    <script>
        function showDetil(url, dimid)
        {
            var packageid = C('<%=txtPackageID.ClientID%>').getValue();
            C('<%=windowMain.ClientID%>').c_show('<%=getBaseUrl()%>' + url + '?DIMENSIONID=' + dimid + '&PACKAGEID=' + packageid, '<%=base.getString("ZGAIA00532")%>');
        }
    </script>
</body>
</html>
