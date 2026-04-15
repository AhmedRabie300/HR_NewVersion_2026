namespace Application.System.MasterData.DependantType.Dtos
{
    public sealed record CreateDependantTypeDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? regComputerId
    );
}