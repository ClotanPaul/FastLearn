namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompletedCourseEnrollment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EnrolledStudentInCourses", "CompletedCourse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EnrolledStudentInCourses", "CompletedCourse");
        }
    }
}
