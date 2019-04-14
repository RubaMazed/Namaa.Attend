namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCommunityCenter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommunityCenters", "BeginingCIn", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.CommunityCenters", "EndingCIn", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.CommunityCenters", "BeginingCOut", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.CommunityCenters", "EndingCOut", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.CommunityCenters", "FromHour");
            DropColumn("dbo.CommunityCenters", "ToHour");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommunityCenters", "ToHour", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.CommunityCenters", "FromHour", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.CommunityCenters", "EndingCOut");
            DropColumn("dbo.CommunityCenters", "BeginingCOut");
            DropColumn("dbo.CommunityCenters", "EndingCIn");
            DropColumn("dbo.CommunityCenters", "BeginingCIn");
        }
    }
}
