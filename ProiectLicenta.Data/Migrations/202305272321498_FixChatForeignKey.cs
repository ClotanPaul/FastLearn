namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixChatForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "Student_UserDataId", "dbo.UserDatas");
            DropIndex("dbo.Chats", new[] { "Student_UserDataId" });
            DropColumn("dbo.Chats", "HelpingStudentId");
            DropColumn("dbo.Chats", "StudentId");
            RenameColumn(table: "dbo.Chats", name: "HelpingStudent_UserDataId", newName: "HelpingStudentId");
            RenameColumn(table: "dbo.Chats", name: "Student_UserDataId", newName: "StudentId");
            RenameIndex(table: "dbo.Chats", name: "IX_HelpingStudent_UserDataId", newName: "IX_HelpingStudentId");
            AlterColumn("dbo.Chats", "StudentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Chats", "StudentId");
            AddForeignKey("dbo.Chats", "StudentId", "dbo.UserDatas", "UserDataId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "StudentId", "dbo.UserDatas");
            DropIndex("dbo.Chats", new[] { "StudentId" });
            AlterColumn("dbo.Chats", "StudentId", c => c.Int());
            RenameIndex(table: "dbo.Chats", name: "IX_HelpingStudentId", newName: "IX_HelpingStudent_UserDataId");
            RenameColumn(table: "dbo.Chats", name: "StudentId", newName: "Student_UserDataId");
            RenameColumn(table: "dbo.Chats", name: "HelpingStudentId", newName: "HelpingStudent_UserDataId");
            AddColumn("dbo.Chats", "StudentId", c => c.Int(nullable: false));
            AddColumn("dbo.Chats", "HelpingStudentId", c => c.Int());
            CreateIndex("dbo.Chats", "Student_UserDataId");
            AddForeignKey("dbo.Chats", "Student_UserDataId", "dbo.UserDatas", "UserDataId");
        }
    }
}
