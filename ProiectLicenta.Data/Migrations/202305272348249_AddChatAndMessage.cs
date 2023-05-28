namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChatAndMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        Topic = c.String(),
                        StudentId = c.Int(nullable: false),
                        HelpingStudentId = c.Int(nullable: false),
                        SubChapterId = c.Int(nullable: false),
                        UserData_UserDataId = c.Int(),
                    })
                .PrimaryKey(t => t.ChatId)
                .ForeignKey("dbo.UserDatas", t => t.HelpingStudentId, cascadeDelete: false)
                .ForeignKey("dbo.UserDatas", t => t.StudentId, cascadeDelete: false)
                .ForeignKey("dbo.SubChapters", t => t.SubChapterId, cascadeDelete: true)
                .ForeignKey("dbo.UserDatas", t => t.UserData_UserDataId)
                .Index(t => t.StudentId)
                .Index(t => t.HelpingStudentId)
                .Index(t => t.SubChapterId)
                .Index(t => t.UserData_UserDataId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        MessageContent = c.String(),
                        UserId = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ChatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .ForeignKey("dbo.UserDatas", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ChatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "UserData_UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.Chats", "SubChapterId", "dbo.SubChapters");
            DropForeignKey("dbo.Chats", "StudentId", "dbo.UserDatas");
            DropForeignKey("dbo.Chats", "HelpingStudentId", "dbo.UserDatas");
            DropForeignKey("dbo.Messages", "UserId", "dbo.UserDatas");
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Chats", new[] { "UserData_UserDataId" });
            DropIndex("dbo.Chats", new[] { "SubChapterId" });
            DropIndex("dbo.Chats", new[] { "HelpingStudentId" });
            DropIndex("dbo.Chats", new[] { "StudentId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
        }
    }
}
