namespace Application.UARbac.Modules.Dtos
{
    public sealed record CreateModuleDto(
        string Code,
        string? Prefix,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? FormId,
        bool? IsRegistered,
        bool? FiscalYearDependant,
        int? Rank,
        string? Remarks,
        bool? IsAR,
        bool? IsAP,
        bool? IsGL,
        bool? IsFA,
        bool? IsINV,
        bool? IsHR,
        bool? IsMANF,
        bool? IsSYS,
        int? RegUserId,
        int? regComputerId
    );
}