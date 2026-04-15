namespace Application.System.MasterData.DependantType.Dtos
{
    public sealed record UpdateDependantTypeDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}