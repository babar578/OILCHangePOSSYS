<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComsumptionReport.aspx.cs" Inherits="POS.Web.Reports.ComsumptionReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="800px" Height="600px"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
