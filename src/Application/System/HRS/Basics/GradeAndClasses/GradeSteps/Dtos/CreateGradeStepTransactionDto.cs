namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos
{
    public sealed record CreateGradeStepTransactionDto(
        int? GradeTransactionId,
        decimal? Amount,
        string? Remarks,
        bool? Active,
        DateTime? ActiveDate,
        string? ActiveDateD,
        int? RegComputerId
    );
}