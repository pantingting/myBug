<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSecMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.UserSecMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="RegionPanel1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false" BodyPadding="5px">
            <Regions>
                <c:Region runat="server" ID="Region1" Position="Top" Layout="Fit" ShowBorder="false" ShowHeader="false">
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
                                <c:FormRow runat="server" ColumnWidths="0.3 0.3 0.4">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtLoginName" Label="登录名" Readonly="true"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtDept" Label="归属部门" Readonly="true"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtEmployeeID" Label="工号" Readonly="true"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server" ColumnWidths="0.3 0.3 0.4">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtName" Label="姓名" Readonly="true"></c:TextBox>
                                        <c:DropDownList runat="server" ID="drpIsLock" Label="是否停用" Readonly="true"></c:DropDownList>
                                        <c:ContentPanel runat="server" ID="ContentPanel1" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" ID="Region2" Position="Center" Layout="Fit" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:TabStrip runat="server" ID="mainTabStrip">
                            <Tabs>
                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <c:HiddenField runat="server" ID="txtUserID"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtFromUrl"></c:HiddenField>
    <script>
        C.ready(function () {
            C('<%=cmdReturn.ClientID%>').on('click', function () {
                var fromurl = C('<%=txtFromUrl.ClientID%>').getValue();
                if (fromurl == 'USER')
                    document.location.replace("../UserAndRole/UserMaintainPage.aspx?BACK=Back");
                if (fromurl == 'SEC')
                    document.location.replace("SetSecuityMainPage.aspx?BACK=Back&SELECTTAB=USER");
            });
        });
    </script>
</body>
</html>
