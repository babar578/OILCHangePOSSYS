<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainViewer.aspx.cs" Inherits="POS.Web.RptViewer.MainViewer" %>
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
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="False" style="margin-left: 0px">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
