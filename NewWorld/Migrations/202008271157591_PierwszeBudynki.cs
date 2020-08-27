namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PierwszeBudynki : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RezydencjaFarmerow = c.Int(nullable: false),
                        Farmerzy = c.Int(nullable: false),
                        ChatkaRybacka = c.Int(nullable: false),
                        FarmersSatisfaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resources", t => t.FarmersSatisfaction_Id)
                .Index(t => t.FarmersSatisfaction_Id);
            
            AddColumn("dbo.Islands", "Buildings_Id", c => c.Int());
            CreateIndex("dbo.Islands", "Buildings_Id");
            AddForeignKey("dbo.Islands", "Buildings_Id", "dbo.Buildings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Islands", "Buildings_Id", "dbo.Buildings");
            DropForeignKey("dbo.Buildings", "FarmersSatisfaction_Id", "dbo.Resources");
            DropIndex("dbo.Islands", new[] { "Buildings_Id" });
            DropIndex("dbo.Buildings", new[] { "FarmersSatisfaction_Id" });
            DropColumn("dbo.Islands", "Buildings_Id");
            DropTable("dbo.Buildings");
        }
    }
}
