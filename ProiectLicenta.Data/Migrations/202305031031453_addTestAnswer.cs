namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTestAnswer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAnswers",
                c => new
                    {
                        UserAnswerId = c.Int(nullable: false, identity: true),
                        Passed = c.Boolean(nullable: false),
                        Score = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                        TestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserAnswerId)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAnswers", "TestId", "dbo.Tests");
            DropIndex("dbo.UserAnswers", new[] { "TestId" });
            DropTable("dbo.UserAnswers");
        }
    }
}
