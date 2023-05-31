namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migchangecontent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contents", "ContentValue", c => c.String(maxLength: 3000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contents", "ContentValue", c => c.String(maxLength: 1000));
        }
    }
}
