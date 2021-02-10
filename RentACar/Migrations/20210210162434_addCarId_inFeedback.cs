using Microsoft.EntityFrameworkCore.Migrations;

namespace RentACar.Migrations
{
    public partial class addCarId_inFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_Cars_CarID",
                table: "FeedBacks");

            migrationBuilder.AlterColumn<int>(
                name: "CarID",
                table: "FeedBacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_Cars_CarID",
                table: "FeedBacks",
                column: "CarID",
                principalTable: "Cars",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_Cars_CarID",
                table: "FeedBacks");

            migrationBuilder.AlterColumn<int>(
                name: "CarID",
                table: "FeedBacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_Cars_CarID",
                table: "FeedBacks",
                column: "CarID",
                principalTable: "Cars",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
