<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPCM5.aspx.cs" Inherits="SPCRpt.SPCM5" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                <table width="100%">
                    <tr width="100%">
                        <td width="100%">
                            <%--  <asp:TabContainer ID="TabContainer1" runat="server" Height="526px" Width="100%" ActiveTabIndex="0"
                            Font-Bold="true" Font-Size="Large" Style="font-weight: 700">
                            <asp:TabPanel runat="server" HeaderText="Chart" ID="TabPanel1">
                                <ContentTemplate>--%>
                            <rsweb:ReportViewer ID="rptTableView" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                                Width="100%" Style="border: 0; border-style: none;" Height="100%">
                                <LocalReport ReportPath="rptSPC.rdlc">
                                    <DataSources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                                         <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet2" />
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
