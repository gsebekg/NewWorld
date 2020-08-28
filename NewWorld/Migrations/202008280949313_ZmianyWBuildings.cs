namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianyWBuildings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Buildings", "Farmerzy", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Buildings", "Farmerzy", c => c.Int(nullable: false));
        }
    }
}
