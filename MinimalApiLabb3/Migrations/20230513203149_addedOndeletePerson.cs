using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiLabb3.Migrations
{
    /// <inheritdoc />
    public partial class addedOndeletePerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests");

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests",
                column: "FK_PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests");

            migrationBuilder.AddForeignKey(
                name: "FK_Interests_Persons_FK_PersonId",
                table: "Interests",
                column: "FK_PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }
    }
}
