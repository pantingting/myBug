<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BchExtendSelectPage.aspx.cs" Inherits="HRONE.PresentationLayer.Security.PackageSet.BchExtendSelectPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="mainPanel" />
        <c:Panel runat="server" ID="mainPanel" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdConfirm" Text="确定" IconFont="Save" OnClick="cmdConfirm_Click"></c:Button>
                        <c:Button runat="server" ID="cmdReturn" Text="返回" IconFont="Close" OnClick="cmdReturn_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:Grid ShowGridHeader="false" ShowBorder="true" ID="grdMain" Title="请选中需要选择的,可以多选" runat="server">
                    <Columns>
                        <c:BoundField runat="server" ColumnID="KEY" DataField="KEY" Hidden="true"></c:BoundField>
                        <c:BoundField runat="server" BoxFlex="1" DataField="VALUE" ColumnID="VALUE"></c:BoundField>
                    </Columns>
                </c:Grid>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
