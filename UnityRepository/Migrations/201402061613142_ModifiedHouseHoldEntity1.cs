namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedHouseHoldEntity1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HouseHolds", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HouseHolds", "Id", c => c.Int(nullable: false));
        }
    }
}
