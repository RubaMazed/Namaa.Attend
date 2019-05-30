using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Namaa.BioMertics.UI.Models
{
    public class DiscountDetailViewModel
    {
        public string EnrollNum { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string CommunityName { get; set; }

        public int TotalHours { get; set; }
        public int TotalMins { get; set; }
        public string TotalTime { get; set; }

        public int TotalDailyVacation { get; set; }
        public double TotalHouVacatin { get; set; }

        public int DelayHour { get; set; }
        public int DelayMins { get; set; }
        public string TotalDelay { get; set; }

        public double DiscountDay { get; set; }
        public int DiscountHour { get; set; }
    }
}