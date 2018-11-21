<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecuritySetPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecuritySetPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" ID="RegionPanel1" BodyPadding="5px" ShowBorder="false">
            <Regions>
                <c:Region runat="server" BodyPadding="5px" Position="Top" ShowHeader="false" CssStyle="border-bottom:none;">
                    <Items>
                        <c:Button runat="server" ID="cmdSave" Text="保存" Icon="SystemSave" OnClick="cmdSave_Click"></c:Button>
                    </Items>
                </c:Region>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Left" Width="200px" Layout="Fit">
                    <Items>
                        <c:Tree ShowHeader="false" runat="server" ID="UlModuleTree" OnNodeCommand="UlModuleTree_NodeCommand">
                        </c:Tree>
                    </Items>
                </c:Region>
                <c:Region runat="server" CssStyle="border-left:none !important;" ShowHeader="false" Position="Center" Layout="Fit">
                    <Items>
                        <c:Tree runat="server" ID="UlFunTree" ShowBorder="false" ShowHeader="false" OnNodeCheck="UlFunTree_NodeCheck" >
                        </c:Tree>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <c:HiddenField runat="server" ID="txtIsBack"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtPackageID"></c:HiddenField>
    <c:Label runat="server" ID="lbMenuIDList" Hidden="true"></c:Label>
</body>
</html>
