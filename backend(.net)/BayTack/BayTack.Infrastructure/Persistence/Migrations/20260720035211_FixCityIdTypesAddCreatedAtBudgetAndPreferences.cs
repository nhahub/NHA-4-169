using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BayTack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCityIdTypesAddCreatedAtBudgetAndPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkshopCityId",
                table: "ProviderProfiles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WorkshopAreaId",
                table: "ProviderProfiles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocationCityId",
                table: "CustomerJobs",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "LocationAreaId",
                table: "CustomerJobs",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "CustomerJobs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "CustomerJobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressCityId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressAreaId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShowActiveJobWidget = table.Column<bool>(type: "bit", nullable: false),
                    NotifyNewOffers = table.Column<bool>(type: "bit", nullable: false),
                    NotifyOrderUpdates = table.Column<bool>(type: "bit", nullable: false),
                    NotifyPromotions = table.Column<bool>(type: "bit", nullable: false),
                    NotifySms = table.Column<bool>(type: "bit", nullable: false),
                    ShowProfileToMessagedProviders = table.Column<bool>(type: "bit", nullable: false),
                    ShareLocationWithActiveOrders = table.Column<bool>(type: "bit", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "CustomerJobs");

            migrationBuilder.AlterColumn<int>(
                name: "WorkshopCityId",
                table: "ProviderProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkshopAreaId",
                table: "ProviderProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationCityId",
                table: "CustomerJobs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<int>(
                name: "LocationAreaId",
                table: "CustomerJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressCityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressAreaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
