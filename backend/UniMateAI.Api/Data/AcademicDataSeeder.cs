using Microsoft.EntityFrameworkCore;
using UniMateAI.Api.Models;

using UniMateBackend.Data;

public static class AcademicDataSeeder
{
    public static async Task SeedAsync(UniMateDbContext db)
    {
        // =========================================================
        // IMPORTANT
        // This seed is for the real development student only.
        // Student ID 2 is the student whose academic record was provided.
        // =========================================================

        var student = await db.Students
            .SingleOrDefaultAsync(s => s.Id == 2);

        if (student == null)
        {
            Console.WriteLine("Student ID 2 not found. Academic seed skipped.");
            return;
        }

        // Prevent duplicate academic records for this student only.
        if (await db.AcademicSemesters
            .AnyAsync(s => s.StudentId == student.Id))
        {
            Console.WriteLine(
                $"Academic data already exists for {student.FullName}. Seed skipped.");

            return;
        }

        // =========================================================
        // OFFICIAL BSCS CURRICULUM
        // =========================================================

        if (!await db.CurriculumCourses.AnyAsync())
        {
            var curriculum = new List<CurriculumCourse>
            {
                // =====================================================
                // SEMESTER 1
                // =====================================================

                new()
                {
                    SemesterNumber = 1,
                    CourseId = "CS 1102",
                    CourseName = "Programming Fundamentals",
                    CreditHours = "3+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 1,
                    CourseId = "CS 1101",
                    CourseName = "Application of ICT",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 1,
                    CourseId = "MT 1701",
                    CourseName = "Discrete Structures",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 1,
                    CourseId = "MT 1702",
                    CourseName = "Calculus and Analytic Geometry",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 1,
                    CourseId = "EN 1101",
                    CourseName = "Functional English",
                    CreditHours = "3+0",
                    IsLab = false
                },

                // =====================================================
                // SEMESTER 2
                // =====================================================

                new()
                {
                    SemesterNumber = 2,
                    CourseId = "CS 1201",
                    CourseName = "Digital Logic Design",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 2,
                    CourseId = "CS 1202",
                    CourseName = "Object Oriented Programming",
                    CreditHours = "3+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 2,
                    CourseId = "CS 1203",
                    CourseName = "Database Systems",
                    CreditHours = "3+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 2,
                    CourseId = "MT 1201",
                    CourseName = "Multi-variable Calculus",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 2,
                    CourseId = "MT 1202",
                    CourseName = "Linear Algebra",
                    CreditHours = "3+0",
                    IsLab = false
                },

                // =====================================================
                // SEMESTER 3
                // =====================================================

                new()
                {
                    SemesterNumber = 3,
                    CourseId = "AI 2101",
                    CourseName = "Artificial Intelligence",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 3,
                    CourseId = "CS 2102",
                    CourseName = "Computer Networks",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 3,
                    CourseId = "CS 2103",
                    CourseName = "Data Structures and Algorithms",
                    CreditHours = "3+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 3,
                    CourseId = "CY 2101",
                    CourseName = "Information Security",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 3,
                    CourseId = "MT 1203",
                    CourseName = "Probability and Statistics",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 3,
                    CourseId = "SE 2101",
                    CourseName = "Software Engineering",
                    CreditHours = "3+0",
                    IsLab = false
                },

                // =====================================================
                // SEMESTER 4
                // =====================================================

                new()
                {
                    SemesterNumber = 4,
                    CourseId = "CS 2202",
                    CourseName = "Theory of Automata",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 4,
                    CourseId = "CS 2203",
                    CourseName = "Advanced Database Management Systems",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 4,
                    CourseId = "CS 2204",
                    CourseName = "Computer Organization and Assembly Language",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 4,
                    CourseId = "HU 1101",
                    CourseName = "Islamic Studies / Ethics",
                    CreditHours = "2+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 4,
                    CourseId = "EN 1202",
                    CourseName = "Expository Writing",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 4,
                    CourseId = "GE 2201",
                    CourseName = "Applied Physics",
                    CreditHours = "2+1",
                    IsLab = true
                },

                // =====================================================
                // SEMESTER 5
                // =====================================================

                new()
                {
                    SemesterNumber = 5,
                    CourseId = "CE 3101",
                    CourseName = "Computer Architecture",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 5,
                    CourseId = "CS 3101",
                    CourseName = "Operating Systems",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 5,
                    CourseId = "CS 3102",
                    CourseName = "HCI and Computer Graphics",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 5,
                    CourseId = "CS 3104",
                    CourseName = "Mobile App Development",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 5,
                    CourseId = "CS 3105",
                    CourseName = "Web Technologies",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 5,
                    CourseId = "MG 1201",
                    CourseName = "Introduction to Management",
                    CreditHours = "3+0",
                    IsLab = false
                },

                // =====================================================
                // SEMESTER 6
                // =====================================================

                new()
                {
                    SemesterNumber = 6,
                    CourseId = "CS 3202",
                    CourseName = "Parallel and Distributed Computing",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 6,
                    CourseId = "CS 3203",
                    CourseName = "Visual Programming",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 6,
                    CourseId = "CS 3204",
                    CourseName = "Web Engineering",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 6,
                    CourseId = "CS 3205",
                    CourseName = "Compiler Construction",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 6,
                    CourseId = "CY 2201",
                    CourseName = "Cyber Security",
                    CreditHours = "2+1",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 6,
                    CourseId = "MT 2207",
                    CourseName = "Numerical Analysis",
                    CreditHours = "2+1",
                    IsLab = true
                },

                // =====================================================
                // SEMESTER 7
                // =====================================================

                new()
                {
                    SemesterNumber = 7,
                    CourseId = "CS 4101",
                    CourseName = "Analysis of Algorithms",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 7,
                    CourseId = "CS 4191",
                    CourseName = "Final Year Project-1",
                    CreditHours = "0+2",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 7,
                    CourseId = "EN 4101",
                    CourseName = "Technical and Business Writing",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 7,
                    CourseId = "EE 2202",
                    CourseName = "Entrepreneurship",
                    CreditHours = "2+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 7,
                    CourseId = "MG 2102",
                    CourseName = "Introduction to Marketing",
                    CreditHours = "3+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 7,
                    CourseId = "SE 2103",
                    CourseName = "Software Testing and Quality Assurance",
                    CreditHours = "2+1",
                    IsLab = true
                },

                // =====================================================
                // SEMESTER 8
                // =====================================================

                new()
                {
                    SemesterNumber = 8,
                    CourseId = "CS 4292",
                    CourseName = "Final Year Project-2",
                    CreditHours = "0+4",
                    IsLab = true
                },
                new()
                {
                    SemesterNumber = 8,
                    CourseId = "GE 2101",
                    CourseName = "Civics and Community Engagement",
                    CreditHours = "2+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 8,
                    CourseId = "HU 1102",
                    CourseName = "Ideology and Constitution of Pakistan",
                    CreditHours = "2+0",
                    IsLab = false
                },
                new()
                {
                    SemesterNumber = 8,
                    CourseId = "GE 4201",
                    CourseName = "Professional Practices",
                    CreditHours = "2+0",
                    IsLab = false
                }
            };

            await db.CurriculumCourses.AddRangeAsync(curriculum);
            await db.SaveChangesAsync();
        }

        // =========================================================
        // SEMESTER 1 — FALL 2024
        // =========================================================

        var sem1 = new AcademicSemester
        {
            StudentId = student.Id,
            Session = "Fa-2024",
            SemesterName = "Fall 2024",
            SemesterNumber = 1,
            GPA = 2.60,
            CGPA = 2.60,
            Status = "Completed",
            SemesterFee = 137000,
            RebateOrConcession = 54000,
            FeeReceived = 83000,
            FeeRemaining = 0,
            PreviousOutstanding = 0,
            TotalOutstanding = 0,
            AdditionalFeeComments = "Admission and Security fee",
            RebateComments = "Scholarship"
        };

        sem1.Courses.AddRange(new[]
        {
            CreateCourse("CS 1101", "Application of ICT", "2+1", "C-"),
            CreateCourse("CS 1102", "Programming Fundamentals", "3+1", "B-"),
            CreateCourse("EN 1101", "Functional English", "3+0", "B-"),
            CreateCourse("HU 1101", "Islamic Studies / Ethics", "2+0", "B-"),
            CreateCourse("MT 1103", "Discrete Structures", "3+0", "B+")
        });

        // =========================================================
        // SEMESTER 2 — SPRING 2025
        // =========================================================

        var sem2 = new AcademicSemester
        {
            StudentId = student.Id,
            Session = "Sp-2025",
            SemesterName = "Spring 2025",
            SemesterNumber = 2,
            GPA = 2.08,
            CGPA = 2.32,
            Status = "Completed",
            SemesterFee = 112500,
            RebateOrConcession = 48000,
            FeeReceived = 64500,
            FeeRemaining = 0,
            PreviousOutstanding = 0,
            TotalOutstanding = 0,
            AdditionalFeeComments = "Installment Charges / Late Fee Fine [7 Weeks]",
            RebateComments = "Need Based Scholarship"
        };

        sem2.Courses.AddRange(new[]
        {
            CreateCourse("CS 1201", "Digital Logic Design", "2+1", "C"),
            CreateCourse("CS 2004", "Object Oriented Programming", "3+1", "C"),
            CreateCourse("CS 2201", "Database Systems", "3+1", "C+"),
            CreateCourse("MT 1102", "Calculus and Analytic Geometry", "3+0", "B"),
            CreateCourse(
                "SE 2101",
                "Software Engineering",
                "3+0",
                "D",
                isRetake: true)
        });

        // =========================================================
        // SEMESTER 3 — FALL 2025
        // =========================================================

        var sem3 = new AcademicSemester
        {
            StudentId = student.Id,
            Session = "Fa-2025",
            SemesterName = "Fall 2025",
            SemesterNumber = 3,
            GPA = 2.54,
            CGPA = 2.41,
            Status = "Completed",
            SemesterFee = 119600,
            RebateOrConcession = 60000,
            FeeReceived = 59600,
            FeeRemaining = 0,
            PreviousOutstanding = 0,
            TotalOutstanding = 0,
            AdditionalFeeComments = "Installment Charges / Others - Membership",
            RebateComments = "Need Based Scholarship"
        };

        sem3.Courses.AddRange(new[]
        {
            CreateCourse("CS 2102", "Computer Networks", "2+1", "C"),
            CreateCourse(
                "CS 2103",
                "Data Structures and Algorithms",
                "3+1",
                "C+"),
            CreateCourse("CY 2101", "Information Security", "2+1", "C-"),
            CreateCourse("EN 1202", "Expository Writing", "3+0", "B+"),
            CreateCourse("MT 1202", "Linear Algebra", "3+0", "B+"),
            CreateCourse("MT 1203", "Probability and Statistics", "3+0", "B-")
        });

        // =========================================================
        // SEMESTER 4 — SPRING 2026
        // =========================================================

        var sem4 = new AcademicSemester
        {
            StudentId = student.Id,
            Session = "Sp-2026",
            SemesterName = "Spring 2026",
            SemesterNumber = 4,
            GPA = 2.72,
            CGPA = 2.49,
            Status = "Completed",
            SemesterFee = 119500,
            RebateOrConcession = 38000,
            FeeReceived = 81500,
            FeeRemaining = 0,
            PreviousOutstanding = 0,
            TotalOutstanding = 0,
            AdditionalFeeComments = "Installment Charges",
            RebateComments = "Need Based Scholarship"
        };

        sem4.Courses.AddRange(new[]
        {
            CreateCourse(
                "AI 2101",
                "Artificial Intelligence Sec II",
                "2+1",
                "B-"),

            CreateCourse(
                "CS 2202",
                "Theory of Automata Sec II",
                "3+0",
                "B-"),

            CreateCourse(
                "CS 2203",
                "Advanced Database Management Systems",
                "2+1",
                "B"),

            CreateCourse(
                "CS 2204",
                "Computer Organization and Assembly Language",
                "2+1",
                "B-"),

            CreateCourse(
                "GE 2201",
                "Applied Physics",
                "2+1",
                "B"),

            CreateCourse(
                "MT 1201",
                "Multi-variable Calculus",
                "3+0",
                "C+")
        });

        // =========================================================
        // SUMMER 2026
        // =========================================================

        var summer = new AcademicSemester
        {
            StudentId = student.Id,
            Session = "Su-2026",
            SemesterName = "Summer 2026",
            SemesterNumber = 0,
            GPA = 3.34,
            CGPA = 2.66,
            Status = "Completed",
            SemesterFee = 47500,
            RebateOrConcession = 0,
            FeeReceived = 47500,
            FeeRemaining = 0,
            PreviousOutstanding = 0,
            TotalOutstanding = 0,
            AdditionalFeeComments = "Installment Charges",
            PaymentComments =
                "Su26/00046 and Su26/00124 installments paid"
        };

        var informationSecurity = CreateCourse(
            "CY 2101",
            "Information Security",
            "2+1",
            "A-",
            isRetake: true,
            previousGrade: "C-");

        var softwareEngineering = CreateCourse(
            "SE 2101",
            "Software Engineering",
            "3+0",
            "B",
            isRetake: true,
            previousGrade: "D");

        summer.Courses.AddRange(new[]
        {
            informationSecurity,
            softwareEngineering
        });

        // =========================================================
        // SAVE SEMESTERS AND COURSES
        // =========================================================

        await db.AcademicSemesters.AddRangeAsync(
            sem1,
            sem2,
            sem3,
            sem4,
            summer);

        await db.SaveChangesAsync();

        // =========================================================
        // INFORMATION SECURITY ASSESSMENTS
        // =========================================================

        var assessments = new List<CourseAssessment>
        {
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Assignment 1",
                MaxMarks = 10,
                ObtainedMarks = 10,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Assignment 2",
                MaxMarks = 10,
                ObtainedMarks = 5,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Assignment 3",
                MaxMarks = 10,
                ObtainedMarks = 10,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Assignment 4",
                MaxMarks = 10,
                ObtainedMarks = 9,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Quiz 1",
                MaxMarks = 10,
                ObtainedMarks = 9,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Quiz 2",
                MaxMarks = 10,
                ObtainedMarks = 10,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Quiz 3",
                MaxMarks = 10,
                ObtainedMarks = 10,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Quiz 4",
                MaxMarks = 10,
                ObtainedMarks = 10,
                Weightage = 3.333
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Lab Project",
                MaxMarks = 20,
                ObtainedMarks = 15.93,
                Weightage = 20
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Mid Term",
                MaxMarks = 45,
                ObtainedMarks = 36,
                Weightage = 25
            },
            new()
            {
                AcademicCourseId = informationSecurity.Id,
                AssessmentType = "Final",
                MaxMarks = 100,
                ObtainedMarks = 58,
                Weightage = 35
            }
        };

