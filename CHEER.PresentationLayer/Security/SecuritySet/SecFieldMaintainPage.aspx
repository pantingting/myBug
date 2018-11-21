<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecFieldMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecFieldMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../../script/mergeCells.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false" BodyPadding="5px">
            <Regions>
                <c:Region runat="server" ShowBorder="true" ShowHeader="false" Position="Left" Width="340" EnableCollapse="true" Layout="Fit" Split="true">
                    <Items>
                        <c:Tree runat="server" ShowBorder="false" ShowHeader="false" ID="UlMenuTree" OnNodeCommand="UlMenuTree_NodeCommand"></c:Tree>
                    </Items>
                </c:Region>
                <c:Region runat="server" ShowBorder="true" ShowHeader="false" Position="Center" Layout="Fit">
                    <Items>
                        <c:Grid runat="server" ID="UlFieldSetGrid" ShowBorder="false" ShowHeader="false" EnableColumnLines="true"></c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <script>
        F.ajaxReady(function () {
            mergeCells(F('<%=UlFieldSetGrid.ClientID%>'), [1]);
        });
    </script>
</body>
</html>
