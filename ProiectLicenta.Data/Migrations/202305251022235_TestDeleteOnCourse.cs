namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestDeleteOnCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tests", "TestId", "dbo.SubChapters");
            AddForeignKey("dbo.Tests", "TestId", "dbo.SubChapters", "SubchapterId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tests", "TestId", "dbo.SubChapters");
            AddForeignKey("dbo.Tests", "TestId", "dbo.SubChapters", "SubchapterId");
        }
    }
}
