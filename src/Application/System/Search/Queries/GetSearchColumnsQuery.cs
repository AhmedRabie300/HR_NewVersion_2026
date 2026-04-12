using Application.System.Search.Dtos;
using Application.System.Search.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.Search.Queries
{
    public static class GetSearchColumns
    {
        public record Query(int SearchID) : IRequest<List<SearchColumnResponseDto>>;

        public class Handler : IRequestHandler<Query, List<SearchColumnResponseDto>>
        {
            private readonly IGeneralSearchRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IGeneralSearchRepository repo, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
            }

            private int GetLanguage()
            {
                var context = _httpContextAccessor.HttpContext;
                var lang = context?.Items["Language"] as int?;
                return lang ?? 1;
            }

            public async Task<List<SearchColumnResponseDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = GetLanguage();
                return await _repo.GetSearchColumnsAsync(request.SearchID, lang, cancellationToken);
            }
        }
    }
}