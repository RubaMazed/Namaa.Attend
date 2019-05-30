using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Namaa.BioMertics.UI.Models
{
    public class EmpDiscountDetails
    {
        public int EnrollNumber { get; set; }
        public string Name { get; set; }
        public List<LogDataInfoViewModel> Logs { get; set; }
        public List<HourVacationViewModel> HourVacations { get; set; }
        public List<DailyVacationViewModel> DailyVacations { get; set; }
    }
}