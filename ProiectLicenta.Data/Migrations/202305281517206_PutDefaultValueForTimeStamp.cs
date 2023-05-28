namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PutDefaultValueForTimeStamp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
        }
    }
}
