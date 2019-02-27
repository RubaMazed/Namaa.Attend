using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class LogDataInfo
    {
        public int Id { get; set; }
        public string EnrollNum { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int WorkCode { get; set; }
        public int VerfiyMode { get; set; }
        public int InOutMode { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
