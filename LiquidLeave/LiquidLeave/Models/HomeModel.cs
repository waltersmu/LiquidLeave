using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiquidLeave.Models
{
    /// <summary>
    /// Page model for the home page, in this case the only page.
    /// </summary>
    public class HomeModel
    {
        public List<LeaveApplication> LeaveApplications { get; set; }

        public string Title { get; set; }
        public string HelpText { get; set; }
        public bool IsAdmin { get; set; }
        
    }
}