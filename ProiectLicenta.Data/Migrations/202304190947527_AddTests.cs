namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestId = c.Int(nullable: false),
                        MinimumScore = c.Int(nullable: false),
                        TestDescription = c.String(),
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.SubChapters", t => t.TestId)
                .Index(t => t.TestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tests", "TestId", "dbo.SubChapters");
            DropIndex("dbo.Tests", new[] { "TestId" });
            DropTable("dbo.Tests");
        }
    }
}
