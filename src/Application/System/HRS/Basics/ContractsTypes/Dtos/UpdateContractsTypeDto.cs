namespace Application.System.HRS.Basics.ContractsTypes.Dtos
{
    public sealed record UpdateContractsTypeDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsSpecial,
        string? Remarks
    );
}