<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSelectPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.UserSelectPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="pageManager" AutoSizePanelID="mainRegion" />
        <c:RegionPanel runat="server" ID="mainRegion" ShowBorder="false">
            <Toolbars>
                <c:Toolbar runat="server" ID="toolBar">
                    <Items>
                        <c:Button runat="server" ID="btnSave" IconFont="Save" Text="保存" OnClick="btnSave_Click"></c:Button>
                        <c:Button runat="server" ID="btnClose" IconFont="Remove" Text="关闭" EnablePostBack="false"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Regions>
                <c:Region Position="Top" RegionPosition="Top" runat="server" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:Form runat="server" ID="mainForm" ShowBorder="false" ShowHeader="false" BodyPadding="5px 5px 0 5px">
                            <Rows>
                                <c:FormRow ColumnWidths="50% 50% 80px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtLoginName" Label="登录名"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtEmployeeid" Label="工号"></c:TextBox>
                                        <c:ContentPanel runat="server" Height="1px" ShowBorder="false" ShowHeader="false"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="50% 50% 80px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtName" Label="姓名"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                        <c:ContentPanel runat="server" Height="1px" ShowBorder="false" ShowHeader="false"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="50% 50% 80px">
                                    <Items>
                                        <c:DropDownList runat="server" ID="drpAccState" Label="在职状态"></c:DropDownList>
                                        <c:DropDownList runat="server" ID="drpLock" Label="是否停用"></c:DropDownList>
                                        <c:ContentPanel runat="server" Height="1px" ShowBorder="false" ShowHeader="false"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="100% 80px">
                                    <Items>
                                        <c:UserControlConnector runat="server">
                                            <uc1:StdBranchLoader runat="server" Label="归属组织" ID="StdBranchLoader" IsHaveManageUnit="true" IsShowLabel="true" IsIncludeSubBranchCanEdit="true" IsIncludeSubBranch="true" IsShowIncludeSubBranch="false" IsShowIncludeStopUnit="false" />
                                        </c:UserControlConnector>
                                        <c:Button runat="server" Width="76px" ID="btnQuery" Icon="SystemSearch" Text="查询" OnClick="btnQuery_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" Layout="Fit" Position="Center" RegionPosition="Center" CssStyle="border-top:1px solid #8a8a8a;" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:Grid EnableColumnLines="true" runat="server" ID="UlUserGrid" OnPageIndexChange="UlUserGrid_PageIndexChange" ShowBorder="false" ShowHeader="false" AllowPaging="true">
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
                                    <c:ListItem Text="20" Value="20" />
                                </c:DropDownList>
                            </PageItems>
                        </c:Grid>
                    </Items>
                </c:Region>
                <c:Region runat="server" Height="5px" RegionPosition="Bottom" Position="Bottom" ShowBorder="false" ShowHeader="false">
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
</body>
</html>
