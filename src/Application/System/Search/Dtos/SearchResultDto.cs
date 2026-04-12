namespace Application.System.Search.Dtos
{
    public sealed record SearchResultDto(
        int SearchID,
        string ObjectName,
        int TotalCount,
        int PageNumber,
        int PageSize,
        List<SearchResultItemDto> Items
    );

    public sealed record SearchResultItemDto(
        int Id,
        string Code,
        string DisplayName,
        Dictionary<string, object> Fields,
        DateTime? CancelDate
    );
}