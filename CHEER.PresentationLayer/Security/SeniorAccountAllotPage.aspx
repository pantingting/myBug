<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeniorAccountAllotPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.SeniorAccountAllotPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px" BoxConfigPadding="5px" CssClass="noShadow">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="分配" Icon="Add" OnClick="cmdAdd_Click"></c:Button>
                        <c:Button runat="server" ID="cmdLook" Text="分配" Icon="ReportMagnify" OnClick="cmdLook_Click"></c:Button>
                        <c:Button runat="server" ID="cmdErDown" Text="错误信息下载" Icon="ArrowDown" OnClick="cmdErDown_Click"></c:Button>
                        <c:Button runat="server" ID="cmdReturn" Text="返回" Icon="PageBack" OnClick="cmdReturn_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:GroupPanel runat="server" ID="gp1" Title="分配规则">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px" LabelWidth="160">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:DropDownList runat="server" ID="drpLogin" Label="帐号名"></c:DropDownList>
                                        <c:Label runat="server" ID="Label1" Text="(说明：可以以工号或者英文名作为帐号名)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow>
                                                    <Items>
                                                        <c:TextBox runat="server" ID="txtBegin" Label="帐号前缀" MaxLength="5"></c:TextBox>
                                                        <c:Label runat="server" ID="Label10" Text="(说明：一般为不超过位为的字母或者数字组合)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>

                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:TextBox runat="server" ID="txtEnd" Label="帐号后缀" MaxLength="5"></c:TextBox>
                                        <c:Label runat="server" ID="Label15" Text="(说明：一般为不超过5位的字母或者数字组合)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow>
                                                    <Items>
                                                        <c:DatePicker runat="server" ID="txtStartDate" Label="生效时间"></c:DatePicker>
                                                        <c:Label runat="server" ID="Label11" Text="(说明：默认选择系统日期，可以设定)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>

                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:DatePicker runat="server" ID="txtEndDate" Label="失效时间"></c:DatePicker>
                                        <c:Label runat="server" ID="Label12" Text="(说明：默认为空，如果设定需大于生效时间)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow>
                                                    <Items>
                                                        <c:TextBox runat="server" ID="txtPassWord" Label="初始密码" TextMode="Password" MaxLength="50"></c:TextBox>
                                                        <c:Label runat="server" ID="Label9" Text="(说明：默认为帐号初始密码，可以设定)"></c:Label>
                                                    </Items>
                                                </c:FormRow>
                                            </Rows>
                                        </c:Form>

                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:CheckBox runat="server" ID="ckpwdchange" Label="初次登陆修改密码" Text="是"></c:CheckBox>
                                        <c:Label runat="server" ID="Label13" Text="(说明：配置分配帐号初次登陆密码修改状况)"></c:Label>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow>
                                    <Items>
                                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                            <Rows>
                                                <c:FormRow>
                                                    <Items>
                                                        <c:DropDownList runat="server" ID="drpRoles" Label="所属角色"></c:DropDownList>
                                                        <c:Label runat="server" ID="Label14" Text="(说明：员工帐号所担任的角色配置)"></c:Label>
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
                <c:Panel runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                            <Rows>
                                <c:FormRow>
                                    <Items>
                                        <c:Label runat="server" ID="lblmsg" ></c:Label>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Panel>
            </Items>
        </c:Panel>
    </form>
    <c:Window runat="server" ID="MaterialView" WindowPosition="Center" Target="Top" EnableIFrame="true" Hidden="true" Height="300px" Width="300px" CloseAction="HidePostBack"></c:Window>
    <script>
 
    </script>
</body>
</html>
