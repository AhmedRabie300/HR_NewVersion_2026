namespace Application.System.HRS.Basics.HICompanies.Dtos
{
    public sealed record UpdateHICompanyClassDto(
        int Id,
        int HICompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        decimal? CompanyAmount,
        decimal? EmployeeAmount
    );
}