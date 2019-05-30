namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIasAdministrativeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VacationTypes", "IsAdministrative", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VacationTypes", "IsAdministrative");
        }
    }
}
