namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubChapterfilesCorrection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CourseFiles", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseFiles", new[] { "CourseId" });
            CreateTable(
                "dbo.SubChapterFiles",
                c => new
                    {
                        SubChapterFilesId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        FilePath = c.String(),
                        UploadedDate = c.DateTime(nullable: false),
                        SubChapterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubChapterFilesId)
                .ForeignKey("dbo.SubChapters", t => t.SubChapterId, cascadeDelete: true)
                .Index(t => t.SubChapterId);
            
            DropTable("dbo.CourseFiles");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.CourseFilesId);
            
            DropForeignKey("dbo.SubChapterFiles", "SubChapterId", "dbo.SubChapters");
            DropIndex("dbo.SubChapterFiles", new[] { "SubChapterId" });
            DropTable("dbo.SubChapterFiles");
            CreateIndex("dbo.CourseFiles", "CourseId");
            AddForeignKey("dbo.CourseFiles", "CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
