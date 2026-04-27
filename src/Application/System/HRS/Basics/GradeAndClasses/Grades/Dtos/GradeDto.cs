using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos
{
    public sealed record GradeDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? GradeLevel,
        decimal? FromSalary,
        decimal? ToSalary,
        decimal? RegularHours,
        int? OverTimeTypeId,
        int? CompanyId,
        string? CompanyName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive,
        List<GradeTransactionDto> Transactions
    );
}