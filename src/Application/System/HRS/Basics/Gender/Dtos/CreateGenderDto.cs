namespace Application.System.HRS.Basics.Gender.Dtos
{
    public sealed record CreateGenderDto(
        string? Code,
        string? EngName,
        string? ArbName,
        int? regComputerId
    );
}