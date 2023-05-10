using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiLabb3.Migrations
{
    /// <inheritdoc />
    public partial class editLinkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Interests_FK_InterestId",
                table: "Links");

            migrationBuilder.AlterColumn<int>(
                name: "FK_InterestId",
                table: "Links",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Interests_FK_InterestId",
                table: "Links",
                column: "FK_InterestId",
                principalTable: "Interests",
                principalColumn: "InterestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Interests_FK_InterestId",
                table: "Links");

            migrationBuilder.AlterColumn<int>(
                name: "FK_InterestId",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Interests_FK_InterestId",
                table: "Links",
                column: "FK_InterestId",
                principalTable: "Interests",
                principalColumn: "InterestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
