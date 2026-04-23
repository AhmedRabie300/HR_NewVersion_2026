namespace Application.System.HRS.Basics.Items.Dtos
{
    public sealed record CreateItemDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        DateTime? PurchaseDate,
        decimal? PurchasePrice,
        DateTime? ExpiryDate,
        string? LicenseNumber,
        bool? IsFromAssets,
        string? Remarks,
        int? RegComputerId
    );
}