namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatnieChatkiDrwala : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buildings", "ChatkaDrwala", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Buildings", "ChatkaDrwala");
        }
    }
}
