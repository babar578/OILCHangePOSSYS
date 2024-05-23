using Microsoft.Reporting.WebForms;
using POS.Utilities.Services;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS.Web.Reports
{
    public partial class ReturnToWasaHousesReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReturnTowareHouse.rdlc");
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.Refresh();

                DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"].ToString());
                DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"].ToString());
                //int ItemId = Convert.ToInt32(Request.QueryString["ItemId"].ToString());
                //int UnitId = Convert.ToInt32(Request.QueryString["UnitId"].ToString());
                string filters = "| Date: " + fromDate.ToString(POS.Utilities.WebConfigSettings.DateFormat)
                  + " - " + toDate.ToString(POS.Utilities.WebConfigSettings.DateFormat) + " |";
                List<ReturnToWareHouseViewModel> list = ReportServices.GetReturnTowareHouseReport(fromDate, toDate).ToList();
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