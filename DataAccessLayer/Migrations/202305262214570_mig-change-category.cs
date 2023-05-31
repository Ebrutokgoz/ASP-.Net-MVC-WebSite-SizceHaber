namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migchangecategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String(maxLength: 200));
        }
    }
}
