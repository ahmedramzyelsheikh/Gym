using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "HealthRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_GymUser_MemberId",
                table: "HealthRecords",
                column: "MemberId",
                principalTable: "GymUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_GymUser_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_MemberId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "HealthRecords");
        }
    }
}
