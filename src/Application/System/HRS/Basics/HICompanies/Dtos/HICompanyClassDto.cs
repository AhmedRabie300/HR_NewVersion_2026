namespace Application.System.HRS.Basics.HICompanies.Dtos
{
    public sealed record HICompanyClassDto(
        int Id,
        int HICompanyId,
        int CompanyId,
        string? CompanyName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        decimal? CompanyAmount,
        decimal? EmployeeAmount,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}