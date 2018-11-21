<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchSearch.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.BranchSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" BodyPadding="5px" ID="RegionPanel1" ShowBorder="false" >
            <Regions>
                <c:Region runat="server" Position="Top" Layout="Fit" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:Form runat="server" BodyPadding="5px" ShowHeader="false">
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="0.5 0.5 72">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtDepName" Label="部门名称"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtDepCode" Label="部门代码"></c:TextBox>
                                        <c:Button runat="server" ID="cmdSearch" IconFont="Search" Text="查询" OnClick="cmdSearch_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
            </Regions>
            <Regions>
                <c:Region runat="server" Position="Center" Layout="Fit" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <c:Grid CssStyle="border-top:none !important;" runat="server" ID="UltDepGrid" EnableColumnLines="true" ShowHeader="false"  AllowPaging="true" ></c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <script>
        C.ready(function () {
            C('<%=UltDepGrid.ClientID%>').on('rowclick', function (event, index) {
                var record = this.getRowValue(index)
                if (record != null && record != "") {
                    var unitid = record.UNITID;
                    var unitcode = record.UNITCODE;
                    var unitsec = "YES";
                    parent.setUnitCode(unitcode, unitid, unitsec);
                }
            });
        });
    </script>
</body>
</html>
