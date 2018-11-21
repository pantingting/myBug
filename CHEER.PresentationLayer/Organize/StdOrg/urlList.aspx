<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="urlList.aspx.cs" Inherits="CHEER.PresentationLayer.Organize.StdOrg.urlList" %>

<%@ Register Src="QueryEmpIdName.ascx" TagPrefix="uc1" TagName="QueryEmpIdName" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .customlabel span {
            color: red;
            padding-left: 5px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="panelMain" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ShowBorder="false" ShowHeader="false" ID="panelMain" Layout="Fit" BodyPadding="5px">
            <Items>
                <c:Panel ID="panel" runat="server" ShowHeader="false" BodyPadding="5px">
                    <Items>
                        <c:Label ID="lblMessage" runat="server" Hidden="true" CssClass="customlabel"></c:Label>
                        <c:GroupPanel runat="server" Title="微信菜单链接">
                            <Items>
                                <c:Form runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
                                    <Items>
                                        <c:Label ID="shake" runat="server" Label="通告"></c:Label>
                                        <c:Label ID="payremind" runat="server" Label="缴费提醒"></c:Label>
                                        <c:Label ID="map" runat="server" Label="地图"></c:Label>
                                        <c:Label ID="supplylist" runat="server" Label="供应链"></c:Label>
                                        <c:Label ID="repair" runat="server" Label="报修"></c:Label>
                                        <c:Label ID="bookHouse" runat="server" Label="租赁"></c:Label>
                                    </Items>
                                </c:Form>
                            </Items>
                        </c:GroupPanel>
                    </Items>
                </c:Panel>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
