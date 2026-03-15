namespace Application.UARbac.Groups.Dtos
{
    public sealed record GroupDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        DateTime RegDate,
        DateTime? CancelDate
    );
}