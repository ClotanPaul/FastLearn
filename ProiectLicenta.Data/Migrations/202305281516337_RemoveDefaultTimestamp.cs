namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDefaultTimestamp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
        }
    }
}
