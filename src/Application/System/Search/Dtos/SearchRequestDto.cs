// Application/System/Search/Dtos/SearchRequestDto.cs
namespace Application.System.Search.Dtos
{
    public sealed record SearchRequestDto(
        int SearchID,              
        string SearchTerm,           
        int PageNumber = 1,           
        int PageSize = 20,            
        string? SortBy = null,        
        bool SortDescending = false   
    );
}