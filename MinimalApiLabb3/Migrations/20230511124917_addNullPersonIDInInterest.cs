using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiLabb3.Migrations
{
    /// <inheritdoc />
    public partial class addNullPersonIDInInterest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests");

            migrationBuilder.AlterColumn<int>(
                name: "FK_PersonId",
                table: "Interests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests",
                column: "FK_PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests");

            migrationBuilder.AlterColumn<int>(
                name: "FK_PersonId",
                table: "Interests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests",
                column: "FK_PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
