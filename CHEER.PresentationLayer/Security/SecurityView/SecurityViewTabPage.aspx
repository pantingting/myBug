<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityViewTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecurityView.SecurityViewTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server" >
   <c:PageManager runat="server"  AutoSizePanelID="myTabPanel"/>
        <c:Panel runat="server" ShowHeader="false" ID="myTabPanel" ShowBorder="false" Layout="Fit">
            
            <Items>
                
                <c:TabStrip runat="server" ID="myTab">
                    <Tabs>

                    </Tabs>
                </c:TabStrip>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
