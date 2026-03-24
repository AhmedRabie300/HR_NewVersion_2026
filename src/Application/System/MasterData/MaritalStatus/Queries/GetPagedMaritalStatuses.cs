using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using MediatR;

namespace Application.System.MasterData.MaritalStatus.Queries
{
    public static class GetPagedMaritalStatuses
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<MaritalStatusDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<MaritalStatusDto>>
        {
            private readonly IMaritalStatusRepository _repo;

            public Handler(IMaritalStatusRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<MaritalStatusDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new MaritalStatusDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();

                return new PagedResult<MaritalStatusDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}