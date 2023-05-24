namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "OwnerId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "OwnerId", c => c.Int(nullable: false));
        }
    }
}
