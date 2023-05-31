namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpingStudentApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HelpingStudentApplications",
                c => new
                    {
                        HelpingStudentApplicationId = c.Int(nullable: false, identity: true),
                        FinishedCoursesIds = c.String(),
                        StudentId = c.Int(nullable: false),
                        ProfessorId = c.Int(),
                        IsApproved = c.Boolean(nullable: false),
                        UserDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HelpingStudentApplicationId)
                .ForeignKey("dbo.UserDatas", t => t.UserDataId)
                .Index(t => t.UserDataId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HelpingStudentApplications", "UserDataId", "dbo.UserDatas");
            DropIndex("dbo.HelpingStudentApplications", new[] { "UserDataId" });
            DropTable("dbo.HelpingStudentApplications");
        }
    }
}
