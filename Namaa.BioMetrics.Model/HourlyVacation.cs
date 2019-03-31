using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class HourlyVacation : BaseEntity
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
        public int Duration { get; set; }
        public TimeSpan FromHour { get; set; }
        public TimeSpan ToHour { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime VacationDate { get; set; }
        public string VacationReason { get; set; }
        #endregion

    }
}
