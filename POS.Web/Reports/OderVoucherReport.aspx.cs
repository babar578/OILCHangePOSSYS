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
    public partial class OderVoucherReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("OderVocher.rdlc");
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.Refresh();
                int ItemId = Convert.ToInt32(Request.QueryString["PrintId"].ToString());
                // var user = Session[WebUtil.CURRENT_USER] as UserViewModel;

                List<QuottionViewModel> list = VendorServices.GetReportQuottionById(ItemId).ToList();

                ReportDataSource dataSource = new ReportDataSource("DataSet1", list);

                var company = ItemServices.GetCompanyById();
                string imagePath = new Uri(Server.MapPath(company.LOGO)).AbsoluteUri;
                ReportParameter parameter1 = new ReportParameter("ImagePath", imagePath);


                ReportParameter parameter2 = new ReportParameter("CompanyName", company.CompanyName);

                ReportParameter parameter3 = new ReportParameter("Address", company.Address);

               ReportParameter parameter4 = new ReportParameter("PhoneNumber1", company.PhoneNumber2);


                ReportParameter parameter5 = new ReportParameter("Email", company.Email);
                ReportViewer1.LocalReport.SetParameters(parameter1);
                ReportViewer1.LocalReport.SetParameters(parameter2);
                ReportViewer1.LocalReport.SetParameters(parameter3);
                ReportViewer1.LocalReport.SetParameters(parameter4);
                ReportViewer1.LocalReport.SetParameters(parameter5);
                // ReportParameter[] rpt = new ReportParameter[1];
                //  ReportViewer1.LocalReport.SetParameters(rpt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dataSource);
            }
        }
    }
}