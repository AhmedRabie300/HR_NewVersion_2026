namespace Application.System.HRS.Gender.Dtos
{
    public sealed record CreateGenderDto(
        string? Code,
        string? EngName,
        string? ArbName,
        int? regComputerId
    );
}