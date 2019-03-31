namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdstartDateTOUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfoes", "StartDate");
        }
    }
}
