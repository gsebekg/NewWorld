namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class island : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Islands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Place = c.Int(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Owner_Id = c.String(maxLength: 128),
                        Resources_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.Resources", t => t.Resources_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Resources_Id);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deski = c.Double(nullable: false),
                        Cegly = c.Double(nullable: false),
                        Zagle = c.Double(nullable: false),
                        StaloweBelki = c.Double(nullable: false),
                        Okna = c.Double(nullable: false),
                        Ryby = c.Double(nullable: false),
                        Sznaps = c.Double(nullable: false),
                        Ubrania = c.Double(nullable: false),
                        Kiełbasa = c.Double(nullable: false),
                        Chleb = c.Double(nullable: false),
                        Mydlo = c.Double(nullable: false),
                        Piwo = c.Double(nullable: false),
                        Konserwy = c.Double(nullable: false),
                        MaszynyDoSzycia = c.Double(nullable: false),
                        Drewno = c.Double(nullable: false),
                        Ziemniaki = c.Double(nullable: false),
                        Welna = c.Double(nullable: false),
                        Glinianka = c.Double(nullable: false),
                        Swinie = c.Double(nullable: false),
                        Zboze = c.Double(nullable: false),
                        Maka = c.Double(nullable: false),
                        Zelazo = c.Double(nullable: false),
                        Wegiel = c.Double(nullable: false),
                        Stal = c.Double(nullable: false),
                        Loj = c.Double(nullable: false),
                        Chmiel = c.Double(nullable: false),
                        Slod = c.Double(nullable: false),
                        Piasek = c.Double(nullable: false),
                        Szklo = c.Double(nullable: false),
                        Wolowina = c.Double(nullable: false),
                        Papryka = c.Double(nullable: false),
                        Gulasz = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Islands", "Resources_Id", "dbo.Resources");
            DropForeignKey("dbo.Islands", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Islands", new[] { "Resources_Id" });
            DropIndex("dbo.Islands", new[] { "Owner_Id" });
            DropTable("dbo.Resources");
            DropTable("dbo.Islands");
        }
    }
}
