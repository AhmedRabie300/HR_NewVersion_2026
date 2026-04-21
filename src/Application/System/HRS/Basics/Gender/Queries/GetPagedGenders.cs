using Application.Common.Models;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using MediatR;
using Application.System.HRS.Basics.Gender.Dtos;

namespace Application.System.HRS.Basics.Gender.Queries
{
    public static class GetPagedGenders
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<GenderDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<GenderDto>>
        {
            private readonly IGenderRepository _repo;

            public Handler(IGenderRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<GenderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new GenderDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<GenderDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}