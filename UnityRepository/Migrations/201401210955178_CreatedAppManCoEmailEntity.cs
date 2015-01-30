namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedAppManCoEmailEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppManCoEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ManCoId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        AccountNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.DocTypes", t => t.DocTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ManCoes", t => t.ManCoId, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.DocTypeId)
                .Index(t => t.ManCoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppManCoEmails", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.AppManCoEmails", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.AppManCoEmails", "ApplicationId", "dbo.Applications");
            DropIndex("dbo.AppManCoEmails", new[] { "ManCoId" });
            DropIndex("dbo.AppManCoEmails", new[] { "DocTypeId" });
            DropIndex("dbo.AppManCoEmails", new[] { "ApplicationId" });
            DropTable("dbo.AppManCoEmails");
        }
    }
}
