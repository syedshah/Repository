namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAppManCoEmailHistory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AppManCoEmailHistories", "AppManCoEmailHistoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppManCoEmailHistories", "AppManCoEmailHistoryId", c => c.Int(nullable: false));
        }
    }
}
