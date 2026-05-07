namespace Application.System.HRS.Basics.HICompanies.Dtos
{
    public sealed record CreateHICompanyDto(
        string Code,
        string EngName,
        string ArbName,
        string? ArbName4S,
        string? Remarks,
         List<CreateHICompanyClassDto> Classes
    );
}