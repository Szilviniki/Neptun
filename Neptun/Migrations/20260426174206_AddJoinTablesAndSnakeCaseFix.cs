using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neptun.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinTablesAndSnakeCaseFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_courses_CourseModelId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_courses_CourseModelId1",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_CourseModelId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_CourseModelId1",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CourseModelId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CourseModelId1",
                table: "users");

            migrationBuilder.CreateTable(
                name: "course_students",
                columns: table => new
                {
                    CourseModel1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_students", x => new { x.CourseModel1Id, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_course_students_courses_CourseModel1Id",
                        column: x => x.CourseModel1Id,
                        principalTable: "courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_students_users_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_teachers",
                columns: table => new
                {
                    CourseModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeachersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_teachers", x => new { x.CourseModelId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_course_teachers_courses_CourseModelId",
                        column: x => x.CourseModelId,
                        principalTable: "courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_teachers_users_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_students_StudentsId",
                table: "course_students",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_course_teachers_TeachersId",
                table: "course_teachers",
                column: "TeachersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_students");

            migrationBuilder.DropTable(
                name: "course_teachers");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseModelId",
                table: "users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseModelId1",
                table: "users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_CourseModelId",
                table: "users",
                column: "CourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_users_CourseModelId1",
                table: "users",
                column: "CourseModelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_users_courses_CourseModelId",
                table: "users",
                column: "CourseModelId",
                principalTable: "courses",
                principalColumn: "course_id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_courses_CourseModelId1",
                table: "users",
                column: "CourseModelId1",
                principalTable: "courses",
                principalColumn: "course_id");
        }
    }
}
