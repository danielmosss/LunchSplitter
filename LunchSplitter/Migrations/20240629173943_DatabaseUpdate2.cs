using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchSplitter.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "email", "name", "password" },
                values: new object[] { 1, "admin@example.com", "SystemAdmin", "L9QreV+LSPGVDQxZReP8se1YcmwgZ86TbeS8q3RsfAo=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
