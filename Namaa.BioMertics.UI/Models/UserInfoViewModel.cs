using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Namaa.BioMetrics.Model;
using Namaa.BioMetrics.Model.Enums;

namespace Namaa.BioMertics.UI.Models
{
    public class UserInfoViewModel
    {
        public int Id { get; set; }
        // [Required]
        public int EnrollNumber { get; set; }
        // [Required]
        public string FullName { get; set; }

        public string FatherName { get; set; }
        [Required]
        public string Position { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string BirthDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string StartDate { get; set; }

        public Gender Gender { get; set; }

        public string DepartmentName { get; set; }

        public string CommunityCentreName { get; set; }

        public int Department { get; set; }
        public int CommunityCentre { get; set; }

        public IList<CommunityCenter> CommunityCenters { get; set; }

        public IList<Department> Departments { get; set; }

        public static implicit operator UserInfoViewModel(UserInfo user)
        {
            return new UserInfoViewModel
            {
                Id = user.Id,
                EnrollNumber = Convert.ToInt32(user.EnrollNumber),
                FullName = user.FullName,
                Position = user.Position,
                BirthDate = user.BirthDate.ToShortDateString(),
                StartDate = user.StartDate.ToShortDateString(),
                CommunityCentreName = user.CommunityCenter.Name,
                DepartmentName = user.Department.Name,
                FatherName = user.FatherName

            };
        }
        public static implicit operator UserInfo(UserInfoViewModel uvm)
        {
            return new UserInfo
            {
                Id = uvm.Id,
                FullName = uvm.FullName,
                BirthDate = Convert.ToDateTime(uvm.BirthDate),
                FatherName = uvm.FatherName,
                EnrollNumber = uvm.EnrollNumber.ToString(),
                CommunityCenterId = uvm.CommunityCentre,
                DepartmentId = uvm.Department,
                Gender = uvm.Gender,
                Position = uvm.Position
            };
        }

        //public static  implicit  operator  List<UserInfo> (UserInfoViewModel uvms)
        //{
        //    return new List<UserInfo>
        //    {

        //    }
        //}
    }
}