<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubServicesMaterial.aspx.cs" Inherits="CHEER.PresentationLayer.Security.Services.SubServicesMaterial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .red span {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="cellValues" />
        <asp:HiddenField runat="server" ID="Oids" />
        <c:PageManager runat="server" ID="PageManager1" AjaxLoadingType="Mask" FormMessageTarget="Qtip" AutoSizePanelID="RegionPanel1" OnCustomEvent="PageManager1_CustomEvent" />
        <c:RegionPanel runat="server" ID="RegionPanel1" ShowBorder="false">
            <Regions>
                <c:Region runat="server" ShowBorder="false" ShowHeader="false" Position="Top" Layout="Fit">
                    <Items>
                        <c:Form runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="5px 5px 2px 5px">
                            <Rows>
                                <c:FormRow runat="server" ColumnWidths="0.2 0.3 0.5">
                                    <Items>
                                        <c:TextBox runat="server" ID="txtAppNO" Label="应用程序号"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtLastTime" Label="最后运行时间"></c:TextBox>
                                        <c:TextBox runat="server" ID="txtRMachineName" Label="机器名"></c:TextBox>
                                    </Items>
                                </c:FormRow>
                            </Rows>
                        </c:Form>
                    </Items>
                </c:Region>
            </Regions>
            <Regions>
                <c:Region CssStyle="border-top:1px solid #727272" runat="server" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button runat="server" ID="cmdRefresh" Text="刷新" Icon="TableRefresh" OnClick="cmdRefresh_Click"></c:Button>
                                <c:Label runat="server" ID="lblAlertMsg" CssClass="red" Text="主服务最后运行时间间隔过长，主服务可能没有启动，请确保主服务的已启动！"></c:Label>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Items>
                        <c:Grid OnRowDataBound="UlUserGrid_RowDataBound" runat="server" ID="UlUserGrid" ShowBorder="false" ShowHeader="false" EnableColumnLines="true">
                        </c:Grid>
                    </Items>
                </c:Region>
            </Regions>
        </c:RegionPanel>
    </form>

    <c:Window runat="server" ID="EditPage" WindowPosition="Center" Target="Top" EnableIFrame="true" CloseAction="HidePostBack" Hidden="true" Width="780px" Height="450px"></c:Window>
    <script>
        Ext.onReady(function () {
            F('<%=UlUserGrid.ClientID%>').on('cellclick', function (me, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                // alert(1);
                var RUNSTATE = record.raw[8];
                var ISENABLE = record.raw[9];
                var oid = record.raw[0];
                Ext.get('<%=Oids.ClientID%>').dom.value = oid;
                if (F('<%=UlUserGrid.ClientID%>').view.getGridColumns()[cellIndex].dataIndex === '<%=CHEER.CommonLayer.eSecurity.Schema.HRSSubSRegisterSchema.RUNSTATE%>') {
                    F.confirm('', '', '<%=base.getString("ZGAIA00338")%>', function () {
                        Ext.get('<%=cellValues.ClientID%>').dom.value = RUNSTATE;
                        F.customEvent('RUNSTATE_Click');
                    });
                }
                else if (F('<%=UlUserGrid.ClientID%>').view.getGridColumns()[cellIndex].dataIndex === '<%=CHEER.CommonLayer.eSecurity.Schema.HRSSubSRegisterSchema.ISENABLE%>') {
                    Ext.get('<%=cellValues.ClientID%>').dom.value = ISENABLE;
                    F.customEvent('ISENABLE_Click');
                }
            });
        });

    function renderMe(value, metaData, record, rowIndex, colIndex, store, view) {
        var retValue;
        switch (value)
        {
            case '<%=(int)CHEER.Common.ServicesRunStateType.STOP%>':
                retValue = '<%="停止"%>';
                break;
            case '<%=(int)CHEER.Common.ServicesRunStateType.STOPING%>':
                retValue = '<%="停止中"%>';
                break;
            case '<%=(int)CHEER.Common.ServicesRunStateType.STARTING%>':
                retValue = '<%="启动中"%>';
                break;
            case '<%=(int)CHEER.Common.ServicesRunStateType.RUN%>':
                retValue = '<%="运行中"%>';
                break;
            case '<%=(int)CHEER.Common.ServicesRunStateType.LISTEN%>':
                retValue = '<%="监听中"%>';
                break;
        }
        switch (value) {
            case '<%=(int)CHEER.Common.ServicesRunStateType.STOP%>':
            case '<%=(int)CHEER.Common.ServicesRunStateType.STOPING%>':
                metaData.style = 'background-image: url(../../Image/TaskRunTest.gif); BACKGROUND-REPEAT: no-repeat; background-position: right 50%; cursor: pointer;';
                break;
            case '<%=(int)CHEER.Common.ServicesRunStateType.STARTING%>':
            case '<%=(int)CHEER.Common.ServicesRunStateType.RUN%>':
            case '<%=(int)CHEER.Common.ServicesRunStateType.LISTEN%>':
                metaData.style = 'background-image: url(../../Image/reset%2016.gif); BACKGROUND-REPEAT: no-repeat; background-position: right 50%; cursor: pointer;';
                break;
        }
        return retValue;
    }
    function isEnable(value, metaData, record, rowIndex, colIndex, store, view) {
        var retValue;
        if (value === '0') {
            retValue = '<%=base.getAlert("ZGAIA00126")%>';
            metaData.style = 'background-image: url(../../Image/TaskRunTest.gif); BACKGROUND-REPEAT: no-repeat; background-position: right 50%; cursor: pointer;';
        }
        if (value === '1') {
            retValue = '<%=base.getAlert("ZGAIA00125")%>';
            metaData.style = 'background-image: url(../../Image/reset%2016.gif); BACKGROUND-REPEAT: no-repeat; background-position: right 50%; cursor: pointer;';
        }
        return retValue;
    }
    </script>
</body>
</html>
