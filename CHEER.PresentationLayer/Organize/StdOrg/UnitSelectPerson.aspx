<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnitSelectPerson.aspx.cs" Inherits="CHEER.PresentationLayer.Organize.StdOrg.UnitSelectPerson" %>

<%@ Register Assembly="CheerUI" Namespace="CheerUI" TagPrefix="c" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="mainRegions" AjaxLoadingType="Mask" />
        <c:RegionPanel BodyPadding="5px" runat="server" ID="mainRegions" ShowBorder="false">
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Top">
                    <Items>
                        <c:Form BodyPadding="5px 5px 0 5px" ID="Form2" runat="server" ShowBorder="true" LabelWidth="80" ShowHeader="false">
                            <Toolbars>
                                <c:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <c:Button runat="server" IconFont="Save" Text="确定" ID="btnConfirm" OnClick="btnConfirm_Click"></c:Button>
                                        <c:Button runat="server" IconFont="Remove" Text="取消" ID="btnCancel" OnClick="btnCancel_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox ID="txtNo" runat="server" Label="工号"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox ID="txtName" runat="server" Label="姓名"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="100% 70px">
                                    <Items>
                                        <c:DropDownList ID="ddlOrg" MaxPopHeight="200" runat="server" Label="组织"></c:DropDownList>
                                        <c:Button IconFont="Search" runat="server" Text="查询" ID="btnQuery" OnClick="btnQuery_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" Layout="Fit" ShowBorder="false" ShowHeader="false" Position="Center">
                    <Items>
                        <c:Grid EnableRowDoubleClickEvent="true" CssStyle="border-top:none;" ShowBorder="true" OnRowDoubleClick="grdMain_RowDoubleClick" ID="grdMain" EnableColumnLines="true" EnableMultiSelect="false" ShowGridHeader="true" runat="server" ShowHeader="false">
                            <Columns>
                                <c:BoundField ColumnID="PERSONID" DataField="PERSONID" HeaderText="" Hidden="true" EnableHeaderMenu="false" />
                                <c:BoundField ColumnID="EMPLOYEEID" DataField="EMPLOYEEID" HeaderText="工号" BoxFlex="1" EnableHeaderMenu="false" />
                                <c:BoundField ColumnID="TRUENAME" DataField="TRUENAME" HeaderText="姓名" BoxFlex="1" EnableHeaderMenu="false" />
                            </Columns>
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>

    </form>
</body>
</html>
