namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatnieWymaganychFarmerow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buildings", "NeededFarmers", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Buildings", "NeededFarmers");
        }
    }
}
