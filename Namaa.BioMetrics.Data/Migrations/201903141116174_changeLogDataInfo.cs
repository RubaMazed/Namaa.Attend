namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeLogDataInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogDataInfoes", "LogDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LogDataInfoes", "LogTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.LogDataInfoes", "Year");
            DropColumn("dbo.LogDataInfoes", "Month");
            DropColumn("dbo.LogDataInfoes", "Day");
            DropColumn("dbo.LogDataInfoes", "Hour");
            DropColumn("dbo.LogDataInfoes", "Minutes");
            DropColumn("dbo.LogDataInfoes", "Seconds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LogDataInfoes", "Seconds", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "Minutes", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "Hour", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "Day", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.LogDataInfoes", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.LogDataInfoes", "LogTime");
            DropColumn("dbo.LogDataInfoes", "LogDate");
        }
    }
}
