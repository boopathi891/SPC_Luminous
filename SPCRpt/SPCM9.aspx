<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPCM9.aspx.cs" Inherits="SPCRpt.SPCM9" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="10000">
                    </asp:Timer>

                    <asp:Label ID="lblFrom" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblShift" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblMachineId" runat="server" Text="" Visible="false"></asp:Label>

                     <td>
                        <asp:DropDownList ID="ddlProductType" runat="server" AutoPostBack="True" Width="91px" Visible="false" Height="20px">
                            <asp:ListItem Text="Positive" Value="Positive"></asp:ListItem>
                            <asp:ListItem Text="Negative" Value="Negative"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProductId" runat="server" AutoPostBack="True" Width="104px" Visible="false" Height="20px">
                        </asp:DropDownList>
                    </td>
                    <table width="100%">
                        <tr width="50%">
                            <td width="50%">
                                <%--  <asp:TabContainer ID="TabContainer1" runat="server" Height="526px" Width="100%" ActiveTabIndex="0"
                            Font-Bold="true" Font-Size="Large" Style="font-weight: 700">
                            <asp:TabPanel runat="server" HeaderText="Chart" ID="TabPanel1">
                                <ContentTemplate>--%>
                                <rsweb:ReportViewer ID="rptTableView" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                                    Width="100%" Style="border: 0; border-style: none;" Height="100%">
                                    <LocalReport ReportPath="rptSPC_DashBoard.rdlc">
                                        <DataSources>
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet2" />
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" Name="XBarChart" />
                                        </DataSources>
                                    </LocalReport>
                                </rsweb:ReportViewer>
                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                    TypeName="SPCRpt.dtSPCTableAdapters.SP_SPC_ReportsTableAdapter">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblFrom" Name="Date" PropertyName="Text"
                                            Type="String" />
                                        <asp:ControlParameter ControlID="lblShift" Name="Shift" PropertyName="Text"
                                            Type="String" />
                                        <asp:ControlParameter ControlID="lblMachineId" Name="MachineId" PropertyName="Text"
                                            Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                    TypeName="SPCRpt.dtSPCTableAdapters.SP_SPC_Reports_ThicknessTableAdapter">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblFrom" Name="Date" PropertyName="Text"
                                            Type="String" />
                                        <asp:ControlParameter ControlID="lblShift" Name="Shift" PropertyName="Text"
                                            Type="String" />
                                        <asp:ControlParameter ControlID="lblMachineId" Name="MachineId"
                                            PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="SPCRpt.dtSPCTableAdapters.SP_XBar_R_ChartTableAdapter">
                                      <SelectParameters>
                                            <asp:ControlParameter Name="ProductId" ControlID="ddlProductId" PropertyName="SelectedValue"
                                                Type="String" DefaultValue="2.3STT" />
                                            <asp:ControlParameter Name="ProductType" ControlID="ddlProductType" PropertyName="SelectedValue"
                                                Type="String" DefaultValue="Negative"/>
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                <%--</ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>--%>
                        </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
