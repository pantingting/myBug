<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubServicesEditPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.Services.SubServicesEditPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .noShadow div {
            -webkit-box-shadow: none !important;
            -moz-box-shadow: none !important;
            box-shadow: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Layout="Fit" CssClass="noShadow">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="cmdAdd" Text="保存" Icon="SystemSave" OnClick="cmdAdd_Click" ValidateForms="Form2"></c:Button>
                        <c:Button runat="server" ID="btnOtherSet" Text="其他配置" Icon="Package"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:Form runat="server" ID="Form2" ShowBorder="false" ShowHeader="false">
                    <Rows>
                        <c:FormRow runat="server" ColumnWidths="0.3 0.7">
                            <Items>
                                <c:TextBox runat="server" ID="txtAppNO" Label="应用程序名"></c:TextBox>
                                <c:TextBox runat="server" ID="txtComName" Label="注册名称"></c:TextBox>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.3 0.7">
                            <Items>
                                <c:TextBox runat="server" ID="txtModuleNO" Label="模块编号"></c:TextBox>
                                <c:TextBox runat="server" ID="txtFileName" Label="所在文件"></c:TextBox>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server">
                            <Items>
                                <c:Form runat="server" ShowBorder="false" ShowHeader="false">
                                    <Rows>
                                        <c:FormRow runat="server" ColumnWidths="0.3 0.7">
                                            <Items>
                                                <c:TextBox runat="server" ID="txtRunState" Label="当前状态"></c:TextBox>
                                                <c:ContentPanel ID="ContentPanel1" runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                                            </Items>
                                        </c:FormRow>
                                    </Rows>
                                </c:Form>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.3 0.7">
                            <Items>
                                <c:TextBox runat="server" ID="txtShowName" Label="服务名"></c:TextBox>
                                <c:TextBox runat="server" ID="txtLogFileDir" Label="日志路径" ShowRedStar="true" Required="true"></c:TextBox>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server">
                            <Items>
                                <c:TextArea runat="server" ID="txtCustomParameter" Label="自定义参数" Height="76" MaxLength="1000" MaxLengthMessage="不得超过1000字"></c:TextArea>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:TextArea runat="server" ID="txtDescription" Label="描述" Height="60" MaxLength="200" MaxLengthMessage="不得超过200字"></c:TextArea>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.3 0.3 0.4">
                            <Items>
                                <c:DropDownList runat="server" ID="drpRunType" Label="运行状态"></c:DropDownList>
                                <c:DropDownList runat="server" ID="drpStartType" Label="启动类型"></c:DropDownList>
                                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.3 0.4 0.3">
                            <Items>
                                <c:TextBox runat="server" ID="txtRunValue" Label="运行值"></c:TextBox>
                                <%--<c:TextBox runat="server" ID="UcTimeRunTime" Label="运行时间" Readonly="true"></c:TextBox>--%>
                                <c:TimePicker runat="server" ID="tpTimeRunTime" Label="运行时间" EnableEdit="true" RequiredMessage="a" CompareMessage="b" RegexMessage="c" MaxLengthMessage="d" MinLengthMessage="e"></c:TimePicker>
                                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                            </Items>
                        </c:FormRow>
                    </Rows>
                </c:Form>
            </Items>
        </c:Panel>
    </form>
    <c:Window runat="server" ID="MsgSetting" WindowPosition="Center" Target="Top" EnableIFrame="true" CloseAction="HidePostBack" Hidden="true" Width="800px" Height="600px"></c:Window>
    <script>
        

        function showother(url)
        {
            F('<%=MsgSetting.ClientID%>').f_show('<%=getBaseUrl()%>'+url, '<%=getString("ZGAIA06115")%>');
        }
    </script>
</body>
</html>
