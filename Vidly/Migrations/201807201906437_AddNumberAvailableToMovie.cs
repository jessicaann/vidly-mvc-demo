namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberAvailableToMovie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "NumberAvailable", c => c.Int(nullable: false));
            AlterColumn("dbo.Rentals", "DateReturned", c => c.DateTime());
            Sql("UPDATE Movies SET NumberAvailable = NumberInStock");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rentals", "DateReturned", c => c.DateTime(nullable: false));
            DropColumn("dbo.Movies", "NumberAvailable");
        }
    }
}
