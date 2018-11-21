<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecuritySetTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SecuritySet.SecuritySetTabPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" />
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false" BodyPadding="5px">
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Top" Layout="Fit">
                    <Items>
                        <c:Form runat="server" ShowHeader="false" BodyPadding="5px">
                            <Toolbars>
                                <c:Toolbar runat="server">
                                    <Items>
                                        <c:Button runat="server" ID="cmdReturn" Text="返回" Icon="PageBack" OnClick="cmdReturn_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="0.4 0.6">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtChanger" Label="最后修改人" Readonly="true"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtChangeTime" Label="最后修改时间" Readonly="true"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server" ColumnWidths="0.4 0.6">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtPackageName" Label="范围包名称"></c:TextBox>
                                        <c:TextArea runat="server" ID="txtDescription" Label="范围包描述" Height="50px"></c:TextArea>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                        <c:Label runat="server" Text=" " Hidden="true"></c:Label>
                    </Items>
                </c:Region>
            </Regions>
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" Position="Center">
                    <Items>
                        <c:TabStrip CssStyle="border-top:none;" runat="server" ID="UlRoleTab">
                            <Tabs>
                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>

    </form>
    <c:HiddenField runat="server" ID="txtPackageID"></c:HiddenField>
    <c:HiddenField runat="server" ID="txtIsBack"></c:HiddenField>
</body>
</html>
