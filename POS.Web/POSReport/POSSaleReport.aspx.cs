using Microsoft.Reporting.WebForms;
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
    public partial class POSSaleReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("POSSale.rdlc");
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.Refresh();
                DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"].ToString());
                DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"].ToString());
                //int ItemId = Convert.ToInt32(Request.QueryString["ItemId"].ToString());
                //int UnitId = Convert.ToInt32(Request.QueryString["UnitId"].ToString());
                string filters = "| Date: " + fromDate.ToString(POS.Utilities.WebConfigSettings.DateFormat)
                  + " - " + toDate.ToString(POS.Utilities.WebConfigSettings.DateFormat) + " |";

                List<POSOderSaleReportViewModel> list = ReportServices.GetPOSSaleReport(fromDate, toDate).ToList();
           
                ReportDataSource dataSource = new ReportDataSource("DataSet1", list);


                var company = ItemServices.GetCompanyById();
                string imagePath = new Uri(Server.MapPath(company.LOGO)).AbsoluteUri;
                ReportParameter parameter = new ReportParameter("ImagePath", imagePath);


                ReportParameter parameter1 = new ReportParameter("CompanyName", company.CompanyName);

                ReportParameter parameter3 = new ReportParameter("Address", company.Address);

                ReportParameter parameter4 = new ReportParameter("Address", company.PhoneNumber1);
                ReportViewer1.LocalReport.SetParameters(parameter1);
                ReportViewer1.LocalReport.SetParameters(parameter);
                // ReportParameter[] rpt = new ReportParameter[1];
                //  ReportViewer1.LocalReport.SetParameters(rpt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dataSource);
            }
        }
    }
}