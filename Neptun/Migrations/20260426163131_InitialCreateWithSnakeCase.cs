using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neptun.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subject_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    credit_value = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    course_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    max_students = table.Column<int>(type: "int", nullable: false),
                    course_type = table.Column<int>(type: "int", nullable: false),
                    study_form = table.Column<int>(type: "int", nullable: false),
                    hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.course_id);
                    table.ForeignKey(
                        name: "FK_courses_subjects_subject_guid",
                        column: x => x.subject_guid,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_type = table.Column<int>(type: "int", nullable: false),
                    study_mode = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    CourseModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseModelId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_courses_CourseModelId",
                        column: x => x.CourseModelId,
                        principalTable: "courses",
                        principalColumn: "course_id");
                    table.ForeignKey(
                        name: "FK_users_courses_CourseModelId1",
                        column: x => x.CourseModelId1,
                        principalTable: "courses",
                        principalColumn: "course_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_subject_guid",
                table: "courses",
                column: "subject_guid");

            migrationBuilder.CreateIndex(
                name: "IX_users_CourseModelId",
                table: "users",
                column: "CourseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_users_CourseModelId1",
                table: "users",
                column: "CourseModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "subjects");
        }
    }
}
