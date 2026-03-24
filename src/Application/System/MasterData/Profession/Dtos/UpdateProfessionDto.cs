namespace Application.System.MasterData.Profession.Dtos
{
    public sealed record UpdateProfessionDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}