namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaseEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityCenters", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CommunityCenters", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommunityCenters", "CreatedBy", c => c.String());
            AddColumn("dbo.CommunityCenters", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommunityCenters", "UpdatedBy", c => c.String());
            AddColumn("dbo.CommunityCenters", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CommunityCenters", "DeletedBy", c => c.String());
            AddColumn("dbo.UserInfoes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserInfoes", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserInfoes", "CreatedBy", c => c.String());
            AddColumn("dbo.UserInfoes", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserInfoes", "UpdatedBy", c => c.String());
            AddColumn("dbo.UserInfoes", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserInfoes", "DeletedBy", c => c.String());
            AddColumn("dbo.DailyVacations", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.DailyVacations", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DailyVacations", "CreatedBy", c => c.String());
            AddColumn("dbo.DailyVacations", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DailyVacations", "UpdatedBy", c => c.String());
            AddColumn("dbo.DailyVacations", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DailyVacations", "DeletedBy", c => c.String());
            AddColumn("dbo.VacationTypes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.VacationTypes", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.VacationTypes", "CreatedBy", c => c.String());
            AddColumn("dbo.VacationTypes", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.VacationTypes", "UpdatedBy", c => c.String());
            AddColumn("dbo.VacationTypes", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.VacationTypes", "DeletedBy", c => c.String());
            AddColumn("dbo.Departments", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departments", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Departments", "CreatedBy", c => c.String());
            AddColumn("dbo.Departments", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Departments", "UpdatedBy", c => c.String());
            AddColumn("dbo.Departments", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Departments", "DeletedBy", c => c.String());
            AddColumn("dbo.HourlyVacations", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.HourlyVacations", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.HourlyVacations", "CreatedBy", c => c.String());
            AddColumn("dbo.HourlyVacations", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.HourlyVacations", "UpdatedBy", c => c.String());
            AddColumn("dbo.HourlyVacations", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.HourlyVacations", "DeletedBy", c => c.String());
            AddColumn("dbo.LogDataInfoes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.LogDataInfoes", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LogDataInfoes", "CreatedBy", c => c.String());
            AddColumn("dbo.LogDataInfoes", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LogDataInfoes", "UpdatedBy", c => c.String());
            AddColumn("dbo.LogDataInfoes", "DeletedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LogDataInfoes", "DeletedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogDataInfoes", "DeletedBy");
            DropColumn("dbo.LogDataInfoes", "DeletedDate");
            DropColumn("dbo.LogDataInfoes", "UpdatedBy");
            DropColumn("dbo.LogDataInfoes", "UpdatedDate");
            DropColumn("dbo.LogDataInfoes", "CreatedBy");
            DropColumn("dbo.LogDataInfoes", "CreationDate");
            DropColumn("dbo.LogDataInfoes", "IsActive");
            DropColumn("dbo.HourlyVacations", "DeletedBy");
            DropColumn("dbo.HourlyVacations", "DeletedDate");
            DropColumn("dbo.HourlyVacations", "UpdatedBy");
            DropColumn("dbo.HourlyVacations", "UpdatedDate");
            DropColumn("dbo.HourlyVacations", "CreatedBy");
            DropColumn("dbo.HourlyVacations", "CreationDate");
            DropColumn("dbo.HourlyVacations", "IsActive");
            DropColumn("dbo.Departments", "DeletedBy");
            DropColumn("dbo.Departments", "DeletedDate");
            DropColumn("dbo.Departments", "UpdatedBy");
            DropColumn("dbo.Departments", "UpdatedDate");
            DropColumn("dbo.Departments", "CreatedBy");
            DropColumn("dbo.Departments", "CreationDate");
            DropColumn("dbo.Departments", "IsActive");
            DropColumn("dbo.VacationTypes", "DeletedBy");
            DropColumn("dbo.VacationTypes", "DeletedDate");
            DropColumn("dbo.VacationTypes", "UpdatedBy");
            DropColumn("dbo.VacationTypes", "UpdatedDate");
            DropColumn("dbo.VacationTypes", "CreatedBy");
            DropColumn("dbo.VacationTypes", "CreationDate");
            DropColumn("dbo.VacationTypes", "IsActive");
            DropColumn("dbo.DailyVacations", "DeletedBy");
            DropColumn("dbo.DailyVacations", "DeletedDate");
            DropColumn("dbo.DailyVacations", "UpdatedBy");
            DropColumn("dbo.DailyVacations", "UpdatedDate");
            DropColumn("dbo.DailyVacations", "CreatedBy");
            DropColumn("dbo.DailyVacations", "CreationDate");
            DropColumn("dbo.DailyVacations", "IsActive");
            DropColumn("dbo.UserInfoes", "DeletedBy");
            DropColumn("dbo.UserInfoes", "DeletedDate");
            DropColumn("dbo.UserInfoes", "UpdatedBy");
            DropColumn("dbo.UserInfoes", "UpdatedDate");
            DropColumn("dbo.UserInfoes", "CreatedBy");
            DropColumn("dbo.UserInfoes", "CreationDate");
            DropColumn("dbo.UserInfoes", "IsActive");
            DropColumn("dbo.CommunityCenters", "DeletedBy");
            DropColumn("dbo.CommunityCenters", "DeletedDate");
            DropColumn("dbo.CommunityCenters", "UpdatedBy");
            DropColumn("dbo.CommunityCenters", "UpdatedDate");
            DropColumn("dbo.CommunityCenters", "CreatedBy");
            DropColumn("dbo.CommunityCenters", "CreationDate");
            DropColumn("dbo.CommunityCenters", "IsActive");
        }
    }
}
