<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecurityMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="mainPanel" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Panel runat="server" Layout="Fit" ID="mainPanel" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
            <Items>
                <c:Grid runat="server" ID="UlPackageGrid" IsDatabasePaging="true" EnableColumnLines="true" AllowPaging="true" ShowHeader="false" OnPageIndexChange="UlPackageGrid_PageIndexChange" OnRowCommand="UlPackageGrid_RowCommand">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="cmdAdd" Text="新增" Icon="Add"></c:Button>
                                <c:Button runat="server" ID="cmdDelete" Text="删除" Icon="Delete" OnClick="cmdDelete_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <c:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </c:ToolbarSeparator>
                        <c:ToolbarText runat="server" Text="每页记录数：">
                        </c:ToolbarText>
                        <c:DropDownList runat="server" ID="ddlPageSize" Width="80px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                            <c:ListItem Text="5" Value="5" />
                            <c:ListItem Text="10" Value="10" />
                            <c:ListItem Text="15" Value="15" />
                            <c:ListItem Text="20" Value="20" Selected="true"/>
                        </c:DropDownList>
                    </PageItems>
                </c:Grid>
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="txtManID"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtManType"></c:HiddenField>
    <c:Window runat="server" ID="SetSecuityMainPage" Width="1200" Height="600" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" CloseAction="HidePostBack" OnClose="SetSecuityMainPage_Close"></c:Window>
    <c:Window runat="server" ID="SecuritySetTab" Width="1200" Height="600" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" CloseAction="HidePostBack"></c:Window>

    <script>
        C.ready(function () {
            C('<%=cmdAdd.ClientID%>').on('click', function () {
                var mantype = C('<%=txtManType.ClientID%>').getValue();
                var manid = C('<%=txtManID.ClientID%>').getValue();
                C('<%=SetSecuityMainPage.ClientID%>').c_show('<%=getBaseUrl()%>Security/PackageSet/PackageMaintainPage.aspx?MANID=' + manid + '&MANTYPE=' + mantype, '<%=base.getString("ZGAIA00518")%>');
            });

        });
        function showDetil(packageid) {
            var mantype = C('<%=txtManType.ClientID%>').getValue();
            var manid = C('<%=txtManID.ClientID%>').getValue();
            C('<%=SetSecuityMainPage.ClientID%>').c_show('<%=getBaseUrl()%>Security/PackageSet/PackageMaintainPage.aspx?MANID=' + manid + '&MANTYPE=' + mantype + '&PACKAGEID=' + packageid, '<%=base.getString("ZGAIA00518")%>');
        }
        function showSecurity(packageid) {
            var manid = C('<%=txtManID.ClientID%>').getValue();
            var mantype = C('<%=txtManType.ClientID%>').getValue();
            C('<%=SetSecuityMainPage.ClientID%>').c_show('<%=getBaseUrl()%>Security/SecuritySet/SecuritySetTabPage.aspx?MANID=' + manid + '&MANTYPE=' + mantype + '&PACKAGEID=' + packageid, '<%="功能权限维护"%>');
        }
    </script>
</body>
</html>
