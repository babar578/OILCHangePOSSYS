using Microsoft.Reporting.WebForms;
using POS.Utilities.MultiTenant;
using POS.Utilities.ReportsModel;
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
    public partial class CurrentStock : ReportBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // === MULTI-TENANT FIX: Ensure tenant context is set ===
            if (!TenantContext.HasTenant)
            {
                // Check if user is logged in
                var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                if (user == null)
                {
                    Response.Redirect("~/Account/Login");
                    return;
                }

                // Get tenant from session and set context
                var tenantId = Session["TenantId"] as int?;
                if (tenantId.HasValue)
                {
                    var tenant = TenantCache.GetTenant(tenantId.Value);
                    if (tenant != null && tenant.IsActive)
                    {
                        TenantContext.CurrentTenant = tenant;
                        System.Diagnostics.Debug.WriteLine($"[CurrentStock] Tenant context set: {tenant.TenantName}");
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
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("CurrentStockReport.rdlc");
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.Refresh();

                DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"].ToString());
                DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"].ToString());

                string filters = "| Date: " + fromDate.ToString(POS.Utilities.WebConfigSettings.DateFormat)
                  + " - " + toDate.ToString(POS.Utilities.WebConfigSettings.DateFormat) + " |";
                List<Fn_ReportWareHouseViewModel> list = ReportServices.GetInventoryBalanceReport(fromDate, toDate, null, null).ToList();
                ReportDataSource dataSource = new ReportDataSource("DataSet1", list);

                ReportParameter[] rpt = new ReportParameter[1];
                rpt[0] = new ReportParameter("filters", filters);
                ReportViewer1.LocalReport.SetParameters(rpt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dataSource);
            }
        }
    }
}