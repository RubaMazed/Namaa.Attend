using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class LogDataInfo : BaseEntity
    {
        public LogDataInfo()
        {
            //  UserInfo = new UserInfo();
        }

        public string EnrollNum { get; set; }

        public DateTime LogDate { get; set; }

        public TimeSpan LogTime { get; set; }
        public TimeSpan LogOutTime { get; set; }


        //public int WorkCode { get; set; }
        //public int VerfiyMode { get; set; }
        //public int InOutMode { get; set; }

        public UserInfo UserInfo { get; set; } = null;
        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }

        public int CommunityCenterId { get; set; }

        //public CommunityCenter CommunityCenter { get; set; }
    }
}
