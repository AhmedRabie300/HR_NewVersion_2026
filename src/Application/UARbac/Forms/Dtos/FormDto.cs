// Application/UARbac/Forms/Dtos/FormDto.cs
namespace Application.UARbac.Forms.Dtos
{
    public sealed record FormDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Rank,
        int ModuleId,
        string? ModuleName
    );
}