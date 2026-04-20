namespace Application.System.MasterData.Sector.Dtos
{
    public sealed record SectorDto(
        int Id,
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? ParentSectorName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    )
    {
        public string? GetDisplayName(int lang) => lang == 2 ? ArbName : EngName;
    }
}