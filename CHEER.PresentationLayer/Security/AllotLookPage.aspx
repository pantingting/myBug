<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllotLookPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.AllotLookPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="导出" Icon="FolderExplore" OnClick="cmdAdd_Click"></c:Button>
                        <c:Button runat="server" ID="cmdDelete" Text="返回" Icon="PageBack" OnClick="cmdDelete_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:Grid runat="server" ID="UlRoleGrid" ShowBorder="false" ShowHeader="false" EnableColumnLines="true" AllowPaging="true" OnPageIndexChange="UlRoleGrid_PageIndexChange">
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
        </c:Panel>
    </form>
</body>
</html>
