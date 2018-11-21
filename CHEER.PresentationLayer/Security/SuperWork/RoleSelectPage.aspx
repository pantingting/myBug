<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSelectPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SuperWork.RoleSelectPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" BodyPadding="5px" ShowBorder="false" ID="RegionPanel1">
            <Regions>
                <c:Region runat="server" Position="Top" ShowBorder="false" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:Form runat="server" ShowHeader="false" BodyPadding="5px">
                            <Rows>
                                <c:FormRow ColumnWidths="0.4 0.6 60">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                        <c:UserControlConnector runat="server" ID="UserControlConnector1">
                                            <uc1:StdBranchLoader runat="server" ID="StdBranchLoader" />
                                        </c:UserControlConnector>
                                        <c:Button runat="server" ID="cmdSearch" Text="查询" OnClick="cmdSearch_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit">
                    <Items>
                        <c:Grid AllowPaging="true" CssStyle="border-top:none !important;" EnableRowDoubleClickEvent="true" OnRowDoubleClick="grdMain_RowDoubleClick" IsDatabasePaging="true" OnPageIndexChange="grdMain_PageIndexChange" ID="grdMain" EnableColumnLines="true" EnableMultiSelect="false" ShowGridHeader="true" runat="server" ShowHeader="false">
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
            </Regions>
        </c:RegionPanel>

    </form>
</body>
</html>