        await db.CourseAssessments.AddRangeAsync(assessments);

        // =========================================================
        // COMPUTER ORGANIZATION ATTENDANCE
        // =========================================================

        var assemblyCourse = sem4.Courses
            .First(c => c.CourseId == "CS 2204");

        var attendanceDates = new[]
        {
            new DateTime(2026, 2, 11),
            new DateTime(2026, 2, 16),
            new DateTime(2026, 2, 17),
            new DateTime(2026, 2, 23),
            new DateTime(2026, 3, 2),
            new DateTime(2026, 3, 3),
            new DateTime(2026, 3, 9),
            new DateTime(2026, 3, 10),
            new DateTime(2026, 3, 16),
            new DateTime(2026, 3, 17),
            new DateTime(2026, 4, 6),
            new DateTime(2026, 4, 7),
            new DateTime(2026, 4, 14),
            new DateTime(2026, 4, 20),
            new DateTime(2026, 4, 21),
            new DateTime(2026, 4, 27),
            new DateTime(2026, 4, 28),
            new DateTime(2026, 4, 29)
        };

        var attendanceRecords = attendanceDates
            .Select(date => new CourseAttendance
            {
                AcademicCourseId = assemblyCourse.Id,
                AttendanceDate = date,
                Status = "Present"
            })
            .ToList();

        await db.CourseAttendanceRecords.AddRangeAsync(attendanceRecords);

        // =========================================================
        // UPDATE STUDENT CGPA
        // =========================================================

        student.CGPA = 2.66;

        await db.SaveChangesAsync();

        Console.WriteLine("==========================================");
        Console.WriteLine("Academic data seeded successfully!");
        Console.WriteLine($"Student: {student.FullName}");
        Console.WriteLine($"Student ID: {student.Id}");
        Console.WriteLine("Semesters: 5");
        Console.WriteLine("Curriculum courses: 44");
        Console.WriteLine("Assessments: 11");
        Console.WriteLine("Attendance records: 18");
        Console.WriteLine("==========================================");
    }

    // =============================================================
    // HELPER METHOD
    // =============================================================

    private static AcademicCourse CreateCourse(
        string courseId,
        string courseName,
        string creditHours,
        string grade,
        bool isRetake = false,
        string? previousGrade = null)
    {
        return new AcademicCourse
        {
            CourseId = courseId,
            CourseName = courseName,
            CreditHours = creditHours,
            Grade = grade,
            ResultStatus = "Completed",
            IsRetake = isRetake,
            PreviousGrade = previousGrade
        };
    }
}