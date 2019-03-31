namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommunityCenters",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    IPAddress = c.String(),
                    PortNum = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserInfoes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EnrollNumber = c.String(),
                    Name = c.String(),
                    FingerIndex = c.Int(nullable: false),
                    Password = c.String(),
                    Enabled = c.Boolean(nullable: false),
                    CommunityCenter_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommunityCenters", t => t.CommunityCenter_Id)
                .Index(t => t.CommunityCenter_Id);

            CreateTable(
                "dbo.LogDataInfoes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EnrollNum = c.String(),
                    Year = c.Int(nullable: false),
                    Month = c.Int(nullable: false),
                    Day = c.Int(nullable: false),
                    Hour = c.Int(nullable: false),
                    Minutes = c.Int(nullable: false),
                    Seconds = c.Int(nullable: false),
                    WorkCode = c.Int(nullable: false),
                    VerfiyMode = c.Int(nullable: false),
                    InOutMode = c.Int(nullable: false),
                    UserInfo_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_Id)
                .Index(t => t.UserInfo_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "CommunityCenter_Id", "dbo.CommunityCenters");
            DropForeignKey("dbo.LogDataInfoes", "UserInfo_Id", "dbo.UserInfoes");
            DropIndex("dbo.LogDataInfoes", new[] { "UserInfo_Id" });
            DropIndex("dbo.UserInfoes", new[] { "CommunityCenter_Id" });
            DropTable("dbo.LogDataInfoes");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.CommunityCenters");
        }
    }
}
