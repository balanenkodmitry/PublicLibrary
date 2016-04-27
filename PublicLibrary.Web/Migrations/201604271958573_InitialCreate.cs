namespace PublicLibrary.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeathDate = c.DateTime(),
                        Age = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BornDate = c.DateTime(nullable: false),
                        Book_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.Book_ID)
                .Index(t => t.Book_ID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BookAvailability = c.Int(nullable: false),
                        WhosTaken_User = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Authors", "Book_ID", "dbo.Books");
            DropIndex("dbo.Authors", new[] { "Book_ID" });
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
