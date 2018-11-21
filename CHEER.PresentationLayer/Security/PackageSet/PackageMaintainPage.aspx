<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.PackageMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .textRed span {
            color:red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="RegionPanel1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:RegionPanel runat="server" ShowBorder="false" ID="RegionPanel1" BodyPadding="5px">
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" Position="Top">
                    <Items>
                        <c:Form runat="server" ID="mainForm" ShowHeader="false" BodyPadding="5px">
                            <Toolbars>
                                <c:Toolbar runat="server" ToolbarAlign="Right">
                                    <Items>
                                        <c:Button runat="server" ValidateForms="mainForm" ID="cmdSave" Text="保存" Icon="SystemSave" OnClick="cmdSave_Click"></c:Button>
                                        <c:Button runat="server" ID="cmdConfirm" Text="功能权限维护" Icon="Wrench" OnClick="cmdConfirm_Click"></c:Button>
                                        <c:Button runat="server" ID="cmdReturn" Text="返回" Icon="PageBack" OnClick="cmdReturn_Click"></c:Button>
                                        <c:ContentPanel runat="server" Width="5px" Height="1px" ShowBorder="false" ShowHeader="false"></c:ContentPanel>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="0.3 0.3 0.4">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtChanger" Readonly="true" Label="最后修改人"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtChangeTime" Readonly="true" Label="最后修改时间"></c:TextBox>
                                        <c:Label runat="server" ID="lbPS" Text="| 注：请先输入【范围包名称】，点击【保存】之后再进行其他操作 "></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.3 0.7">
                                    <Items>
                                        <c:TextBox runat="server" Required="true" ID="txtPackageName" Label="范围包名称"></c:TextBox>
                                        <c:TextArea runat="server" ID="txtDescription" Label="范围包描述" Height="50" MaxLength="100" MaxLengthMessage="不得超过100字"></c:TextArea>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
            </Regions>
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" Position="Center">
                    <Items>
                        <c:TabStrip CssStyle="border-top:none !important;" runat="server" BodyPadding="5px" ID="UlPackageTab">
                            <Tabs>
                            </Tabs>
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Label runat="server" ID="lblrule" CssClass="textRed"></c:Label>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <c:Label runat="server" ID="lbPackageID" Hidden="true"></c:Label>
</body>
</html>
