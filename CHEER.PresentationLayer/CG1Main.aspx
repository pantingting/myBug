<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CG1Main.aspx.cs" Inherits="CHEER.PresentationLayer.CG1Main" %>

<!DOCTYPE html>
<html>
<head id="he" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MyBug</title>
    <link rel="Shortcut Icon" href="favicon.ico">
    <link rel="stylesheet" href="res/css/main.css" />

</head>
<body>
   <form id="form1" runat="server">
        <c:PageManager ID="pageManager" AutoSizePanelID="regionPanel" runat="server" EnableAjax="true" AjaxLoadingType="Mask" />
        <c:RegionPanel runat="server" ID="regionPanel" CssStyle="background: url(image/head_bg_line.gif) repeat-x left top #e5edef;" ShowBorder="false" BoxConfigChildMargin="5px">
            <Regions>
                <c:Region Layout="HBox" ID="regionNavigator" CssClass="regionPanel" runat="server" ShowBorder="false" ShowHeader="false" Position="Top" Height="50px" EnableCollapse="false">
                    <Content>
                        <div style="width: 100%">
                            <div style="width: 100%; position: absolute; z-index: 9999; background-color: #203854; height: 50px; position: relative">
                                <div style="width: 10%; float: left; height: 48px; cursor: pointer;">
                                    <img src="image/akso.png" style="margin-top: 12px; margin-left: 10px;height:25px;display:none;" id="imgHome" />
                                </div>

                                <div style="width: 150px; float: right; text-align: right; margin-right: 0px;">
                                    <div title="用户名:<%= InfomationPackage.UserName %>&#10所属部门:<%= BRANCHNAME %>" style="text-align: center; width: 95px; height: 33px; float: left; color: #bacce3; font: 7px; margin-top: 16.5px; cursor: pointer;">
                                        <%= USERNAME %>
                                    </div>

                                    <div style="background-color: #37506d; width: 1px; height: 12px; float: left; margin-top: 18px;">
                                    </div>
                                    <div style="font-size: 12px; text-align: center; width: 54px; height: 50px; float: left;">
                                        <div style="padding-top: 15px; text-align: center; color: #bacce3">
                                            <a id="imgExit" style="color: Black;"><span style="color: #bacce3; cursor: pointer;">退出</span></a>
                                        </div>
                                    </div>
                                </div>
                                <div style="width: auto; float: right; text-align: left;">
                                    <ul id="nav">
                                        <% menus.ForEach(menu =>
                                            { %>
                                        <li style="height: 50px; width: 90px; cursor: pointer">
                                            <a>
                                                <div style="padding-top: 12px; height: 38px; text-align: center;">
                                                    <div class="menuHover" style="border: 1px solid #203854; width: 90px; height: 24px; border-radius: 4px; margin: 0 auto">
                                                        <%= menu.text %>
                                                    </div>
                                                </div>
                                            </a>
                                            <ul style="border-bottom: 3px solid #e5e0e0;">
                                                <li style="height: 40px; width: 70px; cursor: pointer">
                                                    <ul style="border-bottom: 3px solid #e5e0e0;">
                                                        <% menu.subMenus.ForEach(subMenu =>
                                                            { %>
                                                        <li style="border-right: 3px solid #e5e0e0;">
                                                            <a href="javascript:addTabByUrl('<%= subMenu.id %>','<%= subMenu.text %>','<%= subMenu.url %>')"><%= subMenu.text %></a>
                                                        </li>
                                                        <% }); %>
                                                </li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </li>
                                        <li style="width: 1px;"></li>
                                        <% }); %>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </Content>
                </c:Region>
                <c:Region ID="regionTab" Layout="Fit" ShowBorder="false" ShowHeader="false" runat="server" Position="Center" EnableCollapse="false">
                    <Items>
                        <c:TabStrip ID="mainTabs" ShowBorder="true" runat="server" EnableTabCloseMenu="true">
                            <Tabs>
                                <c:Tab IconFont="Home" ID="tabHome" Title="首页" EnableClose="false" runat="server" EnableIFrame="true" IFrameUrl="MainPage.aspx">
                                </c:Tab>
                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
        <c:Window runat="server" Width="380px" Height="302px" CloseAction="Hide" EnableClose="false" Hidden="true" EnableDrag="false" IFrameUrl="~/Default.aspx?ISOPENED=ISOPENED" EnableIFrame="true" ID="reloginWindow" Title="登录" IconFont="Lock">
        </c:Window>
    </form>
    <script>
        C.ready(function () {
            $('#welcomeInfo').html('<%=USERNAME%>');

            $('#imgExit').click(function () {
                C.util.confirm('确认退出吗?', '提示', 'information', function () {
                    window.location.href = "<%= ResolveUrl("~/Default.aspx") %>";
                }, '');
            })

            $('#imgSet').click(function () {
                __doPostBack('<%=pageManager.ClientID%>', 'changeMenu');
            })

            $('#imgHome').click(function () {
                C('<%=mainTabs.ClientID%>').setActiveTab(0);
            })

            $('.menuHover').hover(function () {
                $(this).css('border', '1px solid #fff');
            }, function () {
                $(this).css('border', '1px solid #203854');
            });
        });

        function showReloginWindow() {
            C('<%=reloginWindow.ClientID%>').c_show();
        }

        function addTabByUrl(tabId, text, url) {
            var currentTab = C('<%=mainTabs.ClientID%>').getTab(tabId);
            currentTab && C('<%=mainTabs.ClientID%>').setActiveTab(currentTab);
            !currentTab && addTab(tabId, url, text, null, false, false);
        }

        var removeTab = function (tabId) {
            var currentTab = C('<%=mainTabs.ClientID%>').getTab('<%=mainTabs.ClientID%>_' + tabId);
            currentTab && C('regionPanel_regionTab_mainTabs').removeTab(currentTab) && cheerHash.remove(url);
        }

        var refreshTab = function (tabId) {
            var currentTab = C('<%=mainTabs.ClientID%>').getTab('<%=mainTabs.ClientID%>_' + tabId);
            currentTab && currentTab.refreshIFrame();
        }

        var doPostTab = function (tabId, doRefresh) {
            var currentTab = C('<%=mainTabs.ClientID%>').getTab('<%=mainTabs.ClientID%>_' + tabId);
            setTimeout(function () {
                currentTab && currentTab.getIFrameWindow() && currentTab.getIFrameWindow().eval(doRefresh)();
            });
        }

        function addTab(id, iframeUrl, title, icon, createToolbar, refreshWhenExist) {
            var mainTabStrip = C('<%=mainTabs.ClientID%>');

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

