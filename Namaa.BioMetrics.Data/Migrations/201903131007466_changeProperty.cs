namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CommunityCenters", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.CommunityCenters", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.UserInfoes", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.UserInfoes", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.DailyVacations", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.DailyVacations", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.VacationTypes", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.VacationTypes", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.Departments", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.Departments", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.HourlyVacations", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.HourlyVacations", "DeletedDate", c => c.DateTime());
            AlterColumn("dbo.LogDataInfoes", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.LogDataInfoes", "DeletedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogDataInfoes", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LogDataInfoes", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.HourlyVacations", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.HourlyVacations", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.VacationTypes", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.VacationTypes", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DailyVacations", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DailyVacations", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserInfoes", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserInfoes", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CommunityCenters", "DeletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CommunityCenters", "UpdatedDate", c => c.DateTime(nullable: false));
        }
    }
}
