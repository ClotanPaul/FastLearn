namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseReviews",
                c => new
                    {
                        CourseReviewId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ReviewText = c.String(),
                        Stars = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseReviewId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseReviews", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseReviews", new[] { "CourseId" });
            DropTable("dbo.CourseReviews");
        }
    }
}
