namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false),
                        CourseDescription = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                        Owner_UserDataId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.UserDatas", t => t.Owner_UserDataId)
                .Index(t => t.Owner_UserDataId);
            
            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        UserDataId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                        Points = c.Int(nullable: false),
                        EnrolledCourseId = c.Int(nullable: false),
                        WarningId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserDataId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Owner_UserDataId", "dbo.UserDatas");
            DropIndex("dbo.Courses", new[] { "Owner_UserDataId" });
            DropTable("dbo.UserDatas");
            DropTable("dbo.Courses");
        }
    }
}
