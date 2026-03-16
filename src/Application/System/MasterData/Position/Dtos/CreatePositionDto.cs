namespace Application.System.MasterData.Position.Dtos
{
    public sealed record CreatePositionDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        int? PositionLevelId,
        int? EvalEvaluationId,
        int? EvalRecruitmentId,
        string? Remarks,
        int? EmployeesNo,
        bool? ApplyValidation,
        string? PositionBudget,
        int? AppraisalTypeGroupId,
        int? RegUserId,
        string? RegComputerId
    );
}