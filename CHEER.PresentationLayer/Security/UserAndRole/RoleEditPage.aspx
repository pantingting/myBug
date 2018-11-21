<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleEditPage.aspx.cs" Inherits="CHEER.PresentationLayer.Security.UserAndRole.RoleEditPage" %>

<%@ Register Src="~/Controls/StdBranchLoader.ascx" TagPrefix="uc1" TagName="StdBranchLoader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" ID="pageManager" AutoSizePanelID="mainPanel" AjaxLoadingType="Mask" />
        <c:Panel ID="mainPanel" runat="server" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="5px">
            <Items>
                <c:Form runat="server" ID="mainForm" ShowHeader="false" BodyPadding="5px">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button ValidateForms="mainForm" runat="server" ID="btnSave" IconFont="Save" Text="保存" OnClick="btnSave_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Rows>
                        <c:FormRow>
                            <Items>
                                <c:TextBox runat="server" ID="txtRoleName" Label="角色名称" MaxLength="50"></c:TextBox>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:UserControlConnector runat="server">
                                    <uc1:StdBranchLoader runat="server" Label="归属组织" ID="StdBranchLoader" IsHaveManageUnit="true" IsShowLabel="true" IsIncludeSubBranchCanEdit="true" IsIncludeSubBranch="true" IsShowIncludeSubBranch="false" IsShowIncludeStopUnit="false" />
                                </c:UserControlConnector>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:TextArea runat="server" Label="角色描述" Height="100px" ID="txtDescription" MaxLength="100"></c:TextArea>
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:CheckBoxList runat="server" ID="chkFormAccess" Hidden="true" Label="表单权限" ColumnVertical="true" ColumnNumber="2"></c:CheckBoxList>
                            </Items>
                        </c:FormRow>
                    </Rows>
                </c:Form>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
