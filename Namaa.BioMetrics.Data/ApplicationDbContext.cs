using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Namaa.BioMetrics.Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Namaa.BioMetrics.Data.IdentityData;

namespace Namaa.BioMetrics.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("LocalConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<CommunityCenter> CommunityCenters { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<DailyVacation> DailyVacations { get; set; }
        public DbSet<HourlyVacation> HourlyVacations { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<LogDataInfo> LogDataInfos { get; set; }



    }
}
