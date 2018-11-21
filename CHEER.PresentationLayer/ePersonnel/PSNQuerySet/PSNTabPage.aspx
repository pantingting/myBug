<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PSNTabPage.aspx.cs" Inherits="CHEER.PresentationLayer.ePersonnel.PSNQuerySet.PSNTabPage" %>

<%@ Register src="PersonMaterialTitleInfo.ascx" tagname="PersonMaterialTitleInfo" tagprefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <c:PageManager runat="server" ID="PageMnager1" AutoSizePanelID="RegionPanel1"/>
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false" BodyPadding="5px">
            <Toolbars>
                <c:Toolbar runat="server">
                    <Items>
                        <c:Button runat="server" ID="btnReturn" Icon="PageBack" OnClick="btnReturn_Click"></c:Button>
                    </Items>
                </c:Toolbar>
            </Toolbars>
            <Regions>
              <%--  <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Top" Height="150px" Layout="Fit" EnableCollapse="true" Split="true">
                    <Items>
                        <c:UserControlConnector  ID="UserControlConnector1" runat ="server">
                              <uc2:PersonMaterialTitleInfo ID="PersonMaterialTitleInfo1" runat="server" />
                        </c:UserControlConnector>
                    </Items>
                </c:Region>--%>
                <c:Region runat="server" RegionPosition="Center" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit">
                    <Items>
                        <c:TabStrip runat="server" ID="PageTabs">
                            <Tabs>

                            </Tabs>
                        </c:TabStrip>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>
</body>
</html>
