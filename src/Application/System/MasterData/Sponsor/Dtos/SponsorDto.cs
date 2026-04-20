public sealed record SponsorDto(
    int Id,
    string Code,
     string? EngName,
    string? ArbName,
    string? ArbName4S,
    int? SponsorNumber,
    DateTime? RegDate,
    DateTime? CancelDate,
    bool? IsActive
);