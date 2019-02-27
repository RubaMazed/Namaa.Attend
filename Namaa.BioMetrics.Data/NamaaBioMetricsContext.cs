using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Namaa.BioMetrics.Model;
namespace Namaa.BioMetrics.Data
{
    public class NamaaBioMetricsContext : DbContext
    {
        public NamaaBioMetricsContext() : base("name=LocalConnection")
        {

        }
        public DbSet<CommunityCenter> CommunityCenters { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<LogDataInfo> LogDataInfos { get; set; }
    }
}
