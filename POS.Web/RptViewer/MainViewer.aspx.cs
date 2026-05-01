using Microsoft.Reporting.WebForms;
using POS.Utilities.MultiTenant;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS.Web.RptViewer
{
    public partial class MainViewer : System.Web.UI.Page
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
                    var report = Session[WebUtil.REPORT_DATA] as ReportViewModel;
                    if (report != null)
                    {
                        GetReport(report.ReportFilePath, report.DatasetName, report.Dataset);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MainViewer] Page_Load Error: {ex.Message}");
                Response.Write($"<div style='padding:20px;background:#ffebee;border:1px solid #f44336;margin:20px;'>");
                Response.Write($"<h3 style='color:#c62828;'>Report Viewer Error</h3>");
                Response.Write($"<p><strong>Message:</strong> {ex.Message}</p>");
                Response.Write($"<p><a href='/Home/Index'>Return to Dashboard</a></p>");
                Response.Write($"</div>");
            }
        }


        public void GetReport(string reportFilePath, string dataSetName, DataSet dataSet)
        {
            try
            {
                if (System.IO.File.Exists(reportFilePath))
                {
                    ReportViewer1.LocalReport.ReportPath = reportFilePath;
                }

                //ReportViewer1.LocalReport.ReportPath = reportFilePath;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rd = new ReportDataSource(dataSetName, dataSet.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Add(rd);

                var company = new CompanyInformationViewModel()
                {
                    CompanyTitle = "Dock 27",
                    Address = "",
                    FullName = "",
                    NTN = "",
                    STRN = "",
                };
                List<CompanyInformationViewModel> companyInformation = new List<CompanyInformationViewModel>();
                companyInformation.Add(company);

                ReportDataSource rd2 = new ReportDataSource("CompanyInfo", companyInformation);
                ReportViewer1.LocalReport.DataSources.Add(rd2);
                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.Width = Unit.Percentage(100);
                ReportViewer1.Height = Unit.Percentage(100);
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MainViewer] GetReport Error: {ex.Message}");
                throw;
            }
        }
    }
}