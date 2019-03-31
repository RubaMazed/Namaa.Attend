using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class DailyVacation : BaseEntity
    {


        #region VacationType
        public VacationType VacationType { get; set; }

        [ForeignKey("VacationType")]
        public int VacationTypeId { get; set; }
        #endregion

        #region Users
        public UserInfo UserInfo { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        #endregion

        #region VacationProperty
        public string Reason { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Duration { get; set; }
        public string VacationReason { get; set; }

        public bool IsDelegacy { get; set; }
        public string DeputationPerson { get; set; }

        #endregion

    }
}
