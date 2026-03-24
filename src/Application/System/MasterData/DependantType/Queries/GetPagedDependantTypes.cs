using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using MediatR;

namespace Application.System.MasterData.DependantType.Queries
{
    public static class GetPagedDependantTypes
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId)
            : IRequest<PagedResult<DependantTypeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<DependantTypeDto>>
        {
            private readonly IDependantTypeRepository _repo;

            public Handler(IDependantTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<DependantTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(x => new DependantTypeDto(
                    x.Id,
                    x.Code,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();

                return new PagedResult<DependantTypeDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}