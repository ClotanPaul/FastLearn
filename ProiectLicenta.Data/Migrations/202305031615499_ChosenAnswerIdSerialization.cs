namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChosenAnswerIdSerialization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAnswers", "ChosenAnswersIdListSerialized", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAnswers", "ChosenAnswersIdListSerialized");
        }
    }
}
