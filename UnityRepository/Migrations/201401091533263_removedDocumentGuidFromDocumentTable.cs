namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedDocumentGuidFromDocumentTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Documents", "DocumentGuid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "DocumentGuid", c => c.String());
        }
    }
}
