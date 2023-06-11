namespace ProiectLicenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WarningReasonLength2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Warnings", "WarningReason", c => c.String(maxLength: 45));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Warnings", "WarningReason", c => c.String(maxLength: 35));
        }
    }
}
