using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniMateAI.Api.DTOs;
using UniMateBackend.Data;

namespace UniMateAI.Api.Controllers;

[ApiController]
[Route("api/academic")]
public class AcademicController : ControllerBase
{
    private readonly UniMateDbContext _db;

    public AcademicController(UniMateDbContext db)
    {
        _db = db;
    }

    // =====================================================
    // GET COMPLETE ACADEMIC RECORD
    // =====================================================

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentAcademicRecord(int studentId)
    {
        var student = await _db.Students
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Student not found."
            });
        }

        var semesters = await _db.AcademicSemesters
            .Where(s => s.StudentId == studentId)
            .Include(s => s.Courses)
            .OrderByDescending(s => s.Session == "Su-2026")
            .ThenByDescending(s => s.SemesterNumber)
            .ToListAsync();

        var curriculum = await _db.CurriculumCourses
            .OrderBy(c => c.SemesterNumber)
            .ThenBy(c => c.CourseId)
            .ToListAsync();

        var semesterDtos = semesters.Select(s => new AcademicSemesterDto
        {
            Id = s.Id,
            Session = s.Session,
            SemesterName = s.SemesterName,
            SemesterNumber = s.SemesterNumber,
            GPA = s.GPA,
            CGPA = s.CGPA,
            Status = s.Status,

            SemesterFee = s.SemesterFee,
            RebateOrConcession = s.RebateOrConcession,
            FeeReceived = s.FeeReceived,
            FeeRemaining = s.FeeRemaining,
            PreviousOutstanding = s.PreviousOutstanding,
            TotalOutstanding = s.TotalOutstanding,

            AdditionalFeeComments = s.AdditionalFeeComments,
            RebateComments = s.RebateComments,
            PaymentComments = s.PaymentComments,

            Courses = s.Courses.Select(c => new AcademicCourseDto
            {
                Id = c.Id,
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                CreditHours = c.CreditHours,
                IsLab = c.IsLab,
                IsRetake = c.IsRetake,
                PreviousGrade = c.PreviousGrade,
                Grade = c.Grade,
                ResultStatus = c.ResultStatus
            }).ToList()
        }).ToList();

        var curriculumDtos = curriculum.Select(c => new CurriculumCourseDto
        {
            Id = c.Id,
            SemesterNumber = c.SemesterNumber,
            CourseId = c.CourseId,
            CourseName = c.CourseName,
            CreditHours = c.CreditHours,
            IsLab = c.IsLab,
            IsRequired = c.IsRequired
        }).ToList();

        return Ok(new
        {
            success = true,

            student = new
            {
                student.Id,
                student.FullName,
                student.RegistrationNumber,
                student.UniversityEmail,
                student.Program,
                student.Department,
                student.Semester,
                student.CGPA
            },

            academicSummary = new
            {
                degreeCGPA = 2.66,
                academicStatus = "Enrolled",
                financialStatus = "Good",
                currentHolds = 0,
                currentFines = 0,
                totalOutstanding = 0
            },

            semesters = semesterDtos,

            curriculum = curriculumDtos
        });
    }


    // =====================================================
    // GET COURSE ASSESSMENTS
    // =====================================================

    [HttpGet("course/{courseId}/assessments")]
    public async Task<IActionResult> GetCourseAssessments(int courseId)
    {
        var course = await _db.AcademicCourses
            .Include(c => c.Assessments)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Course not found."
            });
        }

        return Ok(new
        {
            success = true,
            course = new
            {
                course.Id,
                course.CourseId,
                course.CourseName,
                course.Grade
            },
            assessments = course.Assessments
        });
    }


    // =====================================================
    // GET COURSE ATTENDANCE
    // =====================================================

    [HttpGet("course/{courseId}/attendance")]
    public async Task<IActionResult> GetCourseAttendance(int courseId)
    {
        var course = await _db.AcademicCourses
            .Include(c => c.AttendanceRecords)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Course not found."
            });
        }

        var totalClasses = course.AttendanceRecords.Count;

        var present = course.AttendanceRecords
            .Count(a => a.Status == "Present");

        var percentage = totalClasses == 0
            ? 0
            : Math.Round((double)present / totalClasses * 100, 2);

        return Ok(new
        {
            success = true,

            course = new
            {
                course.Id,
                course.CourseId,
                course.CourseName
            },

            attendance = new
            {
                totalClasses,
                present,
                absent = totalClasses - present,
                percentage,

                records = course.AttendanceRecords
                    .OrderBy(a => a.AttendanceDate)
                    .Select(a => new
                    {
                        a.AttendanceDate,
                        a.Status
                    })
            }
        });
    }
}