<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryEmpIdName.ascx.cs" Inherits="CHEER.PresentationLayer.Organize.StdOrg.QueryEmpIdName" %>
<c:Form ID="userForm" runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
    <Rows>
        <c:FormRow ColumnWidths=".5 .5">
            <Items>
                <c:Panel runat="server" ShowHeader="false" ShowBorder="false" Layout="Table">
                    <Items>
                        <c:TextBox ID="txtNo" runat="server" Label="工号" Readonly="true"></c:TextBox>
                        <c:Image ID="img" runat="server" Width="20px" ImageCssStyle="cursor:pointer;font-size:18px;margin-left:5px;" IconFont="User"></c:Image>
                        <c:Window IconFont="User" Title="  " runat="server" ID="detailWindow" Hidden="true" EnableIFrame="true" IsModal="true" IFrameUrl="UnitSelectPerson.aspx" Target="Top" Height="400" Width="400" WindowPosition="Center"></c:Window>
                    </Items>
                </c:Panel>
                <c:TextBox runat="server" ID="txtName" Label="姓名" Readonly="true"></c:TextBox>
            </Items>
        </c:FormRow>
    </Rows>
</c:Form>
<c:TextBox ID="txtPersonId" runat="server" Hidden="true" HideMode="Display"></c:TextBox>