using Microsoft.Reporting.WebForms;
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
            if (!IsPostBack)
            {
                var report = Session[WebUtil.REPORT_DATA] as ReportViewModel;
                if (report != null)
                {
                    GetReport(report.ReportFilePath, report.DatasetName, report.Dataset);
                }

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
            catch (Exception)
            {
                throw;
            }

        }
    }
}