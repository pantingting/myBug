<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.RoleMaintainPage" %>
<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="pageManager" AjaxLoadingType="Mask" AutoSizePanelID="mainRegion"/>
        <c:RegionPanel runat="server" BodyPadding="5px" ID="mainRegion" ShowBorder="false">
            <Regions>
                <c:Region ID="formRegion" runat="server" Position="Top" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:Form runat="server" ShowHeader="false" BodyPadding="5px 5px 0 5px">
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Button runat="server" ID="btnAdd" Text="新增" IconFont="Plus"></c:Button>
                                        <c:Button runat="server" ID="btnDelete" Text="删除" IconFont="Remove" OnClick="btnDelete_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="33% 67% 72px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                        <c:UserControlConnector runat="server">
                                            <uc1:StdBranchLoader runat="server" Label="归属组织" ID="StdBranchLoader" IsHaveManageUnit="true" IsShowLabel="true" IsIncludeSubBranchCanEdit="true" IsIncludeSubBranch="true" IsShowIncludeStopUnit="false"/>
                                        </c:UserControlConnector>
                                         <c:Button runat="server" ID="btnQuery" Text="查询" IconFont="Search" OnClick="btnQuery_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region ID="gridRegion" runat="server" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:Grid OnRowCommand="grdMain_RowCommand" CssStyle="border-top:none !important;" runat="server" ShowBorder="true" ShowHeader="false" ID="grdMain" EnableColumnLines="true">
                            
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
        <c:Window runat="server" WindowPosition="Center" Target="Top" CloseAction="HidePostBack" OnClose="detailWindow_Close" ID="detailWindow" Hidden="true" EnableIFrame="true" Width="650" Height="450">

        </c:Window>
    </form>
    <script>
        function openaddpage() {
            C('<%= detailWindow.ClientID%>').c_show('<%=getBaseUrl()%>Security/UserAndRole/RoleTabPage.aspx?ISADD=YES');
        }
    </script>
</body>
</html>
