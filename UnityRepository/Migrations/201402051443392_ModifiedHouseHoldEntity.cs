namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedHouseHoldEntity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HouseHoldingRuns", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.HouseHoldingRuns", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HouseHoldingRuns", "EndDate", c => c.DateTime());
            AlterColumn("dbo.HouseHoldingRuns", "StartDate", c => c.DateTime());
        }
    }
}
