using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addHistoryProduct4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetailProductId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DetailProductId",
                table: "Comments",
                column: "DetailProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_DetailProducts_DetailProductId",
                table: "Comments",
                column: "DetailProductId",
                principalTable: "DetailProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_DetailProducts_DetailProductId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DetailProductId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DetailProductId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");
        }
    }
}
