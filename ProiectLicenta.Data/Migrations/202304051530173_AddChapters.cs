namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChapters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ChapterId = c.Int(nullable: false, identity: true),
                        ChapterTitle = c.String(nullable: false),
                        ChapterDescription = c.String(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChapterId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chapters", "CourseId", "dbo.Courses");
            DropIndex("dbo.Chapters", new[] { "CourseId" });
            DropTable("dbo.Chapters");
        }
    }
}
