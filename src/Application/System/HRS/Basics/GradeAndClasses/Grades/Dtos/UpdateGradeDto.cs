namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos
{
    public sealed record UpdateGradeDto(
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
        string? Remarks
    );
}