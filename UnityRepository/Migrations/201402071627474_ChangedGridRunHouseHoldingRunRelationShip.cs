namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedGridRunHouseHoldingRunRelationShip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GridRuns", "HouseHoldingRunId", c => c.Int());
            CreateIndex("dbo.GridRuns", "HouseHoldingRunId");
            AddForeignKey("dbo.GridRuns", "HouseHoldingRunId", "dbo.HouseHoldingRuns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GridRuns", "HouseHoldingRunId", "dbo.HouseHoldingRuns");
            DropIndex("dbo.GridRuns", new[] { "HouseHoldingRunId" });
            DropColumn("dbo.GridRuns", "HouseHoldingRunId");
        }
    }
}
