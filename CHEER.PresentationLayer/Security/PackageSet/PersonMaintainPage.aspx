<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.PersonMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <c:Grid runat="server" ID="UlSecPersonGrid" IsDatabasePaging="true" EnableColumnLines="true" AllowPaging="true" ShowBorder="true" ShowHeader="false" OnPageIndexChange="UlSecPersonGrid_PageIndexChange">
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
    <c:HiddenField runat="server" ID="txtPackageID"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtRightID"></c:HiddenField>
    <c:Window runat="server" ID="MultiPersonSelect" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="550px" Width="850px" OnClose="MultiPersonSelect_Close"></c:Window>
    <script>
        C.ready(function () {
            pageload();
            C('<%=cmdAdd.ClientID%>').on('click', function () {
                return openaddpage();
            });
        });

        function pageload() {
            var packageid = C('<%=txtPackageID.ClientID%>').getValue();
            var psnadd = C('<%=cmdAdd.ClientID%>');
            var psndel = C('<%=cmdDelete.ClientID%>');
            if (packageid == '') {
                psnadd.disabled = true;
                psndel.disabled = true;
            }
            else {
                psnadd.disabled = false;
                psndel.disabled = false;
            }
        }
        function openaddpage() {
            var rightid = C('<%=txtRightID.ClientID%>').getValue();
            var packageid = C('<%=txtPackageID.ClientID%>').getValue();
            C('<%=MultiPersonSelect.ClientID%>').c_show('<%=getBaseUrl()%>Security/PackageSet/MultiPersonSelectPage.aspx?PACKAGEID=' + packageid + '&RIGHTID=' + rightid, '<%="人员选择"%>');
            return false;
        }
    </script>
</body>
</html>
