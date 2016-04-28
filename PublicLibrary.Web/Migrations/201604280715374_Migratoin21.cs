namespace PublicLibrary.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migratoin21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Author_ID", c => c.Int());
            CreateIndex("dbo.Books", "Author_ID");
            AddForeignKey("dbo.Books", "Author_ID", "dbo.Authors", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Author_ID", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "Author_ID" });
            DropColumn("dbo.Books", "Author_ID");
        }
    }
}
