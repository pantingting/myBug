<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CHEER.PresentationLayer.Index" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>MyBug</title>
    <link rel="shortcut icon" type="image/x-Icon" href="favicon.ico" />
</head>
<body class="defaultpage">
    <form id="form1" runat="server">
        <c:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></c:PageManager>
        <c:Panel ID="Panel1" Layout="Region" CssClass="mainpanel" ShowBorder="false" ShowHeader="false" runat="server">
            <Items>
                <c:ContentPanel ID="topPanel" CssClass="topregion bgpanel" RegionPosition="Top" Height="50px" ShowBorder="false" ShowHeader="false" EnableCollapse="true" runat="server">
                    <div id="header" class="ui-widget-header c-mainheader">
                        <table>
                            <tr>
                                <td>
                                    <asp:Image runat="server" ImageUrl="~/image/akso.png" CssClass="icononlyaction" Height="28px" Style="margin: 11px; cursor: pointer;display:none;" onclick="doLogoClick();" />
                                </td>
                                <td style="text-align: right;">
                                    <c:Button ID="userName" runat="server" CssClass="userpicaction" EnablePress="false"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false">
                                    </c:Button>
                                    <div class="c-inline-block" style="background-color: #c2d4e8; width: 1px; height: 15px; top: 5px; position: relative;">
                                    </div>
                                    <c:Button runat="server" CssClass="userpicaction" ID="btnLogout" Text="退出"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false">
                                        <Listeners>
                                            <c:Listener Event="click" Handler="doLogoutClick" />
                                        </Listeners>
                                    </c:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </c:ContentPanel>
                <c:Panel ID="leftPanel" CssClass="leftregion bgpanel"
                    RegionPosition="Left" RegionSplit="true" RegionSplitWidth="2px" RegionSplitIcon="false"
                    ShowBorder="true" Width="220px" ShowHeader="true" Title="菜单列表" MinWidth="220px"
                    EnableCollapse="false" Collapsed="false" Layout="Fit" runat="server">
                    <Items>
                        <c:Tree runat="server" ID="treeMenu" ShowBorder="false" ShowHeader="false" EnableIcons="true" AutoScroll="true"
                            EnableSingleClickExpand="true" HideHScrollbar="true" HideVScrollbar="true" ExpanderToRight="true" HeaderStyle="true">
                        </c:Tree>
                    </Items>
                </c:Panel>
                <c:TabStrip ID="mainTabStrip" CssClass="centerregion" RegionPosition="Center" ShowBorder="true" EnableTabCloseMenu="true" runat="server">
                    <Tabs>
                        <c:Tab ID="Tab1" Title="首页" IconFont="Home" EnableIFrame="true" IFrameUrl="~/MainPage.aspx?id=mainTabStrip" runat="server">
                        </c:Tab>
                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>

        <asp:XmlDataSource ID="XmlDataSource1" runat="server" EnableCaching="false" DataFile="~/menu.xml"></asp:XmlDataSource>

    </form>

    <style>
        #header table {
            width: 100%;
            border-spacing: 0;
            border-collapse: separate;
        }

            #header table td {
                padding: 0;
            }

        #header .logo {
            font-size: 24px;
            font-weight: bold;
            text-decoration: none;
            display: inline-block;
            vertical-align: middle;
        }



        #header .c-btn {
            border-width: 0;
            padding: 4px 10px;
        }

        #header .icontopaction .c-btn-icon {
            width: 22px;
            font-size: 22px;
            line-height: 22px;
            height: 22px;
        }

        #header .icontopaction .c-btn-text {
            font-size: 12px;
            line-height: 16px;
            margin-top: 4px;
        }


        #header .icononlyaction .c-btn-icon,
        #header .icononlyaction .c-btn-text {
            font-size: 30px;
            line-height: 42px;
            height: 42px;
        }


        #header .userpicaction .c-btn-icon {
            border-radius: 50%;
            width: 42px;
            height: 42px;
            margin: 0;
        }

        #header .userpicaction .c-btn-text {
            font-size: 14px;
            line-height: 42px;
        }

        #header .userpicaction:hover {
            background-color: transparent;
        }

        #header .userpicaction {
            background-color: transparent;
        }

        ul.list {
            list-style-type: none;
            padding: 0;
            margin: 0;
        }

            ul.list li {
                margin-bottom: 5px;
            }

        .bottomtable {
            width: 100%;
            font-size: 12px;
        }


        .tabtool.viewcode .ui-icon {
            font-weight: bold;
        }


        ul.applytrial {
            list-style-type: none;
            margin: 10px 0 0 10px;
            border-left-width: 1px;
            border-left-style: dashed;
            padding-left: 15px;
        }

            ul.applytrial li {
                padding: 1px 0;
            }
    </style>

    <script>
        var leftPanelClientID = '<%= leftPanel.ClientID %>';
        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';
        function doLogoutClick() {
            C.util.confirm('确认退出吗?', '提示', 'information', function () {
                window.location.href = "<%= ResolveUrl("~/Default.aspx") %>";
            }, '');
        }

        C.ready(function () {
            C('<%= treeMenu.ClientID %>').on('nodeclick', function (event, nodeId) {
                var nodeValue = C('<%= treeMenu.ClientID %>').getNodeData(nodeId);
                if (nodeValue.attrs.url) {
                    addTabByUrl(nodeValue.id, nodeValue.text, nodeValue.attrs.url);
                }
            });
        })

        function doLogoClick() {
            C(mainTabStripClientID).setActiveTab(0);
        }

        function addTabByUrl(tabId, text, url) {
            var currentTab = C(mainTabStripClientID).getTab(tabId);
            currentTab && C(mainTabStripClientID).setActiveTab(currentTab);
            !currentTab && addTab(tabId, url, text, null, false, false);
        }

        function removeTab(tabId) {
            var currentTab = C(mainTabStripClientID).getTab(mainTabStripClientID + tabId);
            currentTab && C('regionPanel_regionTab_mainTabs').removeTab(currentTab) && cheerHash.remove(url);
        }

        function refreshTab(tabId) {
            var currentTab = C(mainTabStripClientID).getTab(mainTabStripClientID + tabId);
            currentTab && currentTab.refreshIFrame();
        }

        function doPostTab(tabId, doRefresh) {
            var currentTab = C(mainTabStripClientID).getTab(mainTabStripClientID + tabId);
            setTimeout(function () {
                currentTab && currentTab.getIFrameWindow() && currentTab.getIFrameWindow().eval(doRefresh)();
            });
        }

        function addTab(id, iframeUrl, title, icon, createToolbar, refreshWhenExist) {
            var mainTabStrip = C(mainTabStripClientID);

            if (typeof (id) === 'string') {
                tabOptions = {
                    id: id,
                    iframeUrl: iframeUrl,
                    title: title,
                    icon: icon,
                    createToolbar: createToolbar,
                    refreshWhenExist: refreshWhenExist
                };
            }

            C.util.addMainTab(mainTabStrip, tabOptions);
        }
    </script>
</body>
</html>
