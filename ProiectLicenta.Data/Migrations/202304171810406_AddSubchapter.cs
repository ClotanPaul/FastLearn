namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubchapter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubChapters",
                c => new
                    {
                        SubchapterId = c.Int(nullable: false, identity: true),
                        SubchapterTitle = c.String(nullable: false),
                        SubchapterDescription = c.String(nullable: false),
                        ChapterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubchapterId)
                .ForeignKey("dbo.Chapters", t => t.ChapterId, cascadeDelete: true)
                .Index(t => t.ChapterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubChapters", "ChapterId", "dbo.Chapters");
            DropIndex("dbo.SubChapters", new[] { "ChapterId" });
            DropTable("dbo.SubChapters");
        }
    }
}
