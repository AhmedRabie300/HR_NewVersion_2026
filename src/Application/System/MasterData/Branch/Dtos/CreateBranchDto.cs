namespace Application.System.MasterData.Branch.Dtos
{
    public sealed record CreateBranchDto(
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        int? CountryId,
        int? CityId,
        bool? DefaultAbsent,
        int? PrepareDay,
        bool? AffectPeriod,
        string? Remarks,
        int? RegUserId,
        int? regComputerId
    );
}