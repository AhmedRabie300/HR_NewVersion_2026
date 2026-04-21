namespace Application.System.HRS.Basics.Gender.Dtos
{
    public sealed record GenderDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}