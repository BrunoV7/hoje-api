using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoje.Migrations
{
    /// <inheritdoc />
    public partial class CreatedUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
            
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
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
