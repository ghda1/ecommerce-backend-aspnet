using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class SizeColorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_CardNumber",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressName",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Products");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:material", "coton,crepe,silk")
                .Annotation("Npgsql:Enum:payment_method", "credit_card,apple_pay,cash")
                .Annotation("Npgsql:Enum:status", "delivered,canceled,shipped,delayed")
                .OldAnnotation("Npgsql:Enum:color", "red,blue,black,white,green")
                .OldAnnotation("Npgsql:Enum:material", "coton,crepe,silk")
                .OldAnnotation("Npgsql:Enum:payment_method", "credit_card,apple_pay,cash")
                .OldAnnotation("Npgsql:Enum:size", "small,large,medium")
                .OldAnnotation("Npgsql:Enum:status", "delivered,canceled,shipped,delayed");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                table: "OrderDetailses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "OrderDetailses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorId);
                    table.ForeignKey(
                        name: "FK_Colors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    SizeId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.SizeId);
                    table.ForeignKey(
                        name: "FK_Sizes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailses_ColorId",
                table: "OrderDetailses",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailses_SizeId",
                table: "OrderDetailses",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ProductId",
                table: "Colors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ProductId",
                table: "Sizes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailses_Colors_ColorId",
                table: "OrderDetailses",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailses_Sizes_SizeId",
                table: "OrderDetailses",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "SizeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailses_Colors_ColorId",
                table: "OrderDetailses");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailses_Sizes_SizeId",
                table: "OrderDetailses");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetailses_ColorId",
                table: "OrderDetailses");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetailses_SizeId",
                table: "OrderDetailses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "OrderDetailses");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "OrderDetailses");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:color", "red,blue,black,white,green")
                .Annotation("Npgsql:Enum:material", "coton,crepe,silk")
                .Annotation("Npgsql:Enum:payment_method", "credit_card,apple_pay,cash")
                .Annotation("Npgsql:Enum:size", "small,large,medium")
                .Annotation("Npgsql:Enum:status", "delivered,canceled,shipped,delayed")
                .OldAnnotation("Npgsql:Enum:material", "coton,crepe,silk")
                .OldAnnotation("Npgsql:Enum:payment_method", "credit_card,apple_pay,cash")
                .OldAnnotation("Npgsql:Enum:status", "delivered,canceled,shipped,delayed");

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardNumber",
                table: "Payments",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressName",
                table: "Addresses",
                column: "AddressName",
                unique: true);
        }
    }
}
