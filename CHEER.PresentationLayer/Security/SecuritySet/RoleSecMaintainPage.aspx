<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSecMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.RoleSecMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" BodyPadding="5px" ID="RegionPanel1" ShowBorder="false">
            <Regions>
                <c:Region runat="server" Position="Top" ShowBorder="false" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:Form runat="server" ShowHeader="false" BodyPadding="5px">
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Button runat="server" ID="cmdReturn" Text="返回" Icon="PageBack"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtDept" Label="归属部门"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:TabStrip CssStyle="border-top:none !important;" runat="server" ID="UlRoleSecTab">
                            <Tabs>
                                <c:Tab runat="server" ShowHeader="false">
                                    <Items></Items>
                                </c:Tab>
                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
        <c:HiddenField runat="server" ID="txtRoleID"></c:HiddenField>
        <c:HiddenField runat="server" ID="txtFromUrl"></c:HiddenField>
    </form>
    <script>
        C.ready(function () {
            C('<%=cmdReturn.ClientID%>').on('click', function () {
                var fromurl = C('<%=txtFromUrl.ClientID%>').getValue();
                if (fromurl == 'ROLE')
                    document.location.replace("../UserAndRole/RoleMaintainPage.aspx?BACK=Back");
                if (fromurl == 'SEC')
                    document.location.replace("SetSecuityMainPage.aspx?BACK=Back&SELECTTAB=ROLE");
            });
        });
    </script>
</body>
</html>
