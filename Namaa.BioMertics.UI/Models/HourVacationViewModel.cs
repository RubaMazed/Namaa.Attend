using Namaa.BioMetrics.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Namaa.BioMertics.UI.Models
{
    [Authorize]
    public class HourVacationViewModel
    {
        public int Id { get; set; }

        #region Users
        // [Required]
        [DisplayName("Name")]
        public string UserName { get; set; }

        public int UserId { get; set; }
        // [Required]
        [DisplayName("Position")]
        public string UserPosition { get; set; }
        //[Required]
        [DisplayName("Department")]
        public string DepartmentName { get; set; }
        //[Required]
        [DisplayName("Center")]
        public string CommunityCenterName { get; set; }
        #endregion

        #region VacationProperty
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Duration { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan FromHour { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan ToHour { get; set; }
        public string ApplicationDate { get; set; } = DateTime.Now.ToShortDateString();
        public string VacationDate { get; set; } = DateTime.Now.ToShortDateString();
        public string VacationReason { get; set; }
        #endregion

        [Required]
        [DisplayName("Type")]
        public int VacationType { get; set; }
        public IList<VacationType> VacationTypes { get; set; }

        public static implicit operator HourVacationViewModel(HourlyVacation vacation)
        {
            return new HourVacationViewModel
            {
                ApplicationDate = vacation.ApplicationDate.ToShortDateString(),
                FromHour = vacation.FromHour,
                ToHour = vacation.ToHour,
                Id = vacation.Id,
                VacationReason = vacation.VacationReason,
                Duration = vacation.Duration,
                VacationType = vacation.VacationTypeId,
                UserId = vacation.UserInfoId,
                VacationDate = vacation.VacationDate.ToShortDateString(),
                UserName = vacation.UserInfo.FullName,
                UserPosition = vacation.UserInfo.Position,
                CommunityCenterName = vacation.UserInfo.CommunityCenter.Name,
                DepartmentName = vacation.UserInfo.Department.Name

            };
        }

        public static implicit operator HourlyVacation(HourVacationViewModel hvm)
        {
            return new HourlyVacation
            {
                Id = hvm.Id,
                Duration = hvm.Duration,
                VacationTypeId = hvm.VacationType,
                ApplicationDate = Convert.ToDateTime(hvm.ApplicationDate),
                FromHour = hvm.FromHour,
                ToHour = hvm.ToHour,
                VacationReason = hvm.VacationReason,
                VacationDate = Convert.ToDateTime(hvm.VacationDate),
                UserInfoId = hvm.UserId
            };
        }
    }
}