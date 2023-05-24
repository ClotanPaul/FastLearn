namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDataAndWarningUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Warnings",
                c => new
                    {
                        WarningId = c.Int(nullable: false, identity: true),
                        WarningReason = c.String(),
                        UserId = c.Int(nullable: false),
                        User_UserDataId = c.Int(),
                    })
                .PrimaryKey(t => t.WarningId)
                .ForeignKey("dbo.UserDatas", t => t.User_UserDataId)
                .Index(t => t.User_UserDataId);
            
            AddColumn("dbo.UserDatas", "SuspendedUntil", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserDatas", "IsSuspended", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserDatas", "WarningId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDatas", "WarningId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Warnings", "User_UserDataId", "dbo.UserDatas");
            DropIndex("dbo.Warnings", new[] { "User_UserDataId" });
            DropColumn("dbo.UserDatas", "IsSuspended");
            DropColumn("dbo.UserDatas", "SuspendedUntil");
            DropTable("dbo.Warnings");
        }
    }
}
