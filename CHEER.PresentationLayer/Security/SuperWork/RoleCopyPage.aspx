<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleCopyPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SuperWork.RoleCopyPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                        <c:Form ID="Form2" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false">
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
                <c:GroupPanel runat="server" ID="gp2" Title="命名规则">
                    <Items>
                        <c:Form ID="Form3" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow ColumnWidths="0.2 0.3 0.5">
                                                    <Items>
                                                        <c:Label runat="server" ShowLabel="false" Text="字符替换" ID="Label6"></c:Label>
                                                        <c:TextBox runat="server" ID="txtRepOID" MaxLength="50"></c:TextBox>
                                                        <c:Label runat="server" ID="Label11" Text="(说明：要替换的字符)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.3 0.5">
                                    <Items>
                                        <c:Label ID="Label3" runat="server" ShowLabel="false" Text=" "></c:Label>
                                        <c:TextBox runat="server" ID="txtRepNew" MaxLength="50"></c:TextBox>
                                        <c:Label runat="server" ID="Label12" Text="(说明：替换成的字符)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow ColumnWidths="0.2 0.3 0.5">
                                                    <Items>
                                                        <c:Label runat="server" ID="Label7" Text="字符插入"></c:Label>
                                                        <c:NumberBox runat="server" ID="numIndex" Label="起始位置" MaxLength="2"></c:NumberBox>
                                                        <c:Label runat="server" ID="Label14" Text="(说明：插入字符的开始位置)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.3 0.5">
                                    <Items>
                                        <c:Label ID="Label13" runat="server" Text=" "></c:Label>
                                        <c:TextBox runat="server" ID="txtInsertText" MaxLength="10"></c:TextBox>
                                        <c:Label runat="server" ID="Label15" Text="(说明：要插入的字符)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow ColumnWidths="0.2 0.3 0.5">
                                                    <Items>
                                                        <c:Label runat="server" ID="Label4" Text="名称前缀"></c:Label>
                                                        <c:TextBox runat="server" ID="txtBegin" MaxLength="10"></c:TextBox>
                                                        <c:Label runat="server" ID="Label17" Text="(说明：角色名称前缀)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.3 0.5">
                                    <Items>
                                        <c:Label runat="server" ID="Label5" Text="名称后缀"></c:Label>
                                        <c:TextBox runat="server" ID="txtEnd" MaxLength="10"></c:TextBox>
                                        <c:Label runat="server" ID="Label18" Text="(说明：角色名称后缀)"></c:Label>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:GroupPanel>
                <c:GroupPanel runat="server" ID="gp3" Title="复制目标">
                    <Items>
                        <c:Form ID="Form4" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px" LabelWidth="180">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:UserControlConnector ID="UserControlConnector1" runat="server">
                                            <uc1:StdBranchLoader runat="server" Label="归属组织" ID="StdBranchLoader" IsHaveManageUnit="true" IsShowLabel="true" IsIncludeSubBranchCanEdit="true" IsIncludeSubBranch="true" IsShowIncludeSubBranch="false" IsShowIncludeStopUnit="false" />
                                        </c:UserControlConnector>

                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:CheckBox runat="server" ID="ckPack" Label="是否复制范围包" Text="是" AutoPostBack="true" OnCheckedChanged="ckPack_CheckedChanged"></c:CheckBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:CheckBox runat="server" ID="ckSec" Label="是否复制权限设定" Text="是" AutoPostBack="true" OnCheckedChanged="ckSec_CheckedChanged"></c:CheckBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:GroupPanel>
                <c:Panel runat="server" ShowBorder="false" ShowHeader="false" Height="40">
                    <Items>
                        <c:Form ID="Form5" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:Label runat="server" ID="lblmsg"></c:Label>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Panel>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
