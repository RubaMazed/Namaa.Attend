namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFatherNameToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "FatherName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfoes", "FatherName");
        }
    }
}
