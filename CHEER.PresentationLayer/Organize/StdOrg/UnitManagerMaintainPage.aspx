<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnitManagerMaintainPage.aspx.cs" Inherits="CHEER.PresentationLayer.Organize.StdOrg.UnitManagerMaintainPage" %>


<%@ Register Src="QueryEmpIdName.ascx" TagPrefix="uc1" TagName="QueryEmpIdName" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .customlabel span {
            color: red;
            padding-left: 5px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <c:PageManager runat="server" AutoSizePanelID="panelMain" AjaxLoadingType="Mask" FormMessageTarget="Qtip" />
        <c:Panel runat="server" ShowBorder="false" ShowHeader="false" ID="panelMain" Layout="Fit" BodyPadding="5px">
            <Items>
                <c:Panel ID="panel" runat="server" ShowHeader="false" BodyPadding="5px">
                    <Toolbars>
                        <c:Toolbar runat="server">
                            <Items>
                                <c:Button ID="btnAdd" IconFont="Plus" runat="server" Text="新增" OnClick="btnAdd_Click"></c:Button>
                                <c:Button ID="btnDelete" IconFont="Remove" runat="server" ConfirmText="确认删除吗？" Text="删除" OnClick="btnDelete_Click"></c:Button>
                                <c:Button ID="btnStop" IconFont="Stop" runat="server" Hidden="true" Text="停用" OnClick="btnStop_Click"></c:Button>
                                <c:Button ID="btnSave" IconFont="Save" runat="server" Text="保存" ValidateForms="formAttribute,formMain" OnClick="btnSave_Click"></c:Button>
                                <c:Button ID="btnEffect" IconFont="Check" runat="server" Text="生效" ValidateForms="formAttribute,formMain" OnClick="btnEffect_Click"></c:Button>
                                <c:Button ID="btnEffectIncludeSubNodes" IconFont="Check" runat="server" Text="生效包含子组织" ValidateForms="formAttribute,formMain" OnClick="btnEffectIncludeSubNodes_Click"></c:Button>
                            </Items>
                        </c:Toolbar>
                    </Toolbars>
                    <Items>
                        <c:Label ID="lblMessage" runat="server" Hidden="true" CssClass="customlabel"></c:Label>
                        <c:GroupPanel ID="groupOrg" runat="server" Title="组织信息">
                            <Items>
                                <c:Form ID="formMain" runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
                                    <Rows>
                                        <c:FormRow ColumnWidths="33% 33% 34%">
                                            <Items>
                                                <c:TextBox runat="server" ID="txtPUnitCode" Readonly="true" Label="父组织代码"></c:TextBox>
                                                <c:TextBox ID="txtPUnitName" runat="server" Readonly="true" Label="父组织名称"></c:TextBox>
                                                <c:TextBox ID="txtUnitCode" runat="server" Label="代码" Required="true" ShowRedStar="true" ></c:TextBox>
                                            </Items>
                                        </c:FormRow>
                                        <c:FormRow ColumnWidths="33% 33% 34%">
                                            <Items>
                                                <c:TextBox ID="txtUnitName" runat="server" Label="名称" Required="true" ShowRedStar="true"></c:TextBox>
                                                <c:NumberBox ID="txtIndex" runat="server" Label="序号"></c:NumberBox>
                                                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1px"></c:ContentPanel>
                                            </Items>
                                        </c:FormRow>
                                    </Rows>
                                </c:Form>
                            </Items>
                        </c:GroupPanel>
                        <c:GroupPanel ID="groupManagerAndHelper" runat="server" Title="主管助理信息" Hidden="true">
                            <Items>
                                <c:Form ID="form2" runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
                                    <Rows>
                                        <c:FormRow ColumnWidths="66% 34%">
                                            <Items>
                                                <c:UserControlConnector ID="uccManager" runat="server">
                                                    <uc1:QueryEmpIdName LblNoText="主管工号" LblNameText="主管姓名" runat="server" ID="QueryEmpIdNameManager" />
                                                </c:UserControlConnector>
                                                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1px"></c:ContentPanel>
                                            </Items>
                                        </c:FormRow>
                                        <c:FormRow ColumnWidths="66% 34%">
                                            <Items>
                                                <c:UserControlConnector ID="uccHelper" runat="server">
                                                    <uc1:QueryEmpIdName LblNoText="助理工号" LblNameText="助理姓名" runat="server" ID="QueryEmpIdNameHelper" />
                                                </c:UserControlConnector>
                                                <c:ContentPanel runat="server" ShowBorder="false" ShowHeader="false" Height="1px"></c:ContentPanel>
                                            </Items>
                                        </c:FormRow>
                                    </Rows>
                                </c:Form>
                            </Items>
                        </c:GroupPanel>
                        <c:GroupPanel ID="groupAttribute" runat="server" Title="基本信息" Hidden="true">
                            <Items>
                                <c:Form ID="formAttribute" runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
                                    <Rows>
                                        <c:FormRow ColumnWidths="33% 67%">
                                            <Items>
                                                <c:TextBox ID="txtCostCenter" runat="server" Label="成本中心"></c:TextBox>
                                                <c:TextBox ID="txtAddress" runat="server" Label="组织地址"></c:TextBox>
                                            </Items>
                                        </c:FormRow>
                                        <c:FormRow Hidden="true" ColumnWidths="33% 33% 34%">
                                            <Items>
                                                <c:NumberBox NoDecimal="true" MinValue="0" ID="txtWorks" runat="server" Label="编制人数"></c:NumberBox>
                                                <c:NumberBox NoDecimal="true" MinValue="0" ID="txtBelongWorks" runat="server" Label="含子组织编制人数"></c:NumberBox>
                                                <c:DropDownList ID="ddlOrgType" runat="server" Label="部门类型"></c:DropDownList>
                                            </Items>
                                        </c:FormRow>
                                    </Rows>
                                </c:Form>
                            </Items>
                        </c:GroupPanel>
						<c:GroupPanel runat="server" Title="基本信息">
							<Items>
								<c:Form runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
									<Rows>
										<c:FormRow>
											<Items>
												<c:TextBox runat="server" ID="txtCompanyFullname" Label="公司全称"></c:TextBox>
												<c:TextBox runat="server" ID="txtLegalPerson" Label="法人"></c:TextBox>
												<c:TextBox runat="server" ID="txtCreditCode" Label="纳税人识别号"></c:TextBox>
											</Items>
										</c:FormRow>
										<c:FormRow>
											<Items>
												<c:TextBox runat="server" ID="txtCompAddress" Label="地址"></c:TextBox>
												<c:TextBox runat="server" ID="txtOpeningBank" Label="开户行"></c:TextBox>
												<c:TextBox runat="server" ID="txtAccounts" Label="帐号"></c:TextBox>
											</Items>
										</c:FormRow>
									</Rows>
								</c:Form>
							</Items>
						</c:GroupPanel>
                        <%-- <c:GroupPanel runat="server" Title="微信菜单链接">
                            <Items>
                                <c:Form runat="server" ShowHeader="false" ShowBorder="false" LabelWidth="150">
                                    <Items>
                                        <c:Label ID="shake" runat="server" Label="通告"></c:Label>
                                        <c:Label ID="moreRedPackage" runat="server" Label="更多红包"></c:Label>
                                        <c:Label ID="userAwards" runat="server" Label="我的红包"></c:Label>
                                        <c:Label ID="userCustomers" runat="server" Label="我的客户"></c:Label>
                                        <c:Label ID="userRecommend" runat="server" Label="我要推荐"></c:Label>
                                        <c:Label ID="bookHouse" runat="server" Label="预约买房"></c:Label>
                                    </Items>
                                </c:Form>
                            </Items>
                        </c:GroupPanel>--%>
                    </Items>
                </c:Panel>
            </Items>
        </c:Panel>
        <c:HiddenField runat="server" ID="txtUnitId"></c:HiddenField>
        <c:HiddenField runat="server" ID="txtPUnitId"></c:HiddenField>
        <c:HiddenField runat="server" ID="txtPSourceUnitId"></c:HiddenField>
        <c:HiddenField ID="txtIndexHidden" runat="server"></c:HiddenField>
    </form>

</body>
</html>
