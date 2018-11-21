<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbstractMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.PackageSet.AbstractMaintainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="Panel1" />
        <c:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" BodyPadding="5px">
            <Items>
                <c:Form runat="server" ShowBorder="true" ShowHeader="false" LabelWidth="180px" BodyPadding="5px">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="cmdSave" IconFont="Save" Text="保存" OnClick="cmdSave_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Rows>
                        <c:FormRow runat="server" ColumnWidths="0.2 0.3 0.5">
                            <Items>
                                <c:CheckBox runat="server" ID="chkSelf" Label="所在部门" OnCheckedChanged="chkSelf_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:CheckBox runat="server" ID="chkSelfSub" Label="所在部门及子部门" OnCheckedChanged="chkSelfSub_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.2 0.3 0.5">
                            <Items>
                                <c:CheckBox runat="server" ID="chkManager" Label="担任主管部门" OnCheckedChanged="chkManager_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:CheckBox runat="server" ID="chkManagerSub" Label="担任主管部门及子部门" OnCheckedChanged="chkManagerSub_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:ContentPanel ID="ContentPanel1" runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.2 0.3 0.5">
                            <Items>
                                <c:CheckBox runat="server" ID="chkHR" Label="担任管理部门" OnCheckedChanged="chkHR_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:CheckBox runat="server" ID="chkHRSub" Label="担任管理部门及子部门" OnCheckedChanged="chkHRSub_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                            </Items>
                        </c:FormRow>
                        <c:FormRow runat="server" ColumnWidths="0.2 0.3 0.5">
                            <Items>
                                <c:CheckBox runat="server" ID="chkEmpSelf" Label="员工本人" OnCheckedChanged="chkEmpSelf_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:CheckBox runat="server" ID="chkEmpSelfExcept" Label="除员工本人" OnCheckedChanged="chkEmpSelfExcept_CheckedChanged" AutoPostBack="true"></c:CheckBox>
                                <c:ContentPanel ID="ContentPanel3" runat="server" ShowBorder="false" ShowHeader="false" Height="1"></c:ContentPanel>
                            </Items>
                        </c:FormRow>
                    </Rows>
                </c:Form>
            </Items>
        </c:Panel>

    </form>
</body>
</html>
