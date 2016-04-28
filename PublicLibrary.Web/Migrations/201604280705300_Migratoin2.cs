namespace PublicLibrary.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migratoin2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "WhosTakenUser_ID", c => c.Int());
            CreateIndex("dbo.Books", "WhosTakenUser_ID");
            AddForeignKey("dbo.Books", "WhosTakenUser_ID", "dbo.Users", "ID");
            DropColumn("dbo.Books", "WhosTaken_User");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "WhosTaken_User", c => c.Int());
            DropForeignKey("dbo.Books", "WhosTakenUser_ID", "dbo.Users");
            DropIndex("dbo.Books", new[] { "WhosTakenUser_ID" });
            DropColumn("dbo.Books", "WhosTakenUser_ID");
        }
    }
}
