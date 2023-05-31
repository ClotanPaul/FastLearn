namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatIssueSolvedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "IssueSolved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "IssueSolved");
        }
    }
}
