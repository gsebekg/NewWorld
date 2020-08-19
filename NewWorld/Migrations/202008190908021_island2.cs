namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class island2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Islands", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Islands", new[] { "Owner_Id" });
            AddColumn("dbo.Islands", "Glinianka", c => c.Int(nullable: false));
            AddColumn("dbo.Islands", "Zelazo", c => c.Int(nullable: false));
            AddColumn("dbo.Islands", "Ziemniaki", c => c.Boolean(nullable: false));
            AddColumn("dbo.Islands", "Zboze", c => c.Boolean(nullable: false));
            AddColumn("dbo.Islands", "Chmiel", c => c.Boolean(nullable: false));
            AddColumn("dbo.Islands", "Papryka", c => c.Boolean(nullable: false));
            AddColumn("dbo.Islands", "Property_Id", c => c.Int());
            CreateIndex("dbo.Islands", "Property_Id");
            AddForeignKey("dbo.Islands", "Property_Id", "dbo.UserGameProperties", "Id");
            DropColumn("dbo.Islands", "Owner_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Islands", "Owner_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Islands", "Property_Id", "dbo.UserGameProperties");
            DropIndex("dbo.Islands", new[] { "Property_Id" });
            DropColumn("dbo.Islands", "Property_Id");
            DropColumn("dbo.Islands", "Papryka");
            DropColumn("dbo.Islands", "Chmiel");
            DropColumn("dbo.Islands", "Zboze");
            DropColumn("dbo.Islands", "Ziemniaki");
            DropColumn("dbo.Islands", "Zelazo");
            DropColumn("dbo.Islands", "Glinianka");
            CreateIndex("dbo.Islands", "Owner_Id");
            AddForeignKey("dbo.Islands", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
