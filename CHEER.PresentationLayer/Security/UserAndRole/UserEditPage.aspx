<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEditPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.UserEditPage" %>

<%@ Register Src="~/Controls/PersonSelect.ascx" TagPrefix="uc1" TagName="PersonSelect" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Panel runat="server" ShowBorder="false" IsViewPort="true" ShowHeader="false" Layout="Fit" BodyPadding="5px 5px 0 5px">
            <Items>
                <c:Form runat="server" ID="mainForm" LabelWidth="140px" BodyPadding="5px" ShowHeader="false" ShowBorder="true">
                    <Toolbars>
                        <c:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <c:Button runat="server" ID="btnAdd" IconFont="Save" Text="保存" ValidateForms="mainForm" OnClick="btnAdd_Click" EnablePostBack="true"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Rows>
                        <c:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <c:TextBox runat="server" ID="txtLoginName" Label="登录名" ShowRedStar="true" Required="true" MaxLength="40"></c:TextBox>
                                <c:UserControlConnector ID="UserControlConnector1" runat="server">
                                    <uc1:PersonSelect runat="server" ID="PersonSelect" />
                                </c:UserControlConnector>
                            </Items>
                        </c:FormRow>
                        <c:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <c:TextBox runat="server" ID="txtPassWord" Label="密码" Required="true" ShowRedStar="true" TextMode="Password" MaxLength="40"></c:TextBox>
                                <c:TextBox runat="server" ID="txtConfirmPwd" Label="确认密码" Required="true" ShowRedStar="true" TextMode="Password" MaxLength="40"></c:TextBox>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:DatePicker runat="server" ID="txtStartDate" Label="生效日期"></c:DatePicker>
                                <c:DatePicker runat="server" ID="txtEndDate" Label="失效日期"></c:DatePicker>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:DropDownList runat="server" ID="drpIsLock" Label="是否停用"></c:DropDownList>
                                <c:DropDownList runat="server" Hidden="true" ID="drpChangePwd" Label="下次登录更改密码"></c:DropDownList>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:TextArea runat="server" ID="txtDescription" Label="用户描述" Height="178" MaxLength="100" MaxLengthMessage="不得超过100字"></c:TextArea>
                            </Items>
                        </c:FormRow>
                    </Rows>
                </c:Form>
            </Items>
        </c:Panel>
        <c:Window Title="  " Hidden="true" runat="server" ID="detailWindow" EnableIFrame="true" IsModal="true" IFrameUrl="PersonShowPage.aspx" Target="Top" Height="400" Width="400" WindowPosition="Center"></c:Window>
    </form>
</body>
</html>
