<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchMaintainpage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.BranchMaintainpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel1" FormMessageTarget="Qtip" AjaxLoadingType="Mask"/>
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <c:TabStrip runat="server" ID="UlPsnRelateTab" >
                    <Tabs>
                        
                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
