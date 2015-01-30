namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHouseHoldEntitytoDcoument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "HouseHoldId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "HouseHoldId");
        }
    }
}
