namespace Application.UARbac.Modules.Dtos
{
    public sealed record UpdateModuleDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Rank,
        string? Remarks,
        int? FormId,
        bool? IsRegistered,
        bool? FiscalYearDependant,
        bool? IsAR,
        bool? IsAP,
        bool? IsGL,
        bool? IsFA,
        bool? IsINV,
        bool? IsHR,
        bool? IsMANF,
        bool? IsSYS
    );
}