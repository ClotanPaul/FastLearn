namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentEnrolledInCourseTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EnrolledStudentInCourses",
                c => new
                    {
                        EnrolledStudentInCourseId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        LastCompletedChapterId = c.Int(nullable: false),
                        LastCompletedSubChapterId = c.Int(nullable: false),
                        UserDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EnrolledStudentInCourseId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.UserDatas", t => t.UserDataId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.UserDataId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnrolledStudentInCourses", "UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.EnrolledStudentInCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.EnrolledStudentInCourses", new[] { "UserDataId" });
            DropIndex("dbo.EnrolledStudentInCourses", new[] { "CourseId" });
            DropTable("dbo.EnrolledStudentInCourses");
        }
    }
}
