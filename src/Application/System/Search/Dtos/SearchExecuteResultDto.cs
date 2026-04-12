namespace Application.System.Search.Dtos
{
    public sealed record SearchExecuteResultDto(
        int SearchID,
        string SearchName,
        int TotalCount,
        int PageNumber,
        int PageSize,
        List<Dictionary<string, object>> Items
    );
}