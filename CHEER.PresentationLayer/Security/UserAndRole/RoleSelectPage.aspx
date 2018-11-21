<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSelectPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.RoleSelectPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager" AutoSizePanelID="RegionPanel1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false" BodyPadding="5px">
            <Regions>
                <c:Region runat="server" ID="Region1" Layout="Fit" Position="Top" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:Form runat="server" BodyPadding="5px" ShowHeader="false">
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Button runat="server" ID="btnConfirm" Text="确定" IconFont="CheckCircle" OnClick="btnConfirm_Click"></c:Button>
                                        <c:Button runat="server" ID="btnCancle" Text="取消" IconFont="Remove" OnClick="btnCancle_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="89% 11%">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                        <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1px"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server">
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow runat="server" ColumnWidths="89% 11%">
                                                    <Items>
                                                        <c:UserControlConnector ID="UserControlConnector1" runat="server">
                                                            <uc1:StdBranchLoader runat="server" IsShowLabel="true" ID="StdBranchLoader" />
                                                        </c:UserControlConnector>
                                                        <c:Button runat="server" ID="btnSearch" Text="查询" Icon="SystemSearch" OnClick="btnSearch_Click"></c:Button>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region ID="Region2" runat="server" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit">
                    <Items>
                        <c:Grid AllowPaging="true" CssStyle="border-top:none;" ID="UlRoleGrid" EnableColumnLines="true" runat="server" ShowBorder="true" ShowHeader="false" IsDatabasePaging="true" OnPageIndexChange="UlRoleGrid_PageIndexChange">
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
</body>
</html>
