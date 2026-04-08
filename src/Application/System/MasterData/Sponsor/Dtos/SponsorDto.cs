public sealed record SponsorDto(
    int Id,
    string Code,
    int CompanyId,
    string? CompanyName,
    string? EngName,
    string? ArbName,
    string? ArbName4S,
    int? SponsorNumber,
    DateTime RegDate,
    DateTime? CancelDate,
    bool IsActive
);