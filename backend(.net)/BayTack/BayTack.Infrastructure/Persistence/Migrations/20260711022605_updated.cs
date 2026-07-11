using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BayTack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "ServiceCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ServiceCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryCategoryId",
                table: "ProviderProfiles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuspendReason",
                table: "ProviderProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "PrimaryCategoryId",
                table: "ProviderProfiles");

            migrationBuilder.DropColumn(
                name: "SuspendReason",
                table: "ProviderProfiles");
        }
    }
}
