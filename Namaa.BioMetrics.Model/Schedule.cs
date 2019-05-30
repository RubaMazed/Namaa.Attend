using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class Schedule : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TimeSpan BeginCheckIn { get; set; }
        public TimeSpan EndCheckIn { get; set; }
        public TimeSpan BeginCheckOut { get; set; }
        public TimeSpan EndCheckOut { get; set; }

        public CommunityCenter CommunityCenter { get; set; }

        [ForeignKey("CommunityCenter")]
        public int CommunityCenterId { get; set; }

    }
}
