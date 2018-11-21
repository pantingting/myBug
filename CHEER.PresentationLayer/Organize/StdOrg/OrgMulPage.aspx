<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgMulPage.aspx.cs" Inherits="CHEER.PresentationLayer.Organize.StdOrg.OrgMulPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="panelMain" AjaxLoadingType="Mask" />
        <c:Panel runat="server" ShowBorder="false" ShowHeader="false" ID="panelMain" Layout="Fit" BodyPadding="5px">
            <Items>
                <c:Grid EnableMultiSelect="true" AllowPaging="true" EnableColumnLines="true" OnPageIndexChange="grdMain_PageIndexChange" runat="server" ShowHeader="false" ID="grdMain">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button IconFont="Plus" runat="server" ID="btnAdd" Text="新增" OnClientClick="return showAddDialog();"></c:Button>
                                <c:Button IconFont="Remove" runat="server" ID="btnDelete" Text="删除" OnClick="btnDelete_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                </c:Grid>
            </Items>
        </c:Panel>
        <c:Window runat="server" WindowPosition="Center" Target="Top" ID="detailWindow" EnableIFrame="true" IFrameUrl="" Hidden="true" Height="400px" Width="400px" OnClose="detailWindow_Close" CloseAction="HidePostBack"></c:Window>
    </form>
    <script>
        function showAddDialog() {
            C('<%=detailWindow.ClientID%>').c_show('<%=getBaseUrl()%>Organize/StdOrg/MulAddPage.aspx?UNITID=<%=UNITID%>', '<%=base.getString("ZGAIA00826")%>');
            return false;
        }
    </script>
</body>
</html>
