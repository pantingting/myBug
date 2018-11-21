<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectDetail.aspx.cs" Inherits="CHEER.PresentationLayer.eProject.ProjectDetail" %>

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
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" Text="保存" ID="saveBtn" OnClick="saveBtn_Click" ValidateForms="Form2" />
                        <c:Button runat="server"  Text="关闭" OnClientClick="C.activeWindow.close();" ID="btnClose" EnablePostBack="false"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Items>
                <c:Form runat="server" BodyPadding="5px" ShowHeader="false" ID="Form2">
                    <Items>
                        <c:FormRow>
                            <Items>
                                <c:TextBox runat="server" Label="项目名称" ID="name" ShowRedStar="true" Required="true" />
                            </Items>
                        </c:FormRow>
                        <c:FormRow>
                            <Items>
                                <c:DatePicker runat="server" Label="开始日期" ID="startTime" ShowRedStar="true" Required="true" />
                            </Items>
                        </c:FormRow>
                    </Items>
                </c:Form>
            </Items>
        </c:Panel>
    </form>
</body>
</html>
