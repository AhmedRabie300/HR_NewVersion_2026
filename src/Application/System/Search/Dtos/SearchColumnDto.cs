namespace Application.System.Search.Dtos
{
    public sealed record SearchColumnDto(
        int FieldID,
        string FieldName,
        string DisplayName,
        string? FieldType,
        int? FieldLength,
        bool IsCriteria,
        bool IsView,
        int? RankCriteria,
        int? RankView
    );
}