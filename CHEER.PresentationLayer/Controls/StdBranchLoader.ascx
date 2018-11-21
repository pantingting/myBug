<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StdBranchLoader.ascx.cs" Inherits="CHEER.PresentationLayer.Controls.StdBranchLoader" %>
<c:Form runat="server"  ShowBorder="false"  ShowHeader="false" ID="formBranch" ColumnWidth="100%">
    <Rows>
        <c:FormRow ID="formBranchDetail" ColumnWidths="100% 90px 0px">
            <Items>
                <c:DropDownList MaxPopHeight="240px" MatchFieldWidth="false" runat="server" ID="ddlBranch" Label="组织"></c:DropDownList>
                <c:CheckBox runat="server" ID="cbIncludeSubBranch" Text="包括子组织"></c:CheckBox>
                <c:CheckBox runat="server" Hidden="true" ID="cbIncludeStopBranch" Text="包含停用组织"></c:CheckBox>
            </Items>
        </c:FormRow>
    </Rows>
</c:Form>
<c:TextBox runat="server" ID="txtDeptInfo" Hidden="true"></c:TextBox>

