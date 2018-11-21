<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchDisTree.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.BranchDisTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Panel BodyPadding="5px" runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <c:Tree runat="server" ID="UltraDepTree" ShowHeader="false">
                </c:Tree>
            </Items>
        </c:Panel>
    </form>
    <script>
        C.ready(function () {
            C('<%=UltraDepTree.ClientID%>').on('nodeclick', function (event, node) {
                node = this.getNodeData(node);
                if (node.text != null && node.text != "") {
                    var nodetag = node.tag;
                    var unitinfor = nodetag.split('*|*');
                    var unitcode = unitinfor[0];
                    var unitid = unitinfor[3];
                    var unitsec = unitinfor[4];
                    //alert(unitinfor[0]);
                    parent.setUnitCode(unitcode, unitid, unitsec);
                }
            });
        });
    </script>
</body>
</html>
