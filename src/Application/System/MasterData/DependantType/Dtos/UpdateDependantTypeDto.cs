namespace Application.System.MasterData.DependantType.Dtos
{
    public sealed record UpdateDependantTypeDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}