namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseReviewSuspension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseReviews", "DeactivationReason", c => c.String());
            AddColumn("dbo.CourseReviews", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CourseReviews", "IsActive");
            DropColumn("dbo.CourseReviews", "DeactivationReason");
        }
    }
}
