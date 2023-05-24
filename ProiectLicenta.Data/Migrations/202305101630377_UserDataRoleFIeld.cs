namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDataRoleFIeld : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "UserRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDatas", "UserRole");
        }
    }
}
