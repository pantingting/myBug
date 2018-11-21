<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityViewUserPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecurityView.SecurityViewUserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../../script/mergeCells.js"></script>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="mYpanel2" />
        <c:Panel runat="server" ID="mYpanel2" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <c:Grid runat="server" ShowBorder="false" EnableColumnLines="true" ShowHeader="false" ID="mYGrid" OnPageIndexChange="mYGrid_PageIndexChange" AllowPaging="true">
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
    <script>

        Ext.onReady(function () {
            mergeCells(F('<%=mYGrid.ClientID%>'), [1]);
        });
    </script>
    <script type="text/javascript">
        function mer() {
            mergeCells(F('<%=mYGrid.ClientID%>'), [1])
           }
    </script>
</body>
</html>
