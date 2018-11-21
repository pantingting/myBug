<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSelectPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SuperWork.UserSelectPage" %>

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
        <c:RegionPanel BodyPadding="5px" runat="server" ShowBorder="false" ID="RegionPanel1">
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Top">
                    <Items>
                        <c:Form BoxConfigPosition="Start" BodyPadding="5px" ID="Form2" runat="server" LabelWidth="80" ShowHeader="false">
                            <Rows>
                                <c:FormRow ColumnWidths="50% 50%">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtLoginName" Label="登录名"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtEmployeeid" Label="工号"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="50% 50%">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtName" Label="姓名"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="角色名称"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="50% 50%">
                                    <Items>
                                        <c:DropDownList runat="server" ID="drpAccState" Label="在职状态"></c:DropDownList>
                                        <c:DropDownList runat="server" ID="drpLock" Label="是否停用"></c:DropDownList>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="100% 60px">
                                    <Items>
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
                        <c:Grid AllowPaging="true" EnableRowDoubleClickEvent="true" OnRowDoubleClick="grdMain_RowDoubleClick" IsDatabasePaging="true" OnPageIndexChange="grdMain_PageIndexChange" ID="grdMain" EnableColumnLines="true" EnableMultiSelect="false" ShowGridHeader="true" runat="server" ShowHeader="false">
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
    </form>
</body>
</html>
