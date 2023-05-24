namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseFiles",
                c => new
                    {
                        CourseFilesId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        FilePath = c.String(),
                        UploadedDate = c.DateTime(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseFilesId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseFiles", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseFiles", new[] { "CourseId" });
            DropTable("dbo.CourseFiles");
        }
    }
}
