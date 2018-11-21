<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountAllotPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.AccountAllotPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" CssClass="noShadow">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="分配" Icon="Add" OnClick="cmdAdd_Click"></c:Button>
                        <c:Button runat="server" ID="cmdLook" Text="查看详细" Icon="ReportMagnify" OnClick="cmdLook_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:GroupPanel runat="server" ID="gp1" Title="分配规则">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:Label runat="server" ID="Label10" Text="(说明：以员工工号作为对应员工的登陆帐号)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                    <Items>
                                        <c:Label runat="server" ID="Label4" Text="生效时间"></c:Label>
                                        <c:DatePicker runat="server" ID="txtStartDate"></c:DatePicker>
                                        <c:Label runat="server" ID="Label11" Text="(说明：默认选择系统日期，可以设定)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                                    <Items>
                                                        <c:Label runat="server" ID="Label5" Text="失效时间"></c:Label>
                                                        <c:DatePicker runat="server" ID="txtEndDate" ></c:DatePicker>
                                                        <c:Label runat="server" ID="Label12" Text="(说明：默认为空，如果设定需大于生效时间)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                    <Items>
                                        <c:Label runat="server" ID="Label6" Text="初始密码"></c:Label>
                                        <c:TextBox runat="server" ID="txtPassWord" TextMode="Password" MaxLength="50"></c:TextBox>
                                        <c:Label runat="server" ID="Label9" Text="(说明：默认为帐号初始密码，可以设定)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                                    <Items>
                                                        <c:Label runat="server" ID="Label7" Text="初次登陆修改密码"></c:Label>
                                                        <c:CheckBox runat="server" ID="ckpwdchange" Text="是"></c:CheckBox>
                                                        <c:Label runat="server" ID="Label13" Text="(说明：配置分配帐号初次登陆密码修改状况)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.2 0.2 0.4">
                                    <Items>
                                        <c:Label runat="server" ID="Label8" Text="员工在职状态"></c:Label>
                                        <c:CheckBox runat="server" ID="ckReg" Text="正式"></c:CheckBox>
                                        <c:CheckBox runat="server" ID="ckPro" Text="试用"></c:CheckBox>
                                        <c:Label runat="server" ID="Label14" Text="(说明：员工在职状态范围设定)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                                    <Items>
                                                        <c:Label runat="server" ID="Label1" Text="所属角色"></c:Label>
                                                        <c:DropDownList runat="server" ID="drpRoles"></c:DropDownList>
                                                        <c:Label runat="server" ID="Label3" Text="(说明：员工帐号所担任的角色配置)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:GroupPanel>
                <c:GroupPanel runat="server" ID="gp2" Title="员工范围">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:Label runat="server" ID="Label15" Text="(说明：默认情况下为用户具有数据权限的员工)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.8">
                                    <Items>
                                        <c:Label runat="server" ID="Label26" Text="部门"></c:Label>
                                        <c:UserControlConnector runat="server" >
                                            <uc1:StdBranchLoader runat="server" ID="StdBranchLoader" IsShowLabel="false" />
                                        </c:UserControlConnector>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                    <Items>
                                        <c:Label runat="server" ID="Label23" Text="工号从/到"></c:Label>
                                        <c:TextBox runat="server" ID="txtEmpFrom" MaxLength="50"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtEmpTo" MaxLength="50"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.4 0.4">
                                    <Items>
                                        <c:Label runat="server" ID="Label18" Text="入职日期从/到"></c:Label>
                                        <c:DatePicker runat="server" ID="UcDate1"></c:DatePicker>
                                        <c:DatePicker runat="server" ID="UcDate2"></c:DatePicker>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.8">
                                    <Items>
                                        <c:Label runat="server" ID="Label22" Text="DL/IDL"></c:Label>
                                        <c:DropDownList runat="server" ID="drpDLIDL"></c:DropDownList>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow ColumnWidths="0.2 0.8">
                                    <Items>
                                        <c:Label runat="server" ID="Label20" Text="员工类别"></c:Label>
                                        <c:DropDownList runat="server" ID="drpType"></c:DropDownList>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:GroupPanel>
                <c:Panel runat="server" ShowBorder="false" ShowHeader="false" Height="40">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
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
