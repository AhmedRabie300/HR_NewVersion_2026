namespace Application.System.MasterData.Region.Dtos
{
    public sealed record RegionDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CountryId,
        string? CountryName,
        int? CompanyId,
        string? CompanyName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}