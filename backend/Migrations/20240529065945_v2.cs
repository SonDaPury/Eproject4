using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SubTopics_sub_topic_id",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Users_user_id",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "static_folder",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "static_folder",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "static_folder",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "static_folder",
                table: "Lessons");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Sources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "sub_topic_id",
                table: "Sources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_SubTopics_sub_topic_id",
                table: "Sources",
                column: "sub_topic_id",
                principalTable: "SubTopics",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Users_user_id",
                table: "Sources",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_SubTopics_sub_topic_id",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Users_user_id",
                table: "Sources");

            migrationBuilder.AddColumn<string>(
                name: "static_folder",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Sources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sub_topic_id",
                table: "Sources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "static_folder",
                table: "Sources",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "static_folder",
                table: "Questions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "static_folder",
                table: "Lessons",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_SubTopics_sub_topic_id",
                table: "Sources",
                column: "sub_topic_id",
                principalTable: "SubTopics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Users_user_id",
                table: "Sources",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
