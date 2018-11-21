<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlotSeletlPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.AlotSeletlPage" %>

<%@ Register Src="~/Controls/UserQuery.ascx" TagPrefix="uc1" TagName="UserQuery" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="下一步" Icon="BookNext" OnClick="cmdAdd_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Regions>
                <c:Region runat="server" Position="Top" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="5px">
                    <Items>
                        <c:UserControlConnector runat="server">
                            <uc1:UserQuery runat="server" ID="UserQuery" />
                        </c:UserControlConnector>
                    </Items>
                </c:Region>
                <c:Region runat="server" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="5px">
                    <Items>
                        <c:Grid runat="server" ID="UltraBaseInfo" OnRowDataBound="UltraBaseInfo_RowDataBound" AllowPaging="true" ShowBorder="false" CssStyle="border-top:#727272 1px solid" EnableRowDoubleClickEvent="true" ShowHeader="false" IsDatabasePaging="true" EnableColumnLines="true" EnableMultiSelect="false" ShowGridHeader="true" OnPageIndexChange="UltraBaseInfo_PageIndexChange">
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
