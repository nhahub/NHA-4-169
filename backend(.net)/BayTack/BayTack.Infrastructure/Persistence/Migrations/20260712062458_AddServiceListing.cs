using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BayTack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceListings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IconName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BasicPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasicPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BasicDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BasicDelivery = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StandardName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StandardPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StandardPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    StandardDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StandardDelivery = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PremiumName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PremiumPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PremiumDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PremiumDelivery = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceListings_AspNetUsers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceListings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListings_ProviderId",
                table: "ServiceListings",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListings_ServiceId_ProviderId",
                table: "ServiceListings",
                columns: new[] { "ServiceId", "ProviderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceListings");
        }
    }
}
