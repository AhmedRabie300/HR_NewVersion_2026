// Application/System/Search/Queries/ExecuteSearch.cs
using Application.Common.Abstractions;
using Application.System.Search.Abstractions;
using Application.System.Search.Dtos;
using MediatR;

namespace Application.System.Search.Queries
{
    public static class ExecuteSearch
    {
        public record Query(SearchExecuteRequestDto Request) : IRequest<SearchExecuteResultDto>;

        public class Handler : IRequestHandler<Query, SearchExecuteResultDto>
        {
            private readonly IGeneralSearchRepository _repo;
            private readonly IContextService _contextService;

            public Handler(IGeneralSearchRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<SearchExecuteResultDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();
                var companyId = _contextService.GetCurrentCompanyId();

                return await _repo.ExecuteSearchAsync(request.Request, lang, companyId, cancellationToken);
            }
        }
    }
}