<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUploadFile.ascx.cs" Inherits="CHEER.PresentationLayer.Controls.ucUploadFile" %>


<c:Form runat="server" ID="formUplodFile" ShowBorder="false" ShowHeader="false">
    <Rows>
        <c:FormRow runat="server" ColumnWidths="70% 30%">
            <Items>
                <c:FileUpload AcceptFileTypes="application/vnd.ms-excel" Label="文件" ShowLabel="false" runat="server" ID="myFile" Required="true" ShowRedStar="true"></c:FileUpload>
                <c:Button runat="server" ID="btnImput" Text="上传" OnClick="btnImput_Click" ValidateForms="formUplodFile"></c:Button>
            </Items>
        </c:FormRow>
    </Rows>
</c:Form>
<c:HiddenField ID="txtUploadType" runat="server"></c:HiddenField>
<c:HiddenField ID="txtContentLength" runat="server"></c:HiddenField>
<c:HiddenField ID="txtShortName" runat="server"></c:HiddenField>
<c:HiddenField ID="txtUploadModule" runat="server"></c:HiddenField>
