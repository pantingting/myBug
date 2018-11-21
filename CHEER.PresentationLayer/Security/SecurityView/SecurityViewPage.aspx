<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityViewPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecurityView.SecurityViewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AjaxLoadingType="Mask" ID="myPM" AutoSizePanelID="myRegionpanel" />
        <c:RegionPanel runat="server" ID="myRegionpanel">
            <Regions>
                <c:Region runat="server" ID="TreePanel" ShowBorder="false" ShowHeader="false" Split="true" Width="200px" Position="Left" Layout="Fit">
                    <Items>
                        <c:Tree runat="server" Title="功能权限查看" ShowBorder="true" ID="secviewTree" OnNodeCommand="secviewTree_NodeCommand">
                        </c:Tree>
                    </Items>
                </c:Region>
            </Regions>
            <Regions>
                <c:Region runat="server" ID="detialregion" ShowBorder="true" ShowHeader="false" Layout="Fit" RowHeight="0.5 0.5">
                    <Toolbars>
                        <c:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right">
                            <Items>
                                <c:Button runat="server" ID="btnExport" Text="EXCEL导出" Icon="PageExcel" OnClick="btnExport_Click" EnablePostBack="true"
                                    ></c:Button>
                                <c:Button runat="server" ID="btnView" Text="权限查看" Icon="ApplicationViewDetail" OnClick="btnView_Click"></c:Button>
                                <c:TextArea runat="server" ID="txtMenuIDList" Hidden="true"></c:TextArea>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Items>
                        <c:RegionPanel runat="server">
                            <Regions>
                                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit">
                                    <Items>
                                        <c:Tree runat="server" ID="detialTree" ShowHeader="false" ShowBorder="false" OnNodeCheck="secviewTree_NodeCheck" OnNodeCommand="detialTree_NodeCommand"></c:Tree>
                                        <c:Grid runat="server" Hidden="true" ID="myGrid" HideMode="Display" ShowHeader="false" ShowBorder="false"></c:Grid>
                                    </Items>
                                </c:Region>
                            </Regions>

                            <Regions>
                                <c:Region runat="server" ID="table" Hidden="true" Position="Bottom" ShowBorder="false" ShowHeader="false" ColumnWidth="0.4" BodyPadding="5px 5px 5px 5px" >
                                    <Items>
                                        <c:TextBox runat="server" Label="功能名称" ID="txtMenuName"></c:TextBox>
                                        <c:TextBox runat="server" Label="权限编号" ID="txtMenuNumber"></c:TextBox>
                                        <c:TextBox runat="server" ID="lblRightID" Label="友好名称"></c:TextBox>
                                    </Items>
                                </c:Region>
                            </Regions>

                        </c:RegionPanel>
                    </Items>
                    <Items>
                    </Items>
                </c:Region>
            </Regions>

        </c:RegionPanel>
        <c:HiddenField runat="server" ID="hfValue"></c:HiddenField>
        <br />
        <c:Window runat="server" ID="SetSecuityMainPage" Width="1200" Height="600" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" CloseAction="HidePostBack" Title="功能权限查看"></c:Window>
        <c:Window runat="server" ID="SecuritySetTab" Width="1200" Height="600" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" CloseAction="HidePostBack"></c:Window>

    </form>
    <script type="text/javascript">
        function showSecurity() {

            F('<%=SetSecuityMainPage.ClientID%>').f_show('<%=getBaseUrl()%>Security/SecurityView/SecurityViewTabPage.aspx');
        }

        function SelectCheck(noselect) {
            var selectlist = F('<%=txtMenuIDList.ClientID%>').getValue();

            if (selectlist == "") {

                F.alert(noselect);
                return false;
            }
            else {
                F.disable('<%=btnExport.ClientID%>');
                __doPostBack('<%=btnExport.UniqueID%>', '');
            }

        }
        function SelectCheck2(noselect) {
            var selectlist = F('<%=txtMenuIDList.ClientID%>').getValue();

             if (selectlist == "") {

                 F.alert(noselect);
                 return false;
             }
             else {
                 F.disable('<%=btnView.ClientID%>');
                 __doPostBack('<%=btnView.UniqueID%>', '');
             }



         }
    </script>
</body>
</html>
