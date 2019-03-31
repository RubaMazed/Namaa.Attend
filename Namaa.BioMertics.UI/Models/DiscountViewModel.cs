using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Namaa.BioMetrics.Model;
using Namaa.BioMetrics.Model.Enums;
namespace Namaa.BioMertics.UI.Models
{
    public class DiscountViewModel
    {
        public Months Month { get; set; }

        public int Year { get; set; }

        public int CommunityCenter { get; set; }

        public Dictionary<int, string> Months { get; set; }
        public List<CommunityCenter> CommunityCenters { get; set; }

        public List<DiscountDetailViewModel> Details { get; set; }

    }
}