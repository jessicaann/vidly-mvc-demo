namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGenres : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT Genres ON");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (1, 'Action')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (2, 'Animated')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (3, 'Biographical')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (4, 'Comedy')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (5, 'Drama')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (6, 'Horror')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (7, 'Indie')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (8, 'Musical')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (9, 'Science Fiction')");
            Sql("INSERT INTO Genres (Id, GenreName) VALUES (10, 'Suspense')");
        }
        
        public override void Down()
        {
        }
    }
}
