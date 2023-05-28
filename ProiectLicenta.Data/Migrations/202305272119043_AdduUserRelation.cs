namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdduUserRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "HelpingStudent_UserDataId", c => c.Int());
            AddColumn("dbo.Chats", "Student_UserDataId", c => c.Int());
            AddColumn("dbo.Chats", "UserData_UserDataId", c => c.Int());
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Chats", "HelpingStudent_UserDataId");
            CreateIndex("dbo.Chats", "Student_UserDataId");
            CreateIndex("dbo.Chats", "UserData_UserDataId");
            AddForeignKey("dbo.Chats", "HelpingStudent_UserDataId", "dbo.UserDatas", "UserDataId");
            AddForeignKey("dbo.Chats", "Student_UserDataId", "dbo.UserDatas", "UserDataId");
            AddForeignKey("dbo.Chats", "UserData_UserDataId", "dbo.UserDatas", "UserDataId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "UserData_UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.Chats", "Student_UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.Chats", "HelpingStudent_UserDataId", "dbo.UserDatas");
            DropIndex("dbo.Chats", new[] { "UserData_UserDataId" });
            DropIndex("dbo.Chats", new[] { "Student_UserDataId" });
            DropIndex("dbo.Chats", new[] { "HelpingStudent_UserDataId" });
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Chats", "UserData_UserDataId");
            DropColumn("dbo.Chats", "Student_UserDataId");
            DropColumn("dbo.Chats", "HelpingStudent_UserDataId");
        }
    }
}
