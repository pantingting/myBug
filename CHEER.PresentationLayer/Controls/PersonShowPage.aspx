<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonShowPage.aspx.cs" Inherits="CHEER.PresentationLayer.Controls.PersonShowPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager ID="PageManager1" runat="server" AutoSizePanelID="mainRegions" AjaxLoadingType="Mask" />
        <c:RegionPanel runat="server" ShowBorder="false" ID="mainRegions" BodyPadding="5px">
            <Regions>
                <c:Region ID="Region1" runat="server" ShowBorder="false" ShowHeader="false" Position="Top">
                    <Items>
                        <c:Form BoxConfigPosition="Start" BodyPadding="5px" ShowBorder="true" ID="Form2" runat="server" LabelWidth="80" ShowHeader="false">
                            <Rows>
                                <c:FormRow ColumnWidths="50% 50%">
                                    <Items>
                                        <c:TextBox ID="txtEmpNo" runat="server" Label="工号"></c:TextBox>
                                        <c:TextBox ID="txtEmpName" runat="server" Label="姓名"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="50% 50%">
                                    <Items>
                                        <c:DropDownList ID="ddlSex" runat="server" Label="性别"></c:DropDownList>
                                        <c:DropDownList ID="ddlState" runat="server" Label="在职状态"></c:DropDownList>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="100% 60px">
                                    <Items>
                                        <c:UserControlConnector ID="UserControlConnector1" runat="server">
                                            <uc1:StdBranchLoader runat="server" ID="StdBranchLoader" />
                                        </c:UserControlConnector>
                                        <c:Button runat="server" Text="查询" ID="btnQuery" OnClick="btnQuery_Click"></c:Button>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>

                    </Items>
                </c:Region>
                <c:Region ID="Region2" runat="server" Layout="Fit" ShowHeader="false" ShowBorder="false" Position="Center">
                    <Items>
                        <c:Grid AllowPaging="true" ShowBorder="true" EnableRowDoubleClickEvent="true" OnRowDoubleClick="grdMain_RowDoubleClick" IsDatabasePaging="true" OnPageIndexChange="grdMain_PageIndexChange" ID="grdMain" EnableColumnLines="true" EnableMultiSelect="false" ShowGridHeader="true" runat="server" ShowHeader="false">
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>

    </form>
</body>
</html>
