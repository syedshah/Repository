namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAutoApprovalEntityName : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentAutoApprovals", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.DocumentAutoApprovals", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.DocumentAutoApprovals", "SubDocTypeId", "dbo.SubDocTypes");
            DropIndex("dbo.DocumentAutoApprovals", new[] { "DocTypeId" });
            DropIndex("dbo.DocumentAutoApprovals", new[] { "ManCoId" });
            DropIndex("dbo.DocumentAutoApprovals", new[] { "SubDocTypeId" });
            CreateTable(
                "dbo.AutoApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManCoId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SubDocTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocTypes", t => t.DocTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ManCoes", t => t.ManCoId, cascadeDelete: true)
                .ForeignKey("dbo.SubDocTypes", t => t.SubDocTypeId, cascadeDelete: true)
                .Index(t => t.DocTypeId)
                .Index(t => t.ManCoId)
                .Index(t => t.SubDocTypeId);
            
            DropTable("dbo.DocumentAutoApprovals");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocumentAutoApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManCoId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SubDocTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.AutoApprovals", "SubDocTypeId", "dbo.SubDocTypes");
            DropForeignKey("dbo.AutoApprovals", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.AutoApprovals", "DocTypeId", "dbo.DocTypes");
            DropIndex("dbo.AutoApprovals", new[] { "SubDocTypeId" });
            DropIndex("dbo.AutoApprovals", new[] { "ManCoId" });
            DropIndex("dbo.AutoApprovals", new[] { "DocTypeId" });
            DropTable("dbo.AutoApprovals");
            CreateIndex("dbo.DocumentAutoApprovals", "SubDocTypeId");
            CreateIndex("dbo.DocumentAutoApprovals", "ManCoId");
            CreateIndex("dbo.DocumentAutoApprovals", "DocTypeId");
            AddForeignKey("dbo.DocumentAutoApprovals", "SubDocTypeId", "dbo.SubDocTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentAutoApprovals", "ManCoId", "dbo.ManCoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DocumentAutoApprovals", "DocTypeId", "dbo.DocTypes", "Id", cascadeDelete: true);
        }
    }
}
