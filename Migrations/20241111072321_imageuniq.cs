using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class imageuniq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Image",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Image",
                table: "Products",
                column: "Image",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Image",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Image",
                table: "Products",
                column: "Image");
        }
    }
}
