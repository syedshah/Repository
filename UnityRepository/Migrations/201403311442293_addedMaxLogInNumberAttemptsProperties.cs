namespace UnityRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMaxLogInNumberAttemptsProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FailedLogInCount", c => c.Int());
            AddColumn("dbo.GlobalSettings", "MaxLogInAttempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettings", "MaxLogInAttempts");
            DropColumn("dbo.AspNetUsers", "FailedLogInCount");
        }
    }
}
