using Application.System.Search.Dtos;

namespace Application.System.Search.Abstractions
{
    public interface IGeneralSearchRepository
    {
         Task<List<SearchColumnResponseDto>> GetSearchColumnsAsync(int searchID, int lang, CancellationToken ct);

        Task<SearchExecuteResultDto> ExecuteSearchAsync(SearchExecuteRequestDto request, int companyId, int language, CancellationToken ct);
    }
}