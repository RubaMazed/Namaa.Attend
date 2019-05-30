namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDuration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HourlyVacations", "Duration", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HourlyVacations", "Duration", c => c.Double(nullable: false));
        }
    }
}
