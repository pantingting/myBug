<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiPersonSelectPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.MultiPersonSelectPage" %>

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
        <c:RegionPanel BodyPadding="5px" runat="server" ID="RegionPanel1" ShowBorder="false">
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" Position="Top">
                    <Items>
                        <c:Form runat="server" ShowBorder="true" ShowHeader="false" BodyPadding="5px">
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Button runat="server" ID="cmdOK" Text="确定" Icon="Accept" OnClick="cmdOK_Click"></c:Button>
                                        <c:Button runat="server" ID="cmdCancel" Text="取消" Icon="Cancel" OnClick="cmdCancel_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="0.5 0.5 72px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtWno" Label="工号"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtName" Label="姓名"></c:TextBox>
                                        <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow runat="server" ColumnWidths="0.5 0.5 72px">
                                                    <Items>
                                                        <c:DropDownList runat="server" ID="dltSex" Label="性别"></c:DropDownList>
                                                        <c:DropDownList runat="server" ID="ddlState" Label="在职状态"></c:DropDownList>
                                                        <c:ContentPanel ID="ContentPanel1" runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>

                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server" ColumnWidths="100% 72px">
                                    <Items>
                                        <c:UserControlConnector runat="server">
                                            <uc1:StdBranchLoader runat="server" ID="StdBranchLoader"/>
                                        </c:UserControlConnector>
                                        <c:Button runat="server" Icon="SystemSearch" ID="cmdSeach" Text="查询" OnClick="cmdSeach_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
            </Regions>
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" Position="Center">
                    <Items>
                        <c:Grid　CssStyle="border-top:none !important;" runat="server" ID="Grd" IsDatabasePaging="true" EnableColumnLines="true" AllowPaging="true" ShowHeader="false" OnPageIndexChange="Grd_PageIndexChange">
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
