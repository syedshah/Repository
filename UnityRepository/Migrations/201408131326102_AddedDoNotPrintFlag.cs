namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDoNotPrintFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "MailPrintFlag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "MailPrintFlag");
        }
    }
}
