using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Infrastructure.Data.migrate2
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medium_Images_ImagesId",
                table: "Medium");

            migrationBuilder.DropIndex(
                name: "IX_Medium_ImagesId",
                table: "Medium");

            migrationBuilder.DropColumn(
                name: "ImagesId",
                table: "Medium");

            migrationBuilder.CreateIndex(
                name: "IX_Medium_ImageId",
                table: "Medium",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medium_Images_ImageId",
                table: "Medium",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medium_Images_ImageId",
                table: "Medium");

            migrationBuilder.DropIndex(
                name: "IX_Medium_ImageId",
                table: "Medium");

            migrationBuilder.AddColumn<int>(
                name: "ImagesId",
                table: "Medium",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medium_ImagesId",
                table: "Medium",
                column: "ImagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medium_Images_ImagesId",
                table: "Medium",
                column: "ImagesId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
