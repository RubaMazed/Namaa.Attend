namespace Namaa.BioMetrics.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSchedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        BeginCheckIn = c.Time(nullable: false, precision: 7),
                        EndCheckIn = c.Time(nullable: false, precision: 7),
                        BeginCheckOut = c.Time(nullable: false, precision: 7),
                        EndCheckOut = c.Time(nullable: false, precision: 7),
                        CommunityCenterId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommunityCenters", t => t.CommunityCenterId, cascadeDelete: true)
                .Index(t => t.CommunityCenterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "CommunityCenterId", "dbo.CommunityCenters");
            DropIndex("dbo.Schedules", new[] { "CommunityCenterId" });
            DropTable("dbo.Schedules");
        }
    }
}
