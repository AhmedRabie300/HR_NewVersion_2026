namespace Application.System.HRS.Basics.Gender.Dtos
{
    public sealed record UpdateGenderDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName
    );
}