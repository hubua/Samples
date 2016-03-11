namespace AsyncDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Binary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Image");
        }
    }
}
