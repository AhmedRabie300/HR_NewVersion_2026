using Application.System.HRS.Basics.HICompanies.Dtos;

namespace Application.System.HRS.Basics.HICompanies.Dtos
{
    public sealed record HICompanyDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CompanyId,
        string? CompanyName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive,
        List<HICompanyClassDto> Classes
    );
}