using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_Estate.Migrations
{
    /// <inheritdoc />
    public partial class updateAgainImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImages_Properties_PropertyId",
                table: "PropertyImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImages",
                table: "PropertyImages");

            migrationBuilder.RenameTable(
                name: "PropertyImages",
                newName: "PropertySubImage");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertySubImage",
                newName: "IX_PropertySubImage_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertySubImage",
                table: "PropertySubImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertySubImage_Properties_PropertyId",
                table: "PropertySubImage",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertySubImage_Properties_PropertyId",
                table: "PropertySubImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertySubImage",
                table: "PropertySubImage");

            migrationBuilder.RenameTable(
                name: "PropertySubImage",
                newName: "PropertyImages");

            migrationBuilder.RenameIndex(
                name: "IX_PropertySubImage_PropertyId",
                table: "PropertyImages",
                newName: "IX_PropertyImages_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImages",
                table: "PropertyImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImages_Properties_PropertyId",
                table: "PropertyImages",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
