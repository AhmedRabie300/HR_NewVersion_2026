namespace Application.System.MasterData.Profession.Dtos
{
    public sealed record CreateProfessionDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? regComputerId
    );
}