namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAutoApprovalEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DocumentAutoApprovals", "Approved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentAutoApprovals", "Approved", c => c.Boolean(nullable: false));
        }
    }
}
