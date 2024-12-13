using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addHistoryProduct3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "HistoryByProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "HistoryByProducts");
        }
    }
}
