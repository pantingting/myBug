<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PSNBasicQuery.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.PSNQuerySet.PSNBasicQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AutoSizePanelID="Panel1" FormLabelWidth="160" />
        <c:Panel ID="Panel1" BodyPadding="0px 0px 5 0px" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region">
            <Items>
                <c:Form runat="server" ID="formCondition" CssStyle="border-bottom:none" ShowHeader="false" BodyPadding="5px 5px 0 5px" RegionPosition="Top">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="btnUpdate" IconFont="Save" OnClick="btnUpdate_Click"></c:Button>
                                <c:Button runat="server" ID="btnReturn" IconFont="ChevronLeft" Text="关闭"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                </c:Form>
                <c:Form runat="server" ID="form" CssStyle="border-top:none;"  ShowHeader="false" BodyPadding="0px 5px 0 5px" RegionPosition="Center">
                    <Items>
                        <%--<c:FormRow runat="server">
                            <Items>
                                <c:TextBox runat="server" ID="txtloginName" Label="登录名" ShowRedStar="true" Required="true" Readonly="true"></c:TextBox>
                                <c:TextBox runat="server" ID="txtpassword" TextMode="Password" Label="密码" ShowRedStar="true" Required="true" Readonly="true"></c:TextBox>
                                <c:DropDownList runat="server" ID="ddlstatus1" Label="状态" Required="true" ShowRedStar="true" Readonly="true">
                                    <c:ListItem Value="" Text="" />
                                    <c:ListItem Value="-2" Text="完善资料" />
                                    <c:ListItem Value="-1" Text="个人认证" />
                                    <c:ListItem Value="0" Text="待审核" />
                                    <c:ListItem Value="1" Text="已生效" />
                                    <c:ListItem Value="2" Text="已开除" />
                                </c:DropDownList>
                            </Items>
                        </c:FormRow>

                        <c:FormRow runat="server">
                            <Items>
                                <c:Image runat="server" ID="imgLogoUrl" Label="个人头像" ImageWidth="140" ImageHeight="80"></c:Image>
                                <c:FileUpload runat="server" Hidden="true" ID="imgFileLogoUrl" ButtonOnly="true" AutoPostBack="true" OnFileSelected="imgFileLogoUrl_FileSelected" ButtonIcon="ImageAdd" ShowLabel="false" ButtonText="图片上传"></c:FileUpload>
                            </Items>
                        </c:FormRow>

                        <c:FormRow runat="server">
                            <Items>
                                <c:Image runat="server" ID="imgforeIDUrl" Label="身份证正面" ImageWidth="140" ImageHeight="80"></c:Image>
                                <c:FileUpload runat="server" Hidden="true" ID="imgfileforeIDUrl" ButtonOnly="true" AutoPostBack="true" OnFileSelected="imgfileforeIDUrl_FileSelected" ButtonIcon="ImageAdd" ShowLabel="false" ButtonText="图片上传"></c:FileUpload>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server">
                            <Items>
                                <c:Image runat="server" ID="imgbackIDUrl" Label="身份证反面" ImageWidth="140" ImageHeight="80"></c:Image>
                                <c:FileUpload runat="server" Hidden="true" ID="imgfilebackIDUrl" ButtonOnly="true" AutoPostBack="true" OnFileSelected="imgfilebackIDUrl_FileSelected" ButtonIcon="ImageAdd" ShowLabel="false" ButtonText="图片上传"></c:FileUpload>
                            </Items>
                        </c:FormRow>--%>
                        <c:FormRow ColumnWidths="33% 66%">
                            <Items>
                                <c:DropDownList runat="server" Label="是否是经销商" ID="isSupplierDDL" AutoPostBack="true" OnSelectedIndexChanged="isSupplierDDL_SelectedIndexChanged">
                                    <c:ListItem Text="否" Value="0" />
                                    <c:ListItem Text="是" Value="1" />
                                </c:DropDownList>
                                <c:DropDownList runat="server" Label="经销商" Hidden="true" ID="ddlSupplier" EnableEdit="true" />
                            </Items>
                        </c:FormRow>
                    </Items>
                </c:Form>
            </Items>
        </c:Panel>
    </form>
    <c:HiddenField runat="server" ID="personid"></c:HiddenField>
    <c:HiddenField runat="server" ID="truename"></c:HiddenField>
    <c:HiddenField runat="server" ID="cardnum"></c:HiddenField>
    <c:HiddenField runat="server" ID="hfCardNum"></c:HiddenField>
    <c:HiddenField runat="server" ID="hfCertificateNumber"></c:HiddenField>
    <c:TextBox runat="server" ID="hfimglogo" Hidden="true"></c:TextBox>
    <c:HiddenField runat="server" ID="hfimgforeIDUrl"></c:HiddenField>
    <c:HiddenField runat="server" ID="hfimgbackIDUrl"></c:HiddenField>
    <c:HiddenField runat="server" ID="hfpassword"></c:HiddenField>
</body>
</html>
