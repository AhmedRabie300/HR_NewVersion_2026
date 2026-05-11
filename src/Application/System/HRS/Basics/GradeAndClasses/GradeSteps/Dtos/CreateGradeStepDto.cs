namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos
{
    public sealed record CreateGradeStepDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int GradeId,
        int? Step,
        string? Remarks,
        List<CreateGradeStepTransactionDto> Transactions
    );
}