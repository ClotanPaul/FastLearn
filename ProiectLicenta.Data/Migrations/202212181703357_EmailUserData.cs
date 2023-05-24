namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailUserData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDatas", "Email");
        }
    }
}
