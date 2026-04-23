namespace Application.System.HRS.Basics.HICompanies.Dtos
{
    public sealed record UpdateHICompanyDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}