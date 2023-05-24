namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseDeactivationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "DeactivationReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "DeactivationReason");
        }
    }
}
