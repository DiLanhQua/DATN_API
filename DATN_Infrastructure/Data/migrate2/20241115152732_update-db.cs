using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Infrastructure.Data.migrate2
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusDelivery",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "DetailProducts");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "DetailProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailProducts_ColorId",
                table: "DetailProducts",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailProducts_Color_ColorId",
                table: "DetailProducts",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailProducts_Color_ColorId",
                table: "DetailProducts");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropIndex(
                name: "IX_DetailProducts_ColorId",
                table: "DetailProducts");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "DetailProducts");

            migrationBuilder.AddColumn<byte>(
                name: "StatusDelivery",
                table: "Orders",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "DetailProducts",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
