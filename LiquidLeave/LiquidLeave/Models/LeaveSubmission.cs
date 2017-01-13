using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiquidLeave.Models
{
    public class LeaveSubmission
    {
        public int LeaveApplicationId { get; set; }
        public int NumberOfDays { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public DateTime DateApplied { get; set; }
        public DateTime DateApprovedDenied { get; set; }
        public bool Status { get; set; }
        public string Comments { get; set; }
        public string UserId { get; set; }
        public Nullable<bool> Reviewed { get; set; }
    }
}