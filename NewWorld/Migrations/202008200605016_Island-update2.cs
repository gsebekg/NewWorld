namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Islandupdate2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Islands", new[] { "game_Id" });
            CreateIndex("dbo.Islands", "Game_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Islands", new[] { "Game_Id" });
            CreateIndex("dbo.Islands", "game_Id");
        }
    }
}
