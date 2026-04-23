namespace Application.System.HRS.Basics.ContractsTypes.Dtos
{
    public sealed record ContractsTypeDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CompanyId,
        bool? IsSpecial,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}