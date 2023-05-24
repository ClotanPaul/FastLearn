namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IssueRevoled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "IssueResolved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "IssueResolved");
        }
    }
}
