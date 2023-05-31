namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixHelpingStudentApplicationRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HelpingStudentApplications", "UserDataId", "dbo.UserDatas");
            DropIndex("dbo.HelpingStudentApplications", new[] { "UserDataId" });
            AddColumn("dbo.UserDatas", "HelpingStudentApplicationId", c => c.Int());
            DropColumn("dbo.HelpingStudentApplications", "UserDataId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HelpingStudentApplications", "UserDataId", c => c.Int(nullable: false));
            DropColumn("dbo.UserDatas", "HelpingStudentApplicationId");
            CreateIndex("dbo.HelpingStudentApplications", "UserDataId");
            AddForeignKey("dbo.HelpingStudentApplications", "UserDataId", "dbo.UserDatas", "UserDataId");
        }
    }
}
