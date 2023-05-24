namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubchANDChnumbering : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubChapters", "SubchapterNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Chapters", "ChapterNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapters", "ChapterNumber");
            DropColumn("dbo.SubChapters", "SubchapterNumber");
        }
    }
}
