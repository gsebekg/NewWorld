namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Islandupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Islands", "game_Id", c => c.Int());
            CreateIndex("dbo.Islands", "game_Id");
            AddForeignKey("dbo.Islands", "game_Id", "dbo.Games", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Islands", "game_Id", "dbo.Games");
            DropIndex("dbo.Islands", new[] { "game_Id" });
            DropColumn("dbo.Islands", "game_Id");
        }
    }
}
