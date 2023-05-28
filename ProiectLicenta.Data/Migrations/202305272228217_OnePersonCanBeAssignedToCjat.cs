namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnePersonCanBeAssignedToCjat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chats", "HelpingStudentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chats", "HelpingStudentId", c => c.Int(nullable: false));
        }
    }
}
