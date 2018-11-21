<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAddPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.UserAddPage" %>

<%@ Register Src="~/Controls/PersonSelect.ascx" TagPrefix="uc1" TagName="PersonSelect" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="mainPanel" FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Panel runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px" ID="mainPanel">
            <Items>
                <c:Form runat="server" ID="mainForm" LabelWidth="140px" BodyPadding="5px" ShowHeader="false" ShowBorder="true">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="btnAdd" IconFont="Save" Text="保存" ValidateForms="mainForm" OnClick="btnAdd_Click" EnablePostBack="true"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Rows>
                        <c:FormRow runat="server">
                            <Items>
                                <c:TextBox runat="server" ID="txtLoginName" Label="登录名" ShowRedStar="true" Required="true" MaxLength="40"></c:TextBox>
                                <c:UserControlConnector runat="server">
                                    <uc1:PersonSelect runat="server" ID="PersonSelect" />
                                </c:UserControlConnector>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server">
                            <Items>
                                <c:TextBox runat="server" ID="txtPassWord" TextMode="Password" Label="密码" Required="true" ShowRedStar="true" MaxLength="40"></c:TextBox>
                                <c:TextBox runat="server" ID="txtConfirmPwd" TextMode="Password" Label="确认密码" Required="true" ShowRedStar="true" MaxLength="40"></c:TextBox>
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
                                <c:DropDownList runat="server" ID="drpIsLock"  Label="是否停用"></c:DropDownList>
                                <c:DropDownList runat="server" ID="drpChangePwd" Hidden="true" Label="下次登录更改密码"></c:DropDownList>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:TextArea runat="server" ID="txtDescription" Label="用户描述" Height="188" MaxLength="100" MaxLengthMessage="不得超过100字"></c:TextArea>
                            </Items>
                        </c:FormRow>
                    </Rows>
                </c:Form>
            </Items>
        </c:Panel>
        <c:Window Title="  " Hidden="true" runat="server" ID="detailWindow" EnableIFrame="true" IsModal="true" IFrameUrl="PersonShowPage.aspx" Target="Top" Height="400" Width="400" WindowPosition="Center"></c:Window>
    </form>
    <c:HiddenField runat="server" ID="txtUserID"></c:HiddenField>
    <c:Button runat="server" ID="btnRefresh" Hidden="true"></c:Button>
    <script>
        C.ready(function () {
            var userid = C('<%=txtUserID.ClientID%>').getValue();
            var txtuserid = C('<%=txtUserID.ClientID%>');
            var username = C('<%=txtLoginName.ClientID%>');
            var pwd = C('<%=txtPassWord.ClientID%>').getValue()
            txtuserid.setValue(userid);
            if (userid != '' && pwd == '') {
                var cmdadd = C('<%=btnAdd.ClientID%>');
                cmdadd.disabled = true;
                var refresh = C('<%=btnRefresh.ClientID%>');
                refresh.click();
            }
            C('<%=btnAdd.ClientID%>').on('click', function () {
                IsOKOnUserData('<%=getAlert("ZGAIA00078")%>', '<%=getAlert("ZGAIA00077")%>', '<%=base.getAlert("ZGAIA00076")%>', '<%=getAlert("ZGAIA00075")%>', '<%=base.getAlert("ZGAIA00074")%>', '<%=base.getAlert("ZGAIA00073")%>', '<%=base.getAlert("ZGAIA00072")%>');
            });
        });
        function IsOKOnUserData(msg_namenull, msg_psnnull, msg_pwdnull, msg_confirmnull, msg_pwderror, msg_ErrorDate, msg_morelength) {
            var username = C('<%=txtLoginName.ClientID%>').getValue();// document.getElementById('txtLoginName').value;
            var psnid = C('<%=(this.PersonSelect as CHEER.PresentationLayer.Controls.PersonSelect).TextPersonId.ClientID%>').getValue()// document.getElementById('PersonSelect_txtPersonID').value;
            var pwd = C('<%=txtPassWord.ClientID%>').getValue()// document.getElementById('txtPassWord').value;
            var pwdconfirm = C('<%=txtConfirmPwd.ClientID%>').getValue()// document.getElementById('txtConfirmPwd').value;
            var desc = C('<%=txtDescription.ClientID%>').getValue()// document.getElementById('txtDescription').value;
            if (username == "") {
                C.alert(msg_namenull);
                return false;
            }
            if (psnid == "") {
                C.alert(msg_psnnull);
                return false;
            }
            if (pwd == "") {
                C.alert(msg_pwdnull);
                return false;
            }
            if (pwdconfirm == "") {
                C.alert(msg_confirmnull);
                return false;
            }
            if (desc.length > 100) {
                C.alert(msg_morelength);
                return false;
            }
            if (pwd != pwdconfirm) {
                C.alert(msg_pwderror);
                return false;
            }
            e1 = C('<%=txtStartDate.ClientID%>'); //document.getElementById("txtStartDate_GAIADate");
            e2 = C('<%=txtEndDate.ClientID%>'); //document.getElementById("txtEndDate_GAIADate");
            if (e1.getValue() != "" && e2.getValue != "" && e1.getValue > e2.getValue) {
                alert(msg_ErrorDate);//结束日期不得小于开始日期
                return false;
            }
            return true;
        }
    </script>
</body>
</html>
