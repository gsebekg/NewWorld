namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatnieWszystkichBudynkowFarmerskich : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buildings", "Tartak", c => c.Int(nullable: false));
            AddColumn("dbo.Buildings", "FarmaOwiec", c => c.Int(nullable: false));
            AddColumn("dbo.Buildings", "ZakladTkaczy", c => c.Int(nullable: false));
            AddColumn("dbo.Buildings", "FarmaZiemniakow", c => c.Int(nullable: false));
            AddColumn("dbo.Buildings", "DestylarniaSznapsu", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Buildings", "DestylarniaSznapsu");
            DropColumn("dbo.Buildings", "FarmaZiemniakow");
            DropColumn("dbo.Buildings", "ZakladTkaczy");
            DropColumn("dbo.Buildings", "FarmaOwiec");
            DropColumn("dbo.Buildings", "Tartak");
        }
    }
}
