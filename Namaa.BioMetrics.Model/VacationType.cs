using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Namaa.BioMetrics.Model.Enums;

namespace Namaa.BioMetrics.Model
{
    public class VacationType : BaseEntity
    {

        public string Name { get; set; }
        public int Duration { get; set; }
        public VacationUnit Unit { get; set; }
    }
}
