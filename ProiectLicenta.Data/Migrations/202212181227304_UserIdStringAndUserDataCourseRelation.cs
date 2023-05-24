namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdStringAndUserDataCourseRelation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserDatas", "UserId", c => c.String(nullable: false));
            DropColumn("dbo.Courses", "CommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CommentId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserDatas", "UserId", c => c.Int(nullable: false));
        }
    }
}
