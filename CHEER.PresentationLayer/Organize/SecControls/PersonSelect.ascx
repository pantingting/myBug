<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonSelect.ascx.cs" Inherits="CHEER.PresentationLayer.Organize.SecControls.PersonSelect" %>
<c:Form ID="userForm" runat="server" ShowHeader="false" ShowBorder="false">
    <Rows>
        <c:FormRow>
            <Items>
                <c:Panel runat="server" ShowHeader="false" ShowBorder="false" Layout="Table">
                    <Items>
                        <c:TextBox ColumnWidth="100%" ID="txtName" Required="true" runat="server" Label="员工" Readonly="true" LabelWidth="140px"></c:TextBox>
                        <c:Image ID="img" runat="server" ImageCssStyle="cursor:pointer;font-size:18px;margin-left:5px;" Width="20px" IconFont="User"></c:Image>
                        <c:Window Title="  " runat="server" ID="detailWindow" Hidden="true" EnableIFrame="true" IsModal="true" IFrameUrl="PersonShowPage.aspx" Target="Top" Height="400" Width="400" WindowPosition="Center"></c:Window>
                    </Items>
                </c:Panel>
            </Items>
        </c:FormRow>
    </Rows>
</c:Form>
<c:TextBox ID="txtPersonId" runat="server" Hidden="true" HideMode="Display"></c:TextBox>