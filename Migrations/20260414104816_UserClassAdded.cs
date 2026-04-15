using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_App.Migrations
{
    /// <inheritdoc />
    public partial class UserClassAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ToDoTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoTasks_UserId",
                table: "ToDoTasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoTasks_Users_UserId",
                table: "ToDoTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoTasks_Users_UserId",
                table: "ToDoTasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ToDoTasks_UserId",
                table: "ToDoTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDoTasks");
        }
    }
}
