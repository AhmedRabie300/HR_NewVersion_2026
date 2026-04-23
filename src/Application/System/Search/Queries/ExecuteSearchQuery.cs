using Application.Abstractions;
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
            private readonly ICurrentUser _currentUser;

            public Handler(IGeneralSearchRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<SearchExecuteResultDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;
                var language = _currentUser.Language;

                return await _repo.ExecuteSearchAsync(request.Request, companyId, language, cancellationToken);
            }
        }
    }
}