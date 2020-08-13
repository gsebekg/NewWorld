namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class games : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Name", c => c.String());
            AddColumn("dbo.Games", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Games", "IsBegan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "IsBegan");
            DropColumn("dbo.Games", "CreateDate");
            DropColumn("dbo.Games", "Name");
        }
    }
}
