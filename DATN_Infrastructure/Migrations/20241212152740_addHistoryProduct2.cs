using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addHistoryProduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetailProductId",
                table: "HistoryByProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryByProducts_DetailProductId",
                table: "HistoryByProducts",
                column: "DetailProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryByProducts_DetailProducts_DetailProductId",
                table: "HistoryByProducts",
                column: "DetailProductId",
                principalTable: "DetailProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryByProducts_DetailProducts_DetailProductId",
                table: "HistoryByProducts");

            migrationBuilder.DropIndex(
                name: "IX_HistoryByProducts_DetailProductId",
                table: "HistoryByProducts");

            migrationBuilder.DropColumn(
                name: "DetailProductId",
                table: "HistoryByProducts");
        }
    }
}
