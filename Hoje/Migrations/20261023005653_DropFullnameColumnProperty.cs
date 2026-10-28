using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoje.Migrations
{
    /// <inheritdoc />
    public partial class DropFullnameColumnProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[Users]') AND name = N'Fullname')
                BEGIN
                    DECLARE @var nvarchar(max);
                    SELECT @var = QUOTENAME([d].[name])
                    FROM [sys].[default_constraints] [d]
                    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
                    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Fullname');
                    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT ' + @var + ';');
                    ALTER TABLE [Users] DROP COLUMN [Fullname];
                END;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );
        }
    }
}
