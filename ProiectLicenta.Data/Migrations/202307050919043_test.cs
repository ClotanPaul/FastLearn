namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "QuestionString", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "QuestionString", c => c.String());
        }
    }
}
