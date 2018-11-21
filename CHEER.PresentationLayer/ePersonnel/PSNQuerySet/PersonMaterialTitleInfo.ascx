<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonMaterialTitleInfo.ascx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.PSNQuerySet.PersonMaterialTitleInfo" %>
<c:Panel ID="Panelxxx" runat="server" ShowBorder="True" EnableCollapse="true" Layout="Table" TableConfigColumns="3" ShowHeader="false">
    <Items>
        <c:Panel ID="Panel4" Title="Panel3" Layout="Form" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" ColumnWidth="0.5 0.5">
            <Items>
                <c:TextBox ID="txtTrueName" runat="server" Label="姓名"></c:TextBox>
                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="30px"></c:ContentPanel>
                <c:TextBox ID="txtEmployeeID" runat="server" Label="工号"></c:TextBox>
            </Items>
        </c:Panel>
        <c:Panel ID="Panel5" Title="Panel4" Layout="Form"
            runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" ColumnWidth="0.5 0.5">
            <Items>
                <c:TextBox ID="txtUnitName" runat="server" Label="组织"></c:TextBox>
                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="30px"></c:ContentPanel>
                <c:TextBox ID="txtAttendOnDate" runat="server" Label="入职日"></c:TextBox>
            </Items>
        </c:Panel>
        <c:Panel ID="Panel1" Title="Panel1" Layout="Form" TableRowspan="2" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false">
            <Items>
                <c:Image ID="imgPhoto" runat="server" ImageWidth="80" ImageHeight="110"></c:Image>
            </Items>
        </c:Panel>
    </Items>
</c:Panel>
