using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Namaa.BioMetrics.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namaa.BioMetrics.Model
{
    public class UserInfo : BaseEntity
    {
        public UserInfo()
        {
            //  Logs = new List<LogDataInfo>();
        }

        public string EnrollNumber { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public int FingerIndex { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public CommunityCenter CommunityCenter { get; set; }
        [ForeignKey("CommunityCenter")]
        public int CommunityCenterId { get; set; }

        public ICollection<DailyVacation> DailyVacations { get; set; } = null;
        public ICollection<HourlyVacation> HourlyVacations { get; set; } = null;
        public ICollection<LogDataInfo> Logs { get; set; } = null;
    }
}
