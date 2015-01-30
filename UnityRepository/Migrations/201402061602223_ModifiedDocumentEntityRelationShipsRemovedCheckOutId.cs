namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDocumentEntityRelationShipsRemovedCheckOutId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Documents", "CheckOutId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "CheckOutId", c => c.Int(nullable: false));
        }
    }
}
