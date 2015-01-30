namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAppManCoEmailHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppManCoEmailHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppManCoEmailHistoryId = c.Int(nullable: false),
                        AppManCoEmailId = c.Int(nullable: false),
                        ChangeInfo = c.String(),
                        ChangedBy = c.String(),
                        ChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppManCoEmails", t => t.AppManCoEmailId, cascadeDelete: true)
                .Index(t => t.AppManCoEmailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppManCoEmailHistories", "AppManCoEmailId", "dbo.AppManCoEmails");
            DropIndex("dbo.AppManCoEmailHistories", new[] { "AppManCoEmailId" });
            DropTable("dbo.AppManCoEmailHistories");
        }
    }
}
