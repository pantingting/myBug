<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PSNGrid.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.PSNQuerySet.PSNGrid" %>

<%@ Register Src="../../Controls/UserQuery.ascx" TagPrefix="uc1" TagName="UserQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PManager" AjaxLoadingType="Mask" FormMessageTarget="Qtip" FormLabelWidth="100px" AutoSizePanelID="mainRegions" />
        <c:RegionPanel runat="server" BodyPadding="5px" ShowBorder="false" ID="mainRegions">
            <Regions>
                <c:Region runat="server" ShowHeader="false" Position="Top" BodyPadding="5px 5px 0 5px">
                    <Items>
                        <c:UserControlConnector runat="server">
                            <uc1:UserQuery runat="server" ID="UserQuery" />
                        </c:UserControlConnector>
                    </Items>
                </c:Region>
                <c:Region runat="server" Layout="Fit" ShowBorder="false" ShowHeader="false" Position="Center">
                    <Items>
                        <c:Grid EnableColumnLines="true" EnableCheckBoxSelect="true" EnableMultiSelect="true" CssStyle="border-top:none !important;" IsDatabasePaging="true" AllowPaging="true" OnPageIndexChange="grdMain_PageIndexChange" OnRowCommand="grdMain_RowCommand" OnRowDataBound="grdMain_RowDataBound" runat="server" ID="grdMain" ShowHeader="false">
                            <Toolbars>
                                <c:Toolbar runat="server" ID="toolBar">
                                    <Items>
                                        <c:Button ID="btnAdd" runat="server" Text="新增" IconFont="Plus" OnClick="btnAdd_Click"></c:Button>
                                        <c:Button runat="server" IconFont="Remove" ID="Button1" OnClick="Deletebtn_Click" Text="删除" ConfirmText="确认删除吗?" ConfirmIcon="Warning" ConfirmTitle="警告"></c:Button>
                                        <c:Button ID="btnExport" runat="server" Text="数据导出" IconFont="FileExcelO" OnClick="btnExport_Click" EnableAjax="false" DisableControlBeforePostBack="false"></c:Button>
                                        <c:ToolbarSeparator runat="server"></c:ToolbarSeparator>
                                        <c:ToolbarFill runat="server"></c:ToolbarFill>
                                        <c:Button runat="server" ID="btnPass" ConfirmText="是否确认生效" IconFont="CheckCircle" Text="生效" Hidden="true" OnClick="btnPass_Click"></c:Button>
                                        <c:Button runat="server" ID="btnReject" ConfirmText="是否确认驳回" IconFont="TimesCircle" Text="驳回" Hidden="true" OnClick="btnReject_Click"></c:Button>
                                        <c:Button runat="server" ID="btnDelete" ConfirmText="是否确认开除" IconFont="Remove" Text="开除" Hidden="true" OnClick="btnDelete_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
        <c:Window runat="server" BodyPadding="5px 5px 0 5px" Target="Parent" EnableIFrame="true" Hidden="true" Height="600px" Width="1000px" ID="detailWindow" CloseAction="HidePostBack" OnClose="detailWindow_Close" Title="详细">
        </c:Window>
    </form>
</body>
</html>
