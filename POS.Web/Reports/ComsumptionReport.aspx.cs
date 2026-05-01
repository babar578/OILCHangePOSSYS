using Microsoft.Reporting.WebForms;
using POS.Utilities.MultiTenant;
using POS.Utilities.Services;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS.Web.Reports
{
    public partial class ComsumptionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // === MULTI-TENANT: Ensure tenant context is set ===
                if (!TenantContext.HasTenant)
                {
                    var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                    if (user == null)
                    {
                        Response.Redirect("~/Account/Login");
                        return;
                    }

                    var tenantId = Session["TenantId"] as int?;
                    if (tenantId.HasValue)
                    {
                        var tenant = TenantCache.GetTenant(tenantId.Value);
                        if (tenant != null && tenant.IsActive)
                        {
                            TenantContext.CurrentTenant = tenant;
                        }
                        else
                        {
                            Response.Redirect("~/Account/Login");
                            return;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Account/Login");
                        return;
                    }
                }
                // === END MULTI-TENANT FIX ===

                if (!IsPostBack)
                {
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("ComsumptionReport.rdlc");
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    ReportViewer1.LocalReport.Refresh();
                    DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"].ToString());
                    DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"].ToString());
                //int ItemId = Convert.ToInt32(Request.QueryString["ItemId"].ToString());
                //int UnitId = Convert.ToInt32(Request.QueryString["UnitId"].ToString());
                string filters = "| Date: " + fromDate.ToString(POS.Utilities.WebConfigSettings.DateFormat)
                  + " - " + toDate.ToString(POS.Utilities.WebConfigSettings.DateFormat) + " |";
                List<ComsumptionReportViewModel> list = ReportServices.GetComsumptionReport(fromDate, toDate).ToList();
                ReportDataSource dataSource = new ReportDataSource("DataSet1", list);
                ReportParameter[] rpt = new ReportParameter[1];
                rpt[0] = new ReportParameter("filters", filters);
                ReportViewer1.LocalReport.SetParameters(rpt);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(dataSource);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ComsumptionReport] Error: {ex.Message}");
                Response.Write($"<div style='padding:20px;background:#ffebee;border:1px solid #f44336;margin:20px;'>");
                Response.Write($"<h3 style='color:#c62828;'>Report Error</h3>");
                Response.Write($"<p><strong>Message:</strong> {ex.Message}</p>");
                Response.Write($"<p><a href='/Home/Index'>Return to Dashboard</a></p>");
                Response.Write($"</div>");
            }
        }
    }
}