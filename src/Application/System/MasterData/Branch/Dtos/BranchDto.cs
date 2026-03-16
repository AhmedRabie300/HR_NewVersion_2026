namespace Application.System.MasterData.Branch.Dtos
{
    public sealed record BranchDto(
        int Id,
        string Code,
        int CompanyId,
        string? CompanyName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? ParentBranchName,
        int? CountryId,
        int? CityId,
        bool? DefaultAbsent,
        int? PrepareDay,
        bool? AffectPeriod,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}