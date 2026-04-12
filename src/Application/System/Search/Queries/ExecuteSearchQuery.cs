// Application/System/Search/Queries/ExecuteSearchQuery.cs
using Application.System.Search.Dtos;
using Application.System.Search.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.Search.Queries
{
    public static class ExecuteSearch
    {
        public record Query(SearchExecuteRequestDto Request) : IRequest<SearchExecuteResultDto>;

        public class Handler : IRequestHandler<Query, SearchExecuteResultDto>
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

            private int GetCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                return companyId ?? 0;
            }

            public async Task<SearchExecuteResultDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = GetLanguage();
                var companyId = GetCompanyId();
                return await _repo.ExecuteSearchAsync(request.Request, lang, companyId, cancellationToken);
            }
        }
    }
}