namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeLogDataInfo1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogDataInfoes", "UserInfo_Id", "dbo.UserInfoes");
            DropIndex("dbo.LogDataInfoes", new[] { "UserInfo_Id" });
            RenameColumn(table: "dbo.LogDataInfoes", name: "UserInfo_Id", newName: "UserInfoId");
            AlterColumn("dbo.LogDataInfoes", "UserInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.LogDataInfoes", "UserInfoId");
            AddForeignKey("dbo.LogDataInfoes", "UserInfoId", "dbo.UserInfoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogDataInfoes", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.LogDataInfoes", new[] { "UserInfoId" });
            AlterColumn("dbo.LogDataInfoes", "UserInfoId", c => c.Int());
            RenameColumn(table: "dbo.LogDataInfoes", name: "UserInfoId", newName: "UserInfo_Id");
            CreateIndex("dbo.LogDataInfoes", "UserInfo_Id");
            AddForeignKey("dbo.LogDataInfoes", "UserInfo_Id", "dbo.UserInfoes", "Id");
        }
    }
}
