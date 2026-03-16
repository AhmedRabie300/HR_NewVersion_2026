namespace Application.System.MasterData.Position.Dtos
{
    public sealed record PositionDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? ParentPositionName,   
        int? PositionLevelId,
        string? PositionLevelName,
        int? EvalEvaluationId,
        string? EvalEvaluationName,
        int? EvalRecruitmentId,
        string? EvalRecruitmentName,
        string? Remarks,
        int? EmployeesNo,
        bool? ApplyValidation,
        string? PositionBudget,
        int? AppraisalTypeGroupId,
        string? AppraisalTypeGroupName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    )
    {
        public string? GetDisplayName(int lang) => lang == 2 ? ArbName : EngName;
    }
}