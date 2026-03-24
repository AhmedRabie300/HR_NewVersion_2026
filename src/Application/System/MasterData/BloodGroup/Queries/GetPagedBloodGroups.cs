using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Queries
{
    public static class GetPagedBloodGroups
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm) : IRequest<PagedResult<BloodGroupDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<BloodGroupDto>>
        {
            private readonly IBloodGroupRepository _repo;

            public Handler(IBloodGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<BloodGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new BloodGroupDto(
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

                return new PagedResult<BloodGroupDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}