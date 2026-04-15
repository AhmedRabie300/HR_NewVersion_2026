using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using MediatR;

namespace Application.System.HRS.VacationsPaidType.Queries
{
    public static class GetPagedVacationsPaidTypes
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<VacationsPaidTypeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<VacationsPaidTypeDto>>
        {
            private readonly IVacationsPaidTypeRepository _repo;

            public Handler(IVacationsPaidTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<VacationsPaidTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new VacationsPaidTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<VacationsPaidTypeDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}