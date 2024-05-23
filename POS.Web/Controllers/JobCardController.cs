using POS.Utilities.Services;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS.Web.Controllers
{
    public class JobCardController : Controller
    {
        //ghgch
        // GET: JobCard
        #region Job Card 

        public ActionResult Units()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var units = ItemServices.GetAllJobCards();
            return View("Units", units);
        }

        [HttpGet]
        public ActionResult Unit(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            VehicaljobCardViewModel model = new VehicaljobCardViewModel();

            if (id != null)
            {
                model = ItemServices.GetVehicalJobCardById(id ?? 0);
            }

            return View("_Unit", model);
        }

        [HttpPost]
        public JsonResult AddUnit(VehicaljobCardViewModel model)
        {
            ResponseDto response = new ResponseDto();
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    response = ItemServices.AddJobCard(model);
                }
                else
                {
                    add = ItemServices.UpdateJobCard(model);
                }

              

            }
            catch (Exception ex)
            {
                response.Message= ex.Message.ToString();
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult DeleteUnit(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteJobCard(id);
                if (del)
                {
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


        #endregion
    }
}