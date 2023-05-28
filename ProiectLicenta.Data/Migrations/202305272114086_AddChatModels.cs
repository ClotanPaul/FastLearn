namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChatModels : DbMigration
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
                    })
                .PrimaryKey(t => t.ChatId)
                .ForeignKey("dbo.SubChapters", t => t.SubChapterId, cascadeDelete: true)
                .Index(t => t.SubChapterId);
            
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
                .Index(t => t.ChatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "SubChapterId", "dbo.SubChapters");
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropIndex("dbo.Chats", new[] { "SubChapterId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
        }
    }
}
