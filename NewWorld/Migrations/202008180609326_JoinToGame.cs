namespace NewWorld.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JoinToGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Password", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Password");
        }
    }
}
