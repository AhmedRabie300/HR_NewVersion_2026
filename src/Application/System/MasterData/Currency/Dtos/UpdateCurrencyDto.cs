namespace Application.System.MasterData.Currency.Dtos
{
    public sealed record UpdateCurrencyDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? EngSymbol,
        string? ArbSymbol,
        int? DecimalFraction,
        string? DecimalEngName,
        string? DecimalArbName,
        decimal? Amount,
        int? NoDecimalPlaces,
        string? Remarks
    );
}