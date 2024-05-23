using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using POS.Utilities.ReportsModel;

namespace POS.Web.Controllers
{
    public class ReportController : Controller
    {

        #region Inventry

        public ActionResult POSSaleReport()
        {
            return View();
        }

        public ActionResult StockCashReport()
        {
            return View();
        }

        public ActionResult InventoryReport()
        {
            return View();
        }
        public ActionResult ReturnToVendorReport()
        {
            return View();
        }
        public ActionResult ReturnToVendorSumamryReport()
        {
            return View();
        }

        public ActionResult ReturnToWasaHousesReport()
        {
            return View();
        }
        public ActionResult ReturnToVendorSummaryReport()
        {
            return View();
        }
        public ActionResult ComsumptionReport()
        {
            return View();
        }

        #endregion

        #region Vender to Waehouse all inventry report

        public ActionResult WastageReport()
        {


            return View();
        }
        public ActionResult IssueToDeptmentReport()
        {


            return View();
        }
        public ActionResult VenderToWarHouseReport()
        {
            IssueToLocationHeadViewModel model = new IssueToLocationHeadViewModel();


            return View(model);
        }
        #endregion

        #region InventoryBalance

        [HttpGet]
        public ActionResult FilterInventoryBalance()
        {
            var model = new FilterInventoryViewModel();
            return PartialView("_FilterInventoryBalance", model);
        }



        public ActionResult InventoryBalance(FilterInventoryViewModel model)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (model.FromDate != DateTime.MinValue && model.ToDate != DateTime.MinValue)
            {
                Session["Balance"] = model;
            }
            return View();
        }
        public ActionResult GetInventoryBalance()
        {
            return PartialView("_GetInventoryBalance");
        }


        #endregion



        #region Payments Report

        [HttpGet]
        public ActionResult FilterPayments()
        {
            var model = new FilterVendorPayment();
            return PartialView("_FilterPayments", model);
        }
        public ActionResult VendorPayments(FilterVendorPayment model)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (model.FromDate != DateTime.MinValue && model.ToDate != DateTime.MinValue)
            {
                Session["_VendorPayment"] = model;
            }
            return View();
        }
        public ActionResult GetVendorPayments()
        {
            return PartialView("_GetVendorPayments");
        }
        #endregion
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GenerateInvoiceReport(int id)
        {
            //var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var bundleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                               .First(x => x.FullName.Contains("POS"));
                    var localPath = new Uri(bundleAssembly.CodeBase).LocalPath;
                    var path = Path.Combine(Path.GetDirectoryName(localPath), "Reports", "Invoice.rdlc");

                    var order = ReportServices.GetInvoiceReport(id);

                    var report = new ReportViewModel()
                    {
                        Dataset = order,
                        DatasetName = "Invoice",
                        RdlcFileName = "Invoice.rdlc",
                        ReportFilePath = path
                    };

                    if (report.Dataset.Tables[0].Rows.Count != 0)
                    {
                        Session[WebUtil.REPORT_DATA] = report;
                        message = "Success";
                    }
                    else
                    {
                        message = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Json(message);
        }

        public ActionResult GetInvoice(int id)
        {
            var order = OrderServices.GetOrderById(id);
            if (order != null)
            {
                order.OrderItems = OrderServices.GetOrderItemsByOrderId(id);
            }

            PosExtensions.btnPrintReceipt(order);
            return View();
        }
        public void PrintInvoice(int id)
        {
            LocalReport report = new LocalReport();
            var bundleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                              .First(x => x.FullName.Contains("POS"));
            var localPath = new Uri(bundleAssembly.CodeBase).LocalPath;
            var path = Path.Combine(Path.GetDirectoryName(localPath), "Reports", "Invoice.rdlc");

            var order = ReportServices.GetInvoiceReport(id);
            report.ReportPath = path;
            report.DataSources.Add(new ReportDataSource("Invoice", order.Tables[0]));

            var company = new CompanyInformationViewModel()
            {
                CompanyTitle = "Dock 27",
                Address = "43 Block D-1 Gulberg-3 Lahore, Pakistan.",
                FullName = "Dock 27 Hospitality Private Limited.",
                NTN = "5216661-0",
                STRN = "3277876157460",
            };
            List<CompanyInformationViewModel> companyInformation = new List<CompanyInformationViewModel>();
            companyInformation.Add(company);
            ReportDataSource rd2 = new ReportDataSource("CompanyInfo", companyInformation);
            report.DataSources.Add(rd2);
            report.PrintToPrinter();
        }
        public JsonResult PrintBill(string invoiceNo)
        {
            //var user = Session[WebUtil.CURRENT_USER] as UserViewModel;

            string message = string.Empty;

            ReportDocument mRptDoc = null;

            try
            {
                DataTable order = ReportServices.PrintBill(invoiceNo);

                //var bundleAssembly = AppDomain.CurrentDomain.GetAssemblies()
                //           .First(x => x.FullName.Contains("POS"));
                //var localPath = new Uri(bundleAssembly.CodeBase).LocalPath;

                //var path = Path.Combine(Path.GetDirectoryName(localPath), "CrystalReports", "Invoice.rpt");

                //mRptDoc = new ReportDocument();
                //mRptDoc.Load(path, OpenReportMethod.OpenReportByDefault);

                mRptDoc = new POS.Reporting.CrystalReports.Invoice();

                mRptDoc.SetDataSource(order);


                if (mRptDoc.HasRecords != false)
                {
                    mRptDoc.PrintToPrinter(1, true, 1, 1);
                    message = "Success";
                }
                else
                {
                    message = "Error";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Json(message);
        }
    }
}