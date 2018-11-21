<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectList.aspx.cs" Inherits="CHEER.PresentationLayer.eProject.ProjectList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="mainPanel" />
        <c:Panel runat="server" ShowHeader="false" ID="mainPanel" BodyPadding="5px" Layout="Fit">
            <Items>
                <c:Grid runat="server" ShowHeader="false" ID="mainGrid">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" Text="新增" ID="addBtn" OnClick="addBtn_Click" />
                                <c:Button runat="server" Text="删除" ID="delBtn" OnClick="delBtn_Click" ConfirmText="确认删除吗?" ConfirmIcon="Warning" ConfirmTitle="警告" />
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Columns>
                        <c:RowNumberField runat="server" Width="70px" HeaderTextAlign="Center" EnablePagingNumber="true" HeaderText="序号" />
                    </Columns>
                </c:Grid>
            </Items>
        </c:Panel>
        <c:Window runat="server" EnableIFrame="true" Hidden="true" Width="400px" Height="300px" ID="detailWindow" OnClose="detailWindow_Close" />
    </form>
</body>
</html>
