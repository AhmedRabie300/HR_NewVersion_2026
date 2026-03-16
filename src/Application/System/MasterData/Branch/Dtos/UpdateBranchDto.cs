namespace Application.System.MasterData.Branch.Dtos
{
    public sealed record UpdateBranchDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        int? CountryId,
        int? CityId,
        bool? DefaultAbsent,
        int? PrepareDay,
        bool? AffectPeriod,
        string? Remarks
    );
}