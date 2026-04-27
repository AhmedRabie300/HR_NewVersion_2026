namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos
{
    public sealed record GradeStepTransactionDto(
        int Id,
        int GradeStepId,
        int? GradeTransactionId,
        int? CompanyId,
        string? CompanyName,
        decimal? Amount,
        string? Remarks,
        bool? Active,
        DateTime? ActiveDate,
        string? ActiveDateD,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}