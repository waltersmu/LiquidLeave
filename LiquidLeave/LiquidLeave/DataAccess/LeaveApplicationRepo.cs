using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LiquidLeave.Constants;
using LiquidLeave.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LiquidLeave.DataAccess
{
    /// <summary>
    /// Data access layer for the leave applications.
    /// </summary>
    public class LeaveApplicationRepo
    {
        public bool IsAdmin { get; set; }

        public ApplicationUser CurrentUser { get; set; }
        /// <summary>
        /// Save a leave application and return the updated list.
        /// </summary>
        /// <param name="application">The leave application model to save</param>
        /// <returns>A list of the leave application</returns>
        public async Task<List<LeaveApplication>> SaveLeaveApplication(LeaveSubmission application)
        {

            using (var db = new LiquidLeaveEntities())
            {
                try
                {
                    var addModel = new LeaveApplication
                    {
                        Comments = application.Comments,
                        DateApplied = application.DateApplied,
                        DateFrom = DateTime.Parse(application.DateFrom),
                        DateTo = DateTime.Parse(application.DateTo),
                        NumberOfDays = application.NumberOfDays,
                        UserId = CurrentUser.Id,

                    };
                    db.LeaveApplications.Add(addModel);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //Log error
                    throw;
                }
                //Returns the refreshed list of the applications depending on the user's role
                return GetLeaveApplications();
            }
        }

        /// <summary>
        /// Gets all the leave applications
        /// </summary>
        /// <returns>Leave Applications</returns>
        public List<LeaveApplication> GetLeaveApplications()
        {
            using (var db = new LiquidLeaveEntities())
            {
                var applications = from apps in db.LeaveApplications.Include("AspNetUser")
                                   orderby apps.DateApplied ascending
                                   select apps;
                //Change the result set depending on the Role the user is in. In this case very straight forward. 
                return IsAdmin ? applications.Where(x => !x.Reviewed).ToList() : applications.Where(x => x.UserId == CurrentUser.Id).ToList();
            }

        }
        /// <summary>
        /// Gets single leave application
        /// </summary>
        /// <param name="leaveApplicationId"></param>
        /// <returns></returns>
        public LeaveApplication GetLeaveApplication(int leaveApplicationId)
        {
            using (var db = new LiquidLeaveEntities())
            {
                return db.LeaveApplications.Include("AspNetUser").SingleOrDefault(x => x.LeaveApplicationId == leaveApplicationId);

            }
        }

        public List<LeaveApplication> UpdateApplication(LeaveSubmission app)
        {
            LeaveApplication application;
            using (var db =new LiquidLeaveEntities())
            {
                application = db.LeaveApplications.Find(app.LeaveApplicationId);
                if (application == null) return null;
                application.Reviewed = true;
                application.Status = app.Status;
                application.DateApprovedDenied=DateTime.Now;
                application.Comments = app.Comments;
                
                db.SaveChanges();
            }

           return GetLeaveApplications();
        }

        public LeaveApplicationRepo()
        {
            IsAdmin = HttpContext.Current.User.IsInRole(Roles.AMINISTRATOR);
            CurrentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        }

    }
}