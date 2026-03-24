namespace Application.System.MasterData.Currency.Dtos
{
    public sealed record CurrencyDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? EngSymbol,
        string? ArbSymbol,
        int DecimalFraction,
        string? DecimalEngName,
        string? DecimalArbName,
        decimal? Amount,
        int? NoDecimalPlaces,
        int? CompanyId,
        string? CompanyName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}