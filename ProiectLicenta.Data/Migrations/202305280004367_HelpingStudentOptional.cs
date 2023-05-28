namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpingStudentOptional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "HelpingStudentId", "dbo.UserDatas");
            DropIndex("dbo.Chats", new[] { "HelpingStudentId" });
            AlterColumn("dbo.Chats", "HelpingStudentId", c => c.Int());
            CreateIndex("dbo.Chats", "HelpingStudentId");
            AddForeignKey("dbo.Chats", "HelpingStudentId", "dbo.UserDatas", "UserDataId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "HelpingStudentId", "dbo.UserDatas");
            DropIndex("dbo.Chats", new[] { "HelpingStudentId" });
            AlterColumn("dbo.Chats", "HelpingStudentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Chats", "HelpingStudentId");
            AddForeignKey("dbo.Chats", "HelpingStudentId", "dbo.UserDatas", "UserDataId", cascadeDelete: true);
        }
    }
}
