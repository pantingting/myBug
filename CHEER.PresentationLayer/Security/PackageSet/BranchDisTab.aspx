<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchDisTab.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.BranchDisTab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="RegionPanel1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:RegionPanel BodyPadding="5px" runat="server" ID="RegionPanel1" ShowBorder="true">
            <Regions>
                <c:Region runat="server" Position="Top" ShowHeader="false" Layout="Fit" BodyPadding="5px">
                    <Items>
                        <c:Form runat="server" BodyPadding="5px" ShowHeader="false">
                            <Toolbars>
                                <c:Toolbar runat="server" ToolbarAlign="Right">
                                    <Items>
                                        <c:CheckBox runat="server" ID="CkbIsIncludeStopUnit" Text="包含停用组织" OnCheckedChanged="CkbIsIncludeStopUnit_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                        <c:Button runat="server" ID="cmdConfirm" Text="确定" Icon="Accept" OnClick="cmdConfirm_Click"></c:Button>
                                        <c:Button runat="server" ID="cmdReturn" Text="取消" Icon="Cancel" OnClick="cmdReturn_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="100% 140px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtUnitCode" Label="部门代码"></c:TextBox>
                                        <c:CheckBox runat="server" ID="checkSub" Label="包含子部门"></c:CheckBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" Position="Center" ShowBorder="false" ShowHeader="false" Layout="Fit">
                    <Items>
                        <c:TabStrip　CssStyle="border-top:none !important;" runat="server" ID="UltDepTab">
                            <Tabs>

                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <c:Label runat="server" ID="lbIsSecurity" CssStyle="display:none;" Hidden="true"></c:Label>
    <c:Label runat="server" ID="lbUnitID" CssStyle="display:none;"  Hidden="true" ></c:Label>
    <c:CheckBox runat="server" ID="checkSubUnderMU" Hidden="true"></c:CheckBox>
    <script>
        C.ready(function () {
            C('<%=cmdConfirm.ClientID%>').on('click', function () {
                return confirmclick('<%="请选择组织"%>', '<%=base.getAlert("ZGAIA00348")%>');
            });

            C('<%=txtUnitCode.ClientID%>').el.onmousedown = function () {
                C('<%=lbIsSecurity.ClientID%>').setValue('NO');
                return false;
            };
            C('<%=checkSub.ClientID%>').el.onclick = function () {
                setcheckbox('<%=checkSub.ClientID%>', '<%=checkSubUnderMU.ClientID%>');
            }
            C('<%=checkSubUnderMU.ClientID%>').el.onclick = function () {
                setcheckbox('<%=checkSubUnderMU.ClientID%>', '<%=checkSub.ClientID%>');
            }
        });

        function confirmclick(msg_nocode, msg_nosec) {
            var unitcode = C('<%=txtUnitCode.ClientID%>').getValue();
            if (unitcode == '') {
                C.alert(msg_nocode);
                return false;
            }
            var unitsec = C('<%=lbIsSecurity.ClientID%>').getValue();
            if (unitsec == 'NOT') {
                C.alert(msg_nosec);
                return false;
            }
            return true;
        }

        function setcheckbox(clicked, unclicked) {
            var oclicked = F(clicked);
            var ounclicked = F(unclicked);

            if (oclicked.checked)
                ounclicked.checked = false;
        }
        function setUnitCode(unitcode, unitid, unitsec) {
            var txtcode = C('<%=txtUnitCode.ClientID%>');
            var txtsec = C('<%=lbIsSecurity.ClientID%>');
            var txtid = C('<%=lbUnitID.ClientID%>');
            txtcode.setValue(unitcode);
            txtid.setValue(unitid);
            if (unitsec == 'YES')
                txtsec.setValue(unitsec);
            else
                txtsec.setValue('NOT');
        }

    </script>
</body>
</html>
