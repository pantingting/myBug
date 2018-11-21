<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonSelect.ascx.cs" Inherits="CHEER.PresentationLayer.Controls.PersonSelect" %>
<c:Form ID="userForm" runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="140px" Height="31px">
    <Rows>
        <c:FormRow ColumnWidths="100% 10px">
            <Items>
                <c:TextBox ID="txtName" Required="true" runat="server" Label="员工" Readonly="true"></c:TextBox>
                <c:Image ID="img" runat="server" ImageCssStyle="cursor:pointer;font-size:16px;padding:0px;    margin: 0px;" IconFont="User"></c:Image>
            </Items>
        </c:FormRow>
    </Rows>
</c:Form>
<c:TextBox ID="txtPersonId" runat="server" Hidden="true" HideMode="Display"></c:TextBox>
<c:TextBox ID="txtWNO" runat="server" Hidden="true" HideMode="Display"></c:TextBox>
