<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.UserMaintainPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>


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
                <c:Region runat="server" Position="Top" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px 5px 0 5px">
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Button runat="server" ID="btnAdd" Text="新增" IconFont="Plus"></c:Button>
                                        <c:Button runat="server" ID="btnDelete" Text="删除" IconFont="Remove" OnClick="btnDelete_Click" EnablePostBack="true"></c:Button>
                                        <c:Button runat="server" ID="btnExport" Text="导出" IconFont="FileExcelO" OnClick="btnExport_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="33% 67% 72px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtLoginName" Label="登录名"></c:TextBox>
                                        <c:UserControlConnector runat="server">
                                            <uc1:StdBranchLoader runat="server" ID="StdBranchLoader" IsHaveManageUnit="true" IsShowLabel="true" IsIncludeSubBranchCanEdit="true" IsIncludeSubBranch="true" IsShowIncludeStopUnit="false" />
                                        </c:UserControlConnector>
                                        <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1px"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server" ColumnWidths="33% 33% 34% 72px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtName" Label="姓名"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtEmployeeid" Label="工号"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                        <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1px">
                                        </c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server">
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow runat="server" ColumnWidths="33% 33% 34% 72px">
                                                    <Items>
                                                        <c:DropDownList runat="server" ID="drpAccStates" Label="在职状态"></c:DropDownList>
                                                        <c:DropDownList runat="server" ID="drpLock" Label="是否停用"></c:DropDownList>
                                                        <c:ContentPanel ID="ContentPanel1" runat="server" ShowBorder="false" ShowHeader="false" Height="1px"></c:ContentPanel>
                                                        <c:Button runat="server" ID="btnSearch" Text="查询" IconFont="Search" OnClick="btnSearch_Click"></c:Button>
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
                <c:Region runat="server" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:Grid OnRowCommand="UlUserGrid_RowCommand" CssStyle="border-top:none !important;" runat="server" IsDatabasePaging="true" EnableColumnLines="true" AllowPaging="true" ShowHeader="false" ID="UlUserGrid" OnPageIndexChange="UlUserGrid_PageIndexChange">
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
                </c:Region>
            </Regions>
        </c:RegionPanel>
        <c:Window runat="server" ID="UserTabPage" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="450px" Width="650px" CloseAction="HidePostBack"></c:Window>
    </form>
    <script>
        function openaddpage() {
            C('<%= UserTabPage.ClientID%>').c_show('<%=getBaseUrl()%>Security/UserAndRole/UserTabPage.aspx?ISADD=YES', '<%=base.getString("ZGAIA00256")%>');
        }
    </script>
</body>
</html>
