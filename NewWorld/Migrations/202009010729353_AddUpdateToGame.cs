namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateToGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Update", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Update");
        }
    }
}
