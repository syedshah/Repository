namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHouseHoldingEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HouseHoldingRuns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grid = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HouseHolds",
                c => new
                    {
                        DocumentId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        HouseHoldDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.DocumentId);
            
            AddColumn("dbo.Documents", "HouseHoldingRun_Id", c => c.Int());
            CreateIndex("dbo.Documents", "HouseHoldingRun_Id");
            AddForeignKey("dbo.Documents", "HouseHoldingRun_Id", "dbo.HouseHoldingRuns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HouseHolds", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "HouseHoldingRun_Id", "dbo.HouseHoldingRuns");
            DropIndex("dbo.HouseHolds", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "HouseHoldingRun_Id" });
            DropColumn("dbo.Documents", "HouseHoldingRun_Id");
            DropTable("dbo.HouseHolds");
            DropTable("dbo.HouseHoldingRuns");
        }
    }
}
