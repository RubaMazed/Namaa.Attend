namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVacationType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VacationTypes", "IsDiscount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VacationTypes", "IsDiscount");
        }
    }
}
