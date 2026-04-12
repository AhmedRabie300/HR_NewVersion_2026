namespace Application.System.Search.Dtos
{
    public sealed record SearchExecuteRequestDto(
        int SearchID,
        Dictionary<string, string> Criteria,  
        int PageNumber = 1,
        int PageSize = 20,
        string? SortBy = null,
        bool SortDescending = false
    );
}