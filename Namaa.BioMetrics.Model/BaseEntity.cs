using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } = null;
        public string UpdatedBy { get; set; }

        public DateTime? DeletedDate { get; set; } = null;
        public string DeletedBy { get; set; }
    }
}
