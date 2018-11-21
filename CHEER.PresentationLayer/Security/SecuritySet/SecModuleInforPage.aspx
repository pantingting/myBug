<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecModuleInforPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecModuleInforPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../script/mergeCells.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <c:Grid runat="server" ID="UlFieldSetGrid" ShowBorder="false" ShowHeader="false" EnableColumnLines="true" OnRowCommand="UlFieldSetGrid_RowCommand">
                </c:Grid>
            </Items>
        </c:Panel>

    </form>
    <c:HiddenField runat="server" ID="txtManID"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtManType"></c:HiddenField>
    <c:Window runat="server" ID="windowMain" Width="600" Height="400" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" CloseAction="HidePostBack"></c:Window>
    <script>
        Ext.onReady(function () {
            mergeCells(F('<%=UlFieldSetGrid.ClientID%>'), [1]);
        });

       
        function showField(funid, menuid) {
            var mantype = F('<%=txtManType.ClientID%>').getValue();
            var manid = F('<%=txtManID.ClientID%>').getValue();
            F('<%=windowMain.ClientID%>').f_show('<%=getBaseUrl()%>Security/SecuritySet/SecFieldInforPage.aspx?FUNPOINTID=' + funid + '&MANID=' + manid + '&MANTYPE=' + mantype, '<%=base.getString("ZGAIA00408")%>');
        }
        function showSecurity(funid, menuid) {
            var mantype = F('<%=txtManType.ClientID%>').getValue();
            var manid = F('<%=txtManID.ClientID%>').getValue();
            var secfunid = funid;
            if (funid == null || funid == '')
                secfunid = menuid;
            F('<%=windowMain.ClientID%>').f_show('<%=getBaseUrl()%>Security/SecuritySet/SecFunInforPage.aspx?FUNPOINTID=' + secfunid + '&MANID=' + manid + '&MANTYPE=' + mantype, '<%=base.getString("ZGAIA00406")%>');
        }
    </script>
</body>
</html>
