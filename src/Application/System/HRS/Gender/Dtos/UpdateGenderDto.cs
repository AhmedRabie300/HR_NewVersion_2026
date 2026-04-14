namespace Application.System.HRS.Gender.Dtos
{
    public sealed record UpdateGenderDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName
    );
}