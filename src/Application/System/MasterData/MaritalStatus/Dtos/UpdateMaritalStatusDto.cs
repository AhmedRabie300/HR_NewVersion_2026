namespace Application.System.MasterData.MaritalStatus.Dtos
{
    public sealed record UpdateMaritalStatusDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}