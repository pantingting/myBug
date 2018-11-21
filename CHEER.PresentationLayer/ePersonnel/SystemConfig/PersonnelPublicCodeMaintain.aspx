<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonnelPublicCodeMaintain.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.SystemConfig.PersonnelPublicCodeMaintain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
        <c:RegionPanel ID="RegionPanel1" BodyPadding="5px" ShowBorder="false" runat="server">
            <Regions>
                <c:Region ID="Region2" ShowHeader="false" Width="220px" ShowBorder="true" EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                    <Items>
                        <c:Tree ShowBorder="false" ID="FTreePublicCode" ShowHeader="false" Title="公用代码维护" runat="server">
                        </c:Tree>
                    </Items>
                </c:Region>
                <c:Region ID="Region1" runat="server" Layout="Fit" EnableCollapse="false" ShowBorder="false" ShowHeader="false" RegionPosition="Center">
                    <Items>
                        <c:Grid DataKeyNames="PSNPUBLICCODEITEM_ITEMID" CssStyle="border-left:none !important;" KeepCurrentSelection="false" AjaxPostBackLoadData="true" EnableColumnLines="true" ID="FGridPublicCode" ShowBorder="true" EnableMultiSelect="true" ShowHeader="false" Title="表格" EnableFrame="false" runat="server" ClicksToEdit="2" AllowCellEditing="true" EnableCheckBoxSelect="true">
                            <Toolbars>
                                <c:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <c:Button ID="btnAdd" runat="server" Text="新增" IconFont="Plus" OnClick="btnAdd_Click"></c:Button>
                                        <c:Button ID="btnAddNewRow" runat="server" Text="新增空行" IconFont="Plus" EnablePostBack="false"></c:Button>
                                        <c:Button ID="btnSave" runat="server" Text="保存" IconFont="Save" OnClick="btnSave_Click"></c:Button>
                                        <c:Button ID="btnDelete" runat="server" ConfirmTarget="Top" ComfirmScript="delectedRows();" ConfirmText="确定要删除吗?" ConfirmIcon="Question" Text="删除" IconFont="Remove" OnClick="btnDelete_Click"></c:Button>
                                        <c:TextBox ID="txtTypeName" runat="server" Width="250px"></c:TextBox>
                                        <c:TextBox ID="txtTypeCode" runat="server" Width="250px"></c:TextBox>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <script>
        function delectedRows() {
            C('<%=FGridPublicCode.ClientID%>').deleteSelectedRows();
        }
    </script>
</body>
</html>
