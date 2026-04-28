namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos
{
    public sealed record UpdateGradeStepTransactionDto(
        int Id,
        int GradeStepId,
        int? GradeTransactionId,
        decimal? Amount,
        string? Remarks,
        bool? Active,
        DateTime? ActiveDate,
        string? ActiveDateD
    );
}