namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedSessionEntityForeignKey : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sessions", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Sessions", "UserId");
            RenameColumn(table: "dbo.Sessions", name: "ApplicationUser_Id", newName: "UserId");
            AlterColumn("dbo.Sessions", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Sessions", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sessions", new[] { "UserId" });
            AlterColumn("dbo.Sessions", "UserId", c => c.String());
            RenameColumn(table: "dbo.Sessions", name: "UserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Sessions", "UserId", c => c.String());
            CreateIndex("dbo.Sessions", "ApplicationUser_Id");
        }
    }
}
