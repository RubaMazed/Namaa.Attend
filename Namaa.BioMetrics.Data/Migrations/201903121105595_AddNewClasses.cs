namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewClasses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserInfoes", "CommunityCenter_Id", "dbo.CommunityCenters");
            DropIndex("dbo.UserInfoes", new[] { "CommunityCenter_Id" });
            RenameColumn(table: "dbo.UserInfoes", name: "CommunityCenter_Id", newName: "CommunityCenterId");
            CreateTable(
                "dbo.DailyVacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacationTypeId = c.Int(nullable: false),
                        UserInfoId = c.Int(nullable: false),
                        Reason = c.String(),
                        ApplicationDate = c.DateTime(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        VacationReason = c.String(),
                        IsDelegacy = c.Boolean(nullable: false),
                        DeputationPerson = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId, cascadeDelete: true)
                .ForeignKey("dbo.VacationTypes", t => t.VacationTypeId, cascadeDelete: true)
                .Index(t => t.VacationTypeId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.VacationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Duration = c.Int(nullable: false),
                        Unit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HourlyVacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacationTypeId = c.Int(nullable: false),
                        UserInfoId = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        FromHour = c.Time(nullable: false, precision: 7),
                        ToHour = c.Time(nullable: false, precision: 7),
                        ApplicationDate = c.DateTime(nullable: false),
                        VacationDate = c.DateTime(nullable: false),
                        VacationReason = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId, cascadeDelete: true)
                .ForeignKey("dbo.VacationTypes", t => t.VacationTypeId, cascadeDelete: true)
                .Index(t => t.VacationTypeId)
                .Index(t => t.UserInfoId);
            
            AddColumn("dbo.CommunityCenters", "FromHour", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.CommunityCenters", "ToHour", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.CommunityCenters", "TotalHour", c => c.Int(nullable: false));
            AddColumn("dbo.UserInfoes", "FullName", c => c.String());
            AddColumn("dbo.UserInfoes", "Position", c => c.String());
            AddColumn("dbo.UserInfoes", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.UserInfoes", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserInfoes", "DepartmentId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserInfoes", "CommunityCenterId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserInfoes", "DepartmentId");
            CreateIndex("dbo.UserInfoes", "CommunityCenterId");
            AddForeignKey("dbo.UserInfoes", "DepartmentId", "dbo.Departments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserInfoes", "CommunityCenterId", "dbo.CommunityCenters", "Id", cascadeDelete: true);
            DropColumn("dbo.UserInfoes", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInfoes", "Name", c => c.String());
            DropForeignKey("dbo.UserInfoes", "CommunityCenterId", "dbo.CommunityCenters");
            DropForeignKey("dbo.HourlyVacations", "VacationTypeId", "dbo.VacationTypes");
            DropForeignKey("dbo.HourlyVacations", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoes", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.DailyVacations", "VacationTypeId", "dbo.VacationTypes");
            DropForeignKey("dbo.DailyVacations", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.HourlyVacations", new[] { "UserInfoId" });
            DropIndex("dbo.HourlyVacations", new[] { "VacationTypeId" });
            DropIndex("dbo.DailyVacations", new[] { "UserInfoId" });
            DropIndex("dbo.DailyVacations", new[] { "VacationTypeId" });
            DropIndex("dbo.UserInfoes", new[] { "CommunityCenterId" });
            DropIndex("dbo.UserInfoes", new[] { "DepartmentId" });
            AlterColumn("dbo.UserInfoes", "CommunityCenterId", c => c.Int());
            DropColumn("dbo.UserInfoes", "DepartmentId");
            DropColumn("dbo.UserInfoes", "BirthDate");
            DropColumn("dbo.UserInfoes", "Gender");
            DropColumn("dbo.UserInfoes", "Position");
            DropColumn("dbo.UserInfoes", "FullName");
            DropColumn("dbo.CommunityCenters", "TotalHour");
            DropColumn("dbo.CommunityCenters", "ToHour");
            DropColumn("dbo.CommunityCenters", "FromHour");
            DropTable("dbo.HourlyVacations");
            DropTable("dbo.Departments");
            DropTable("dbo.VacationTypes");
            DropTable("dbo.DailyVacations");
            RenameColumn(table: "dbo.UserInfoes", name: "CommunityCenterId", newName: "CommunityCenter_Id");
            CreateIndex("dbo.UserInfoes", "CommunityCenter_Id");
            AddForeignKey("dbo.UserInfoes", "CommunityCenter_Id", "dbo.CommunityCenters", "Id");
        }
    }
}
