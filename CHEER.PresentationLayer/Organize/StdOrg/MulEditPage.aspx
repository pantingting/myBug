﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MulEditPage.aspx.cs" Inherits="CHEER.PresentationLayer.Organize.StdOrg.MulEditPage" %>
<%@ Register Src="../SecControls/PersonSelect.ascx" TagPrefix="uc1" TagName="PersonSelect" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .noShadow{
            -webkit-box-shadow: none !important;
            -moz-box-shadow: none !important;
            box-shadow: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="mainForm"  FormMessageTarget="Qtip" AjaxLoadingType="Mask" />
        <c:Form runat="server" ID="mainForm" LabelWidth="140px" BodyPadding="5px" ShowBorder="false" ShowHeader="false">
            <Toolbars>
                <c:Toolbar runat="server" CssClass="noShadow">
                    <Items>
                        <c:Button runat="server" Text="保存" IconFont="Save" ValidateForms="mainForm" ID="btnSave" OnClick="btnSave_Click"></c:Button>
                        <c:Button runat="server" Text="关闭" IconFont="Remove" ID="btnClose" OnClick="btnClose_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Rows>
                <c:FormRow>
                    <Items>
                        <c:DropDownList runat="server" Required="true" ID="ddlMulType" Label="管理类型"></c:DropDownList>
                    </Items>
                </c:FormRow>
                <c:FormRow>
                    <Items>
                        <c:DropDownList runat="server" ID="ddlScope" Label="管理范围"></c:DropDownList>
                    </Items>
                </c:FormRow>
                <c:FormRow>
                    <Items>
                        <c:UserControlConnector runat="server">
                            <uc1:PersonSelect runat="server" ID="PersonSelect"/>
                        </c:UserControlConnector>
                    </Items>
                </c:FormRow>
                <c:FormRow>
                    <Items>
                        <c:TextArea runat="server" ID="txtRemark" Label="备注" Height="215" MaxLength="500"  MaxLengthMessage="不得超过500字"></c:TextArea>
                    </Items>
                </c:FormRow>
            </Rows>
        </c:Form>
    </form>
</body>
</html>
