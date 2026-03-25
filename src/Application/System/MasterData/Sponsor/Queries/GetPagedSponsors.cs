using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using MediatR;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class GetPagedSponsors
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId)
            : IRequest<PagedResult<SponsorDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<SponsorDto>>
        {
            private readonly ISponsorRepository _repo;

            public Handler(ISponsorRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<SponsorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(x => new SponsorDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.SponsorNumber,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();

                return new PagedResult<SponsorDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}