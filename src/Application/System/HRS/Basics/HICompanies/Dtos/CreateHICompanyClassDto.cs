namespace Application.System.HRS.Basics.HICompanies.Dtos
{
    public sealed record CreateHICompanyClassDto(
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        decimal? CompanyAmount,
        decimal? EmployeeAmount,
        int? RegComputerId
    );
}