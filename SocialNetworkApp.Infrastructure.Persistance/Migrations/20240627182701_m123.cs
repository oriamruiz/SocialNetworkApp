using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class m123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypePost",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypePost",
                table: "Posts");
        }
    }
}
