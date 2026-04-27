using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos
{
    public sealed record GradeStepDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int GradeId,
        string? GradeName,
        int? Step,
        int? CompanyId,
        string? CompanyName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive,
        List<GradeStepTransactionDto> Transactions
    );
}