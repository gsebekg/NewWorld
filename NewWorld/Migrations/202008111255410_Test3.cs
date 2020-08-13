namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Game_Id", "dbo.Games");
            DropIndex("dbo.AspNetUsers", new[] { "Game_Id" });
            CreateTable(
                "dbo.ApplicationUserGames",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Game_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Game_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Game_Id);
            
            DropColumn("dbo.AspNetUsers", "Game_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Game_Id", c => c.Int());
            DropForeignKey("dbo.ApplicationUserGames", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.ApplicationUserGames", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserGames", new[] { "Game_Id" });
            DropIndex("dbo.ApplicationUserGames", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserGames");
            CreateIndex("dbo.AspNetUsers", "Game_Id");
            AddForeignKey("dbo.AspNetUsers", "Game_Id", "dbo.Games", "Id");
        }
    }
}
