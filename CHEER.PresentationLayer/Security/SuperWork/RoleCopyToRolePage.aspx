<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleCopyToRolePage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SuperWork.RoleCopyToRolePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="复制" Icon="PageCopy" OnClick="cmdAdd_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:GroupPanel runat="server" ID="gp1" Title="源角色信息">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtName" Label="源角色名称" MaxLength="50"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtDeptName" Label="源角色部门" MaxLength="50"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:GroupPanel>
                <c:GroupPanel runat="server" ID="gp2" Title="复制目标">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="180">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtRoleName" Label="目的角色"></c:TextBox>
                                        <c:Image ID="img" runat="server" ImageCssStyle="cursor:pointer;" Icon="Find"></c:Image>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtAimDeptName" Label="目的角色部门" MaxLength="50"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:CheckBox runat="server" ID="ckPack" Label="是否仅复制范围包" Text="是" AutoPostBack="true" OnCheckedChanged="ckPack_CheckedChanged"></c:CheckBox>
                                        <c:CheckBox runat="server" ID="ckSec" Label="是否覆盖原有权限" Text="是" AutoPostBack="true" OnCheckedChanged="ckSec_CheckedChanged"></c:CheckBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:GroupPanel>
                <c:Panel runat="server" ShowBorder="false" ShowHeader="false" Height="40">
                    <Items>
                        <c:Label runat="server" ID="lblmsg"></c:Label>
                    </Items>
                </c:Panel>
            </Items>
        </c:Panel>
        <c:TextBox runat="server" ID="txtSysID" Hidden="true" HideMode="Display"></c:TextBox>
    </form>
    <c:Window runat="server" ID="detailWindow" Hidden="true" EnableIFrame="true" IsModal="true" IFrameUrl="UserSelectPage.aspx" Target="Top" Height="400" Width="400" WindowPosition="Center" Title="角色选取"></c:Window>
</body>
</html>
