using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Namaa.BioMetrics.Model;

namespace Namaa.BioMertics.UI.Models
{
    public class LogDataInfoViewModel
    {
        public int Id { get; set; }
        public int EnrollNum { get; set; }
        public string FullName { get; set; }
        public string LogDate { get; set; }
        public TimeSpan LogInTime { get; set; }
        public TimeSpan LogOutTime { get; set; }
        public string DepartmentName { get; set; }
        public string CommunityCenterName { get; set; }

        public static implicit operator LogDataInfoViewModel(LogDataInfo log)
        {
            return new LogDataInfoViewModel
            {
                Id = log.Id,
                EnrollNum = Convert.ToInt32(log.EnrollNum),
                FullName = log.UserInfo.FullName,
                LogDate = log.LogDate.ToShortDateString(),
                LogInTime = log.LogTime,
                LogOutTime = log.LogOutTime,
                CommunityCenterName = log.UserInfo.CommunityCenter.Name,
                DepartmentName = log.UserInfo.Department.Name
            };
        }
        public static implicit operator LogDataInfo(LogDataInfoViewModel lvm)
        {
            return new LogDataInfo
            {

                EnrollNum = lvm.EnrollNum.ToString(),
                LogDate = Convert.ToDateTime(lvm.LogDate),
                LogTime = lvm.LogInTime,
                LogOutTime = lvm.LogOutTime,
                Id = lvm.Id

            };
        }
    }
}