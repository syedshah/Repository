namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GridRuns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        XmlFileId = c.Int(),
                        ApplicationId = c.Int(nullable: false),
                        Grid = c.String(),
                        IsDebug = c.Boolean(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.XmlFile", t => t.XmlFileId)
                .Index(t => t.ApplicationId)
                .Index(t => t.XmlFileId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.String(),
                        DocTypeId = c.Int(nullable: false),
                        SubDocTypeId = c.Int(nullable: false),
                        ManCoId = c.Int(nullable: false),
                        GridRunId = c.Int(),
                        CheckOutId = c.Int(nullable: false),
                        ApprovalId = c.Int(nullable: false),
                        RejectionId = c.Int(nullable: false),
                        ExportId = c.Int(nullable: false),
                        MailingName = c.String(),
                        InvestorReference = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocTypes", t => t.DocTypeId, cascadeDelete: true)
                .ForeignKey("dbo.GridRuns", t => t.GridRunId)
                .ForeignKey("dbo.ManCoes", t => t.ManCoId, cascadeDelete: true)
                .ForeignKey("dbo.SubDocTypes", t => t.SubDocTypeId, cascadeDelete: true)
                .Index(t => t.DocTypeId)
                .Index(t => t.GridRunId)
                .Index(t => t.ManCoId)
                .Index(t => t.SubDocTypeId);
            
            CreateTable(
                "dbo.Approvals",
                c => new
                    {
                        DocumentId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        ApprovedBy = c.String(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.CheckOuts",
                c => new
                    {
                        DocumentId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        CheckOutBy = c.String(),
                        CheckOutDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.DocTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentAutoApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManCoId = c.Int(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        SubDocTypeId = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocTypes", t => t.DocTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ManCoes", t => t.ManCoId, cascadeDelete: true)
                .ForeignKey("dbo.SubDocTypes", t => t.SubDocTypeId, cascadeDelete: true)
                .Index(t => t.DocTypeId)
                .Index(t => t.ManCoId)
                .Index(t => t.SubDocTypeId);
            
            CreateTable(
                "dbo.ManCoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        DomicileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Domiciles", t => t.DomicileId)
                .Index(t => t.DomicileId);
            
            CreateTable(
                "dbo.Domiciles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserDomiciles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DomicileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.DomicileId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Domiciles", t => t.DomicileId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DomicileId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Email = c.String(),
                        Comment = c.String(),
                        LastLoginDate = c.DateTime(),
                        LastPasswordChangedDate = c.DateTime(),
                        IsApproved = c.Boolean(),
                        IsLockedOut = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserManCoes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ManCoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ManCoId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.ManCoes", t => t.ManCoId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ManCoId);
            
            CreateTable(
                "dbo.InputFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentSetId = c.String(),
                        FileName = c.String(),
                        ParentFileName = c.String(),
                        Received = c.DateTime(nullable: false),
                        AlloctorGrid = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubDocTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        DocType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocTypes", t => t.DocType_Id)
                .Index(t => t.DocType_Id);
            
            CreateTable(
                "dbo.Exports",
                c => new
                    {
                        DocumentId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        ExportDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.Rejections",
                c => new
                    {
                        DocumentId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        RejectedBy = c.String(),
                        RejectionDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.Documents", t => t.DocumentId)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.IndexDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ArchiveName = c.String(),
                        ArchiveColumn = c.String(),
                        ApplicationId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        DocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Documents", t => t.DocumentId, cascadeDelete: true)
                .Index(t => t.DocumentId);
            
            CreateTable(
                "dbo.GlobalSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinPasswordLength = c.Int(nullable: false),
                        MinNonAlphaChars = c.Int(nullable: false),
                        PasswordExpDays = c.Int(nullable: false),
                        PassChangeBefore = c.Int(nullable: false),
                        NewUserPasswordReset = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsTickers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        News = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileSyncs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SyncDate = c.DateTime(nullable: false),
                        GridRunId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PasswordHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PasswordHash = c.String(),
                        UserId = c.String(maxLength: 128),
                        LogDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SecurityAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Answer = c.String(),
                        SecurityQuestionId = c.String(maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurityQuestions", t => t.SecurityQuestionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SecurityQuestionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SecurityQuestions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Question = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConFile",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InputFiles", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.XmlFile",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Domicile_Id = c.Int(),
                        OffShore = c.Boolean(nullable: false),
                        DocTypeId = c.Int(nullable: false),
                        ManCoId = c.Int(nullable: false),
                        DomicileId = c.String(),
                        BigZip = c.String(),
                        Allocated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InputFiles", t => t.Id)
                .ForeignKey("dbo.Domiciles", t => t.Domicile_Id)
                .ForeignKey("dbo.DocTypes", t => t.DocTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ManCoes", t => t.ManCoId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Domicile_Id)
                .Index(t => t.DocTypeId)
                .Index(t => t.ManCoId);
            
            CreateTable(
                "dbo.ZipFile",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BigZip = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InputFiles", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipFile", "Id", "dbo.InputFiles");
            DropForeignKey("dbo.XmlFile", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.XmlFile", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.XmlFile", "Domicile_Id", "dbo.Domiciles");
            DropForeignKey("dbo.XmlFile", "Id", "dbo.InputFiles");
            DropForeignKey("dbo.ConFile", "Id", "dbo.InputFiles");
            DropForeignKey("dbo.SecurityAnswers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SecurityAnswers", "SecurityQuestionId", "dbo.SecurityQuestions");
            DropForeignKey("dbo.PasswordHistories", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartItems", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.IndexDefinitions", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Documents", "SubDocTypeId", "dbo.SubDocTypes");
            DropForeignKey("dbo.Rejections", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.Documents", "GridRunId", "dbo.GridRuns");
            DropForeignKey("dbo.Exports", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Documents", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.DocumentAutoApprovals", "SubDocTypeId", "dbo.SubDocTypes");
            DropForeignKey("dbo.SubDocTypes", "DocType_Id", "dbo.DocTypes");
            DropForeignKey("dbo.GridRuns", "XmlFileId", "dbo.XmlFile");
            DropForeignKey("dbo.ApplicationUserDomiciles", "DomicileId", "dbo.Domiciles");
            DropForeignKey("dbo.ApplicationUserDomiciles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserManCoes", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.ApplicationUserManCoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManCoes", "DomicileId", "dbo.Domiciles");
            DropForeignKey("dbo.DocumentAutoApprovals", "ManCoId", "dbo.ManCoes");
            DropForeignKey("dbo.DocumentAutoApprovals", "DocTypeId", "dbo.DocTypes");
            DropForeignKey("dbo.CheckOuts", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.Approvals", "DocumentId", "dbo.Documents");
            DropForeignKey("dbo.GridRuns", "ApplicationId", "dbo.Applications");
            DropIndex("dbo.ZipFile", new[] { "Id" });
            DropIndex("dbo.XmlFile", new[] { "ManCoId" });
            DropIndex("dbo.XmlFile", new[] { "DocTypeId" });
            DropIndex("dbo.XmlFile", new[] { "Domicile_Id" });
            DropIndex("dbo.XmlFile", new[] { "Id" });
            DropIndex("dbo.ConFile", new[] { "Id" });
            DropIndex("dbo.SecurityAnswers", new[] { "UserId" });
            DropIndex("dbo.SecurityAnswers", new[] { "SecurityQuestionId" });
            DropIndex("dbo.PasswordHistories", new[] { "UserId" });
            DropIndex("dbo.CartItems", new[] { "DocumentId" });
            DropIndex("dbo.IndexDefinitions", new[] { "ApplicationId" });
            DropIndex("dbo.Documents", new[] { "SubDocTypeId" });
            DropIndex("dbo.Rejections", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "ManCoId" });
            DropIndex("dbo.Documents", new[] { "GridRunId" });
            DropIndex("dbo.Exports", new[] { "DocumentId" });
            DropIndex("dbo.Documents", new[] { "DocTypeId" });
            DropIndex("dbo.DocumentAutoApprovals", new[] { "SubDocTypeId" });
            DropIndex("dbo.SubDocTypes", new[] { "DocType_Id" });
            DropIndex("dbo.GridRuns", new[] { "XmlFileId" });
            DropIndex("dbo.ApplicationUserDomiciles", new[] { "DomicileId" });
            DropIndex("dbo.ApplicationUserDomiciles", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserManCoes", new[] { "ManCoId" });
            DropIndex("dbo.ApplicationUserManCoes", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ManCoes", new[] { "DomicileId" });
            DropIndex("dbo.DocumentAutoApprovals", new[] { "ManCoId" });
            DropIndex("dbo.DocumentAutoApprovals", new[] { "DocTypeId" });
            DropIndex("dbo.CheckOuts", new[] { "DocumentId" });
            DropIndex("dbo.Approvals", new[] { "DocumentId" });
            DropIndex("dbo.GridRuns", new[] { "ApplicationId" });
            DropTable("dbo.ZipFile");
            DropTable("dbo.XmlFile");
            DropTable("dbo.ConFile");
            DropTable("dbo.SecurityQuestions");
            DropTable("dbo.SecurityAnswers");
            DropTable("dbo.PasswordHistories");
            DropTable("dbo.FileSyncs");
            DropTable("dbo.NewsTickers");
            DropTable("dbo.GlobalSettings");
            DropTable("dbo.CartItems");
            DropTable("dbo.IndexDefinitions");
            DropTable("dbo.Rejections");
            DropTable("dbo.Exports");
            DropTable("dbo.SubDocTypes");
            DropTable("dbo.InputFiles");
            DropTable("dbo.ApplicationUserManCoes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ApplicationUserDomiciles");
            DropTable("dbo.Domiciles");
            DropTable("dbo.ManCoes");
            DropTable("dbo.DocumentAutoApprovals");
            DropTable("dbo.DocTypes");
            DropTable("dbo.CheckOuts");
            DropTable("dbo.Approvals");
            DropTable("dbo.Documents");
            DropTable("dbo.GridRuns");
            DropTable("dbo.Applications");
        }
    }
}
