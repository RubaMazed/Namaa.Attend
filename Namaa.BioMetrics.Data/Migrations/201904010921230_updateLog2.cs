namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLog2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogDataInfoes", "LogOutTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.LogDataInfoes", "CommunityCenterId", c => c.Int(nullable: false));
            DropColumn("dbo.LogDataInfoes", "WorkCode");
            DropColumn("dbo.LogDataInfoes", "VerfiyMode");
            DropColumn("dbo.LogDataInfoes", "InOutMode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LogDataInfoes", "InOutMode", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "VerfiyMode", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "WorkCode", c => c.Int(nullable: false));
            DropColumn("dbo.LogDataInfoes", "CommunityCenterId");
            DropColumn("dbo.LogDataInfoes", "LogOutTime");
        }
    }
}
