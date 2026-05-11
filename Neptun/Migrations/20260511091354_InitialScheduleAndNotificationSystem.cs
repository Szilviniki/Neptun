using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neptun.Migrations
{
    /// <inheritdoc />
    public partial class InitialScheduleAndNotificationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notification_log",
                columns: table => new
                {
                    notification_log_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    generated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_log", x => x.notification_log_id);
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                columns: table => new
                {
                    schedule_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    room = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_schedule_courses_CourseModelId",
                        column: x => x.CourseModelId,
                        principalTable: "courses",
                        principalColumn: "course_id");
                    table.ForeignKey(
                        name: "FK_schedule_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedule_course_id",
                table: "schedule",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_CourseModelId",
                table: "schedule",
                column: "CourseModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_log");

            migrationBuilder.DropTable(
                name: "schedule");
        }
    }
}
