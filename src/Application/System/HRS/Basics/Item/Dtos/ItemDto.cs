namespace Application.System.HRS.Basics.Items.Dtos
{
    public sealed record ItemDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        DateTime? PurchaseDate,
        decimal? PurchasePrice,
        DateTime? ExpiryDate,
        string? LicenseNumber,
        int CompanyId,
        string? CompanyName,
        bool? IsFromAssets,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}