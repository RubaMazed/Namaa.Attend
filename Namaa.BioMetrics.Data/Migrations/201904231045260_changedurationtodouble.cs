namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedurationtodouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HourlyVacations", "Duration", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HourlyVacations", "Duration", c => c.Int(nullable: false));
        }
    }
}
