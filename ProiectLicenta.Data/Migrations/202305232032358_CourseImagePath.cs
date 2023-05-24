namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseImagePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "ImagePath");
        }
    }
}
