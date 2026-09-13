namespace UniMateAI.Api.DTOs;

public class AcademicSemesterDto
{
    public int Id { get; set; }
    public string Session { get; set; } = string.Empty;
    public string SemesterName { get; set; } = string.Empty;
    public int SemesterNumber { get; set; }
    public double? GPA { get; set; }
    public double? CGPA { get; set; }
    public string Status { get; set; } = string.Empty;

    public decimal SemesterFee { get; set; }
    public decimal RebateOrConcession { get; set; }
    public decimal FeeReceived { get; set; }
    public decimal FeeRemaining { get; set; }
    public decimal PreviousOutstanding { get; set; }
    public decimal TotalOutstanding { get; set; }

    public string? AdditionalFeeComments { get; set; }
    public string? RebateComments { get; set; }
    public string? PaymentComments { get; set; }

    public List<AcademicCourseDto> Courses { get; set; } = new();
}