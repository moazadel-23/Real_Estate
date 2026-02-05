using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estate.Migrations
{
    /// <inheritdoc />
    public partial class updateInImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "PropertyImages");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "PropertyImages",
                newName: "PropertyImgs");

            migrationBuilder.AddColumn<string>(
                name: "MainImg",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImg",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "PropertyImgs",
                table: "PropertyImages",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "PropertyImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
