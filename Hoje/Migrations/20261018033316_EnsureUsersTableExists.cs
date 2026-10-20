using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoje.Migrations
{
    /// <inheritdoc />
    public partial class EnsureUsersTableExists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NULL
                                 BEGIN
                                     CREATE TABLE [dbo].[Users]
                                     (
                                         [Id] uniqueidentifier NOT NULL,
                                         [Username] nvarchar(max) NOT NULL,
                                         [Password] nvarchar(max) NOT NULL,
                                         [FirstName] nvarchar(max) NOT NULL,
                                         [LastName] nvarchar(max) NOT NULL,
                                         [Fullname] nvarchar(max) NOT NULL,
                                         CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
                                     );
                                 END;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
