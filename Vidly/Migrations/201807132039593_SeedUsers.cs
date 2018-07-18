namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'8e154fc0-e488-4911-b20c-596b22e9dd1b', N'guest@vidly.com', 0, N'AM2fFuwUsAWTe+bunc4IxSDTL45Xd/JeraJpBm5X5mdNzfr1skKTZOt6BFaxhbQZJw==', N'2d3dca20-d2f1-4583-a0ef-d9a919428126', NULL, 0, 0, NULL, 1, 0, N'guest@vidly.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'aa292db5-798d-45a6-a0c1-427ea65fbe31', N'admin@vidly.com', 0, N'AMHe5CWCoKYBax/n7FxPd2kNbeMg5njkyPdjsKZy4DNiA0/sy9sYTl4zJstqJLl9Qw==', N'a797c541-d064-43f6-afa7-892f96ada60a', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'251ebe5a-5b9d-41f1-9e80-3a7909f95ef2', N'CanManageMovies')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'aa292db5-798d-45a6-a0c1-427ea65fbe31', N'251ebe5a-5b9d-41f1-9e80-3a7909f95ef2')
");
        }
        
        public override void Down()
        {
        }
    }
}
