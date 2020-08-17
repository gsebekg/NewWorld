namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserGameProperty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserGameProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Color = c.Int(nullable: false),
                        Coins = c.Long(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Game_Id = c.Int(),
                        Player_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Player_Id)
                .Index(t => t.Game_Id)
                .Index(t => t.Player_Id);
            
            AddColumn("dbo.Games", "MaxPlayers", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGameProperties", "Player_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserGameProperties", "Game_Id", "dbo.Games");
            DropIndex("dbo.UserGameProperties", new[] { "Player_Id" });
            DropIndex("dbo.UserGameProperties", new[] { "Game_Id" });
            DropColumn("dbo.Games", "MaxPlayers");
            DropTable("dbo.UserGameProperties");
        }
    }
}
