using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using Namaa.BioMetrics.Model;

namespace Namaa.BioMertics.UI.Models
{
    public class DailyVacationViewModel
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

        #region Vacation
        [Required]
        [DisplayName("Duration")]
        public int Duration { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public string FromDate { get; set; } = DateTime.Now.ToShortDateString();
        [Required]
        [DisplayName("End Date")]
        public string ToDate { get; set; } = DateTime.Now.ToShortDateString();

        [Required]
        [DisplayName("Application Date")]
        public string ApplicationDate { get; set; } = DateTime.Now.ToShortDateString();
        //[Required]
        [DisplayName("Reason")]
        public string Reason { get; set; }
        public bool IsDelegacy { get; set; }
        public string DeputationPerson { get; set; }
        [Required]
        [DisplayName("Type")]
        public int VacationType { get; set; }
        public IList<VacationType> VacationTypes { get; set; }
        #endregion

        public static implicit operator DailyVacationViewModel(DailyVacation vacation)
        {
            return new DailyVacationViewModel
            {
                ApplicationDate = vacation.ApplicationDate.ToShortDateString(),
                FromDate = vacation.FromDate.ToShortDateString(),
                ToDate = vacation.ToDate.ToShortDateString(),
                DeputationPerson = vacation.DeputationPerson,
                Id = vacation.Id,
                Reason = vacation.Reason,
                Duration = vacation.Duration,
                VacationType = vacation.VacationTypeId,
                IsDelegacy = vacation.IsDelegacy,
                UserId = vacation.UserInfoId,
                UserName = vacation.UserInfo.FullName,
                CommunityCenterName = vacation.UserInfo.CommunityCenter.Name,
                DepartmentName = vacation.UserInfo.Department.Name,
                UserPosition = vacation.UserInfo.Position

            };
        }
        public static implicit operator DailyVacation(DailyVacationViewModel dvm)
        {
            return new DailyVacation
            {
                Id = dvm.Id,
                Duration = dvm.Duration,
                IsDelegacy = dvm.IsDelegacy,
                VacationTypeId = dvm.VacationType,
                ApplicationDate = Convert.ToDateTime(dvm.ApplicationDate),
                FromDate = Convert.ToDateTime(dvm.FromDate),
                ToDate = Convert.ToDateTime(dvm.ToDate),
                Reason = dvm.Reason,
                DeputationPerson = dvm.DeputationPerson,
                UserInfoId = dvm.UserId
            };
        }

    }
}