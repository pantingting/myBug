<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PSNAdd.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.PSNQuerySet.PSNAdd" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .x-boundlist-list-ct {
            white-space: nowrap;
            overflow-x: hidden !important;
        }

        .x-column-header-inner {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager ID="PageManager1" runat="server" AutoSizePanelID="mainRegions" FormLabelWidth="160px" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:RegionPanel ID="mainRegions" runat="server" BodyPadding="5px" ShowBorder="false">
            <Regions>
                <c:Region ID="RegionItem" runat="server" Layout="Region" ShowBorder="true" ShowHeader="false" Position="Center">
                    <Items>
                        <c:Form runat="server" ID="formCondition" ShowHeader="false" ShowBorder="false" BodyPadding="5px 5px 0px 5px" RegionPosition="Top">
                            <Toolbars>
                                <c:Toolbar ID="toolBar" runat="server">
                                    <Items>
                                        <c:Button ID="btnAdd" runat="server" Text="新增" IconFont="Plus" OnClick="btnAdd_Click"></c:Button>
                                        <c:Button ID="btnEffect" runat="server" Text="保存" EnablePostBack="false" OnClientClick="return datePick();" IconFont="Save" OnClick="btnEffect_Click"  ValidateForms="form"></c:Button>
                                        <c:Button ID="btnReturn" runat="server" Text="返回" IconFont="ChevronLeft" OnClick="btnReturn_Click"></c:Button>
                                    </Items>
                                </c:Toolbar>
                            </Toolbars>
                        </c:Form>
                        <c:Form runat="server" ID="form" ShowBorder="false" ShowHeader="false" BodyPadding="0px 5px 0 5px" RegionPosition="Center">
                            <Items>
                               <%-- <c:FormRow runat="server">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtpassword" TextMode="Password" Label="密码" ShowRedStar="true" Required="true"></c:TextBox>
                                        <c:DropDownList runat="server" ID="ddlstatus1" Label="状态" Required="true" ShowRedStar="true">
                                            <c:ListItem Value="" Text="" />
                                            <c:ListItem Value="-2" Text="完善资料" />
                                            <c:ListItem Value="-1" Text="个人认证" />
                                            <c:ListItem Value="0" Text="待审核" />
                                            <c:ListItem Value="1" Text="已生效" />
                                            <c:ListItem Value="2" Text="已开除" />
                                        </c:DropDownList>
                                        <c:Label runat="server"></c:Label>
                                    </Items>
                                </c:FormRow>

                                <c:FormRow runat="server">
                                    <Items>
                                        <c:Image runat="server" ID="imgLogoUrl" Label="个人头像" ImageWidth="140" ImageHeight="80"></c:Image>
                                        <c:FileUpload runat="server" ID="imgFileLogoUrl" ButtonOnly="true" AutoPostBack="true" OnFileSelected="imgFileLogoUrl_FileSelected" ButtonIcon="ImageAdd" ShowLabel="false" ButtonText="图片上传"></c:FileUpload>
                                    </Items>
                                </c:FormRow>

                                <c:FormRow runat="server">
                                    <Items>
                                        <c:Image runat="server" ID="imgforeIDUrl" Label="身份证正面" ImageWidth="140" ImageHeight="80"></c:Image>
                                        <c:FileUpload runat="server" ID="imgfileforeIDUrl" ButtonOnly="true" AutoPostBack="true" OnFileSelected="imgfileforeIDUrl_FileSelected" ButtonIcon="ImageAdd" ShowLabel="false" ButtonText="图片上传"></c:FileUpload>
                                    </Items>
                                </c:FormRow>
                                <c:FormRow runat="server">
                                    <Items>
                                        <c:Image runat="server" ID="imgbackIDUrl" Label="身份证反面" ImageWidth="140" ImageHeight="80"></c:Image>
                                        <c:FileUpload runat="server" ID="imgfilebackIDUrl" ButtonOnly="true" AutoPostBack="true" OnFileSelected="imgfilebackIDUrl_FileSelected" ButtonIcon="ImageAdd" ShowLabel="false" ButtonText="图片上传"></c:FileUpload>
                                    </Items>
                                </c:FormRow>--%>
                                <c:FormRow ColumnWidths="33% 66%">
                                    <Items>
                                        <c:DropDownList runat="server" Label="是否是经销商" ID="isSupplierDDL" AutoPostBack="true" OnSelectedIndexChanged="isSupplierDDL_SelectedIndexChanged">
                                            <c:ListItem  Text="否" Value="0"/>
                                            <c:ListItem Text="是" Value="1" />
                                        </c:DropDownList>
                                        <c:DropDownList runat="server" Label="经销商" Hidden="true" ID="ddlSupplier" EnableEdit="true" />
                                    </Items>
                                </c:FormRow>
                            </Items>
                        </c:Form>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
    <script>
        function datePick() {
            var dataController = document.getElementById('mainRegions_RegionItem_formCondition_ctl02_ctl00_ctl00_7f2894a9-b707-453f-a0fc-5ec6fa10a10c-inputEl');
            console.log(getNowFormatDate());
            console.log(dataController.value);
            if (getNowFormatDate() < dataController.value) {
                alert('出生日期不能选择未来日期！');
                return false;
            } else {
                __doPostBack('<%=btnEffect.UniqueID %>');
            }
        }
        function getNowFormatDate() {
        var date = new Date();
        var seperator1 = "-";
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = year + seperator1 + month + seperator1 + strDate;
        return currentdate;
       }
    </script>
</body>
</html>
