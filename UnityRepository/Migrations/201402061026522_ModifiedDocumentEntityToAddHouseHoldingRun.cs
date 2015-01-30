namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDocumentEntityToAddHouseHoldingRun : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documents", name: "HouseHoldingRun_Id", newName: "HouseHoldingRunId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Documents", name: "HouseHoldingRunId", newName: "HouseHoldingRun_Id");
        }
    }
}
