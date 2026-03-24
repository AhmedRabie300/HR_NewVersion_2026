namespace Application.System.MasterData.MaritalStatus.Dtos
{
    public sealed record CreateMaritalStatusDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegUserId,
        int? regComputerId
    );
}