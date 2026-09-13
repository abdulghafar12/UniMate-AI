using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniMateAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AcademicRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicSemesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Session = table.Column<string>(type: "TEXT", nullable: false),
                    SemesterName = table.Column<string>(type: "TEXT", nullable: false),
                    SemesterNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    GPA = table.Column<double>(type: "REAL", nullable: true),
                    CGPA = table.Column<double>(type: "REAL", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    SemesterFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    RebateOrConcession = table.Column<decimal>(type: "TEXT", nullable: false),
                    FeeReceived = table.Column<decimal>(type: "TEXT", nullable: false),
                    FeeRemaining = table.Column<decimal>(type: "TEXT", nullable: false),
                    PreviousOutstanding = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalOutstanding = table.Column<decimal>(type: "TEXT", nullable: false),
                    AdditionalFeeComments = table.Column<string>(type: "TEXT", nullable: true),
                    RebateComments = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentComments = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSemesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicSemesters_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SemesterNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<string>(type: "TEXT", nullable: false),
                    CourseName = table.Column<string>(type: "TEXT", nullable: false),
                    CreditHours = table.Column<string>(type: "TEXT", nullable: false),
                    IsLab = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcademicSemesterId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<string>(type: "TEXT", nullable: false),
                    CourseName = table.Column<string>(type: "TEXT", nullable: false),
                    CreditHours = table.Column<string>(type: "TEXT", nullable: false),
                    IsLab = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRetake = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreviousGrade = table.Column<string>(type: "TEXT", nullable: true),
                    Grade = table.Column<string>(type: "TEXT", nullable: false),
                    GradePoint = table.Column<double>(type: "REAL", nullable: true),
                    ResultStatus = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicCourses_AcademicSemesters_AcademicSemesterId",
                        column: x => x.AcademicSemesterId,
                        principalTable: "AcademicSemesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcademicCourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssessmentType = table.Column<string>(type: "TEXT", nullable: false),
                    MaxMarks = table.Column<double>(type: "REAL", nullable: false),
                    ObtainedMarks = table.Column<double>(type: "REAL", nullable: false),
                    Weightage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAssessments_AcademicCourses_AcademicCourseId",
                        column: x => x.AcademicCourseId,
                        principalTable: "AcademicCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseAttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AcademicCourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAttendanceRecords_AcademicCourses_AcademicCourseId",
                        column: x => x.AcademicCourseId,
                        principalTable: "AcademicCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCourses_AcademicSemesterId",
                table: "AcademicCourses",
                column: "AcademicSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSemesters_StudentId_Session",
                table: "AcademicSemesters",
                columns: new[] { "StudentId", "Session" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssessments_AcademicCourseId",
                table: "CourseAssessments",
                column: "AcademicCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAttendanceRecords_AcademicCourseId",
                table: "CourseAttendanceRecords",
                column: "AcademicCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumCourses_SemesterNumber_CourseId",
                table: "CurriculumCourses",
                columns: new[] { "SemesterNumber", "CourseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAssessments");

            migrationBuilder.DropTable(
                name: "CourseAttendanceRecords");

            migrationBuilder.DropTable(
                name: "CurriculumCourses");

            migrationBuilder.DropTable(
                name: "AcademicCourses");

            migrationBuilder.DropTable(
                name: "AcademicSemesters");
        }
    }
}
