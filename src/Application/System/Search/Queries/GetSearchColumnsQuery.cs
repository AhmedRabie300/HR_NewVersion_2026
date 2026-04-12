using Application.System.Search.Dtos;
using Application.System.Search.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Common.Abstractions;

namespace Application.System.Search.Queries
{
    public static class GetSearchColumns
    {
        public record Query(int SearchID) : IRequest<List<SearchColumnResponseDto>>;

        public class Handler : IRequestHandler<Query, List<SearchColumnResponseDto>>
        {
            private readonly IGeneralSearchRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(IGeneralSearchRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

           

            public async Task<List<SearchColumnResponseDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();
                return await _repo.GetSearchColumnsAsync(request.SearchID, lang, cancellationToken);
            }
        }
    }
}