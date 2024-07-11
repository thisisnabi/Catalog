using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCatalogItemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogItems",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.DropIndex(
                name: "IX_CatalogItems_Slug",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogItems",
                schema: "catalog",
                table: "CatalogItems",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogItems",
                schema: "catalog",
                table: "CatalogItems");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "catalog",
                table: "CatalogItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogItems",
                schema: "catalog",
                table: "CatalogItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_Slug",
                schema: "catalog",
                table: "CatalogItems",
                column: "Slug");
        }
    }
}
