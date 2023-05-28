namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageUserRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "User_UserDataId", "dbo.UserDatas");
            DropIndex("dbo.Messages", new[] { "User_UserDataId" });
            DropColumn("dbo.Messages", "UserId");
            RenameColumn(table: "dbo.Messages", name: "User_UserDataId", newName: "UserId");
            AlterColumn("dbo.Messages", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "UserId");
            AddForeignKey("dbo.Messages", "UserId", "dbo.UserDatas", "UserDataId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "UserId", "dbo.UserDatas");
            DropIndex("dbo.Messages", new[] { "UserId" });
            AlterColumn("dbo.Messages", "UserId", c => c.Int());
            RenameColumn(table: "dbo.Messages", name: "UserId", newName: "User_UserDataId");
            AddColumn("dbo.Messages", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "User_UserDataId");
            AddForeignKey("dbo.Messages", "User_UserDataId", "dbo.UserDatas", "UserDataId");
        }
    }
}
