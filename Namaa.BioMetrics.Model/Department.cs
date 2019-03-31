using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Namaa.BioMetrics.Model
{
    public class Department : BaseEntity
    {
        public Department()
        {

        }

        public string Name { get; set; }

        public ICollection<UserInfo> Users { get; set; } = null;
    }
}
