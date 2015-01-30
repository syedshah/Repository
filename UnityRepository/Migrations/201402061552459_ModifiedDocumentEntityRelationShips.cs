namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDocumentEntityRelationShips : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Documents", "ApprovalId");
            DropColumn("dbo.Documents", "RejectionId");
            DropColumn("dbo.Documents", "ExportId");
            DropColumn("dbo.Documents", "HouseHoldId");
            DropColumn("dbo.Approvals", "Id");
            DropColumn("dbo.CheckOuts", "Id");
            DropColumn("dbo.Rejections", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rejections", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.CheckOuts", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.Approvals", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "HouseHoldId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "ExportId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "RejectionId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "ApprovalId", c => c.Int(nullable: false));
        }
    }
}
