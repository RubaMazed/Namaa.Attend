using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string EnrollNumber { get; set; }
        public string Name { get; set; }
        public int FingerIndex { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public ICollection<LogDataInfo> Logs { get; set; }
    }
}
