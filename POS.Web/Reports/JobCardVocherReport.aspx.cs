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
    public partial class JobCardVocherReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("JobcardReports.rdlc");
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.Refresh();
               int ItemId = Convert.ToInt32(Request.QueryString["PrintId"].ToString());
               // var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                
                List<JobCardViewModelReport> list = VendorServices.GetReportjobcardById(ItemId).ToList();
           
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