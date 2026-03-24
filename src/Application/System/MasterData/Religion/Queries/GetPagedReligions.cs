using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using MediatR;

namespace Application.System.MasterData.Religion.Queries
{
    public static class GetPagedReligions
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm) : IRequest<PagedResult<ReligionDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ReligionDto>>
        {
            private readonly IReligionRepository _repo;

            public Handler(IReligionRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<ReligionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(r => new ReligionDto(
                    Id: r.Id,
                    Code: r.Code,
                    EngName: r.EngName,
                    ArbName: r.ArbName,
                    ArbName4S: r.ArbName4S,
                    Remarks: r.Remarks,
                    RegDate: r.RegDate,
                    CancelDate: r.CancelDate,
                    IsActive: r.IsActive()
                )).ToList();

                return new PagedResult<ReligionDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}