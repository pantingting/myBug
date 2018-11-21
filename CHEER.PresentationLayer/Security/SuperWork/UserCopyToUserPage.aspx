<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserCopyToUserPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SuperWork.UserCopyToUserPage" %>

<%@ Register Src="~/Controls/PersonSelect.ascx" TagPrefix="uc1" TagName="PersonSelect" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" FormLabelWidth="180" />
        <c:RegionPanel BodyPadding="5px" runat="server" ID="RegionPanel1" ShowBorder="false">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="复制" Icon="PageCopy" OnClick="cmdAdd_Click"></c:Button>
                        <c:Button runat="server" ID="cmdReturn" Text="返回" Icon="PageBack" OnClick="cmdReturn_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Left" Margin="0 3px 0 0" Width="580px" Layout="Fit">
                    <Items>
                        <c:Form runat="server" Title="源用户信息" BodyPadding="5px">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtName" Readonly="true" Label="源用户登陆名"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtDeptName" Readonly="true" Label="源用户部门"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtEmpID" Readonly="true" Label="源用户工号"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Label runat="server" ID="lblmsg" Text=""></c:Label>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Center" Margin="0 0 0 3px" Layout="Fit">
                    <Items>
                        <c:Form runat="server" ShowBorder="true" Title="复制目标" BodyPadding="5px">
                            <Rows>
                                <c:FormRow ColumnWidths="100% 20px">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="目标用户"></c:TextBox>
                                        <c:Image ID="img" runat="server" ImageCssStyle="cursor:pointer;" Icon="User"></c:Image>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:CheckBox runat="server" ID="ckPack" Label="是否仅复制范围包" Text="是" OnCheckedChanged="ckPack_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:CheckBox runat="server" ID="ckSec" Label="是否覆盖原有权限" Text="是" OnCheckedChanged="ckSec_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <c:TextBox runat="server" ID="testname" Hidden="true" HideMode="Display"></c:TextBox>
    <c:Window Title="  " runat="server" ID="detailWindow" Hidden="true" EnableIFrame="true" IsModal="true" IFrameUrl="UserSelectPage.aspx" Target="Top" Height="400" Width="400" WindowPosition="Center"></c:Window>
</body>
</html>
