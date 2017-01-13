using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiquidLeave.DataAccess;
using LiquidLeave.Models;
using Microsoft.AspNet.Identity.Owin;

namespace LiquidLeave.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var appRepo = new LeaveApplicationRepo();
                var model = new HomeModel();
                model.LeaveApplications = appRepo.GetLeaveApplications();
                model.Title = "Leave Applications";
                model.HelpText = appRepo.IsAdmin
                    ? "Approve or disaprove current leave applications"
                    : "View or add leave applications";
                model.IsAdmin = appRepo.IsAdmin;
                return View(model);
            }
            catch (Exception ex)
            {
                //Log the error if needed
                return View(new HomeModel() {Title = "Error", HelpText = ex.Message});
            }

        }

        /// <summary>
        /// Adds the application to the leave applications.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddApplication(LeaveSubmission model)
        {
            var appRepo = new LeaveApplicationRepo();
            model.DateApplied = DateTime.Now.Date;

            var appList = appRepo.SaveLeaveApplication(model).Result;
            return PartialView(appRepo.IsAdmin ? "AdminView" : "EmployeeView", appList);
        }

        /// <summary>
        /// Gets a single item
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApplication(int applicationId)
        {
            var appRepo = new LeaveApplicationRepo();
            var app = appRepo.GetLeaveApplication(applicationId);

            return PartialView("ManageModal", app);
        }

        /// <summary>
        /// Updates and returns only the unviewed items.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditApplication(LeaveSubmission model)
        {
            var appRepo=new LeaveApplicationRepo();

            var appList = appRepo.UpdateApplication(model);

            return PartialView(appRepo.IsAdmin ? "AdminView" : "EmployeeView", appList);

        }
    }
}