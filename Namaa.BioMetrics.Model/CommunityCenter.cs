using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class CommunityCenter : BaseEntity
    {
        public CommunityCenter()
        {
            Users = new List<UserInfo>();
        }

        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string PortNum { get; set; }

        public TimeSpan BeginingCIn { get; set; }
        public TimeSpan EndingCIn { get; set; }
        public TimeSpan BeginingCOut { get; set; }
        public TimeSpan EndingCOut { get; set; }

        public int TotalHour { get; set; }
        public ICollection<UserInfo> Users { get; set; }
        public ICollection<Schedule> Schedules { get; set; }

    }
}
