<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreSelectStdOrgUnit.aspx.cs" Inherits="CHEER.PresentationLayer.Organize.Common.PreSelectStdOrgUnit" %>
<%@ Register Assembly="CheerUI" Namespace="CheerUI" TagPrefix="c" %>
<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager AutoSizePanelID="regionPanel" runat="server" ID="pageManager" AjaxLoadingType="Mask" />
        <c:RegionPanel ID="regionPanel" ShowBorder="false" runat="server" BodyPadding="5px">
            <Regions>
                <c:Region ID="regionTree" BodyPadding="5px" RegionSplit="true" RegionSplitWidth="2px" RegionSplitIcon="false" ShowBorder="true" Width="250px" ShowHeader="false" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <c:Tree BoxFlex="1" ShowBorder="true" EnableLines="true" AutoLeafIdentification="false" OnNodeLazyLoad="treeOrg_NodeLazyLoad" Title="行政组织维护子组织选择" runat="server" ID="treeOrg">
                            <Toolbars>
                                <c:Toolbar Hidden="true" runat="server">
                                    <Items>
                                        <c:Button runat="server" Text="查找" ID="btnSearch"></c:Button>
                                        <c:Button runat="server" Text="归档" ID="btnArchive"></c:Button>
                                        <c:Button runat="server" Text="复制" ID="btnCopy"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                        </c:Tree>
                    </Items>
                </c:Region>
                <c:Region ID="regionMain" ShowHeader="false" ShowBorder="false" Layout="Fit" Position="Center" runat="server">
                    <Items>
                        <c:TabStrip ID="tabMain" runat="server" ShowBorder="true" CssStyle="border-left:none !important;">
                            <Tabs>
                                <c:Tab Title="组织维护" ID="tabUnitManagerMaintainPage" IFrameUrl="../StdOrg/UnitManagerMaintainPage.aspx" runat="server" EnableIFrame="true">

                                </c:Tab>
                                <c:Tab Title="管理信息维护" ID="tabOrgMulPage" IFrameUrl="../StdOrg/OrgMulPage.aspx" runat="server" Hidden="true" EnableIFrame="true">

                                </c:Tab>
                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
        <c:Button runat="server" ID="btnRefresh" Hidden="true" OnClick="btnRefresh_Click"></c:Button>
    </form>
    <script>
        C.wnd.updateIFrameNode = function (panel, iframeUrl) {
            panel.setIFrameUrl(iframeUrl);
        } 
        function doTreeRefresh() {
            __doPostBack('<%=btnRefresh.UniqueID%>', '');
        }
    </script>
</body>
</html>
