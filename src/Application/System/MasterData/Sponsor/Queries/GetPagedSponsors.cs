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
                // ✅ تحويل CompanyId من int? إلى int
                // لو CompanyId = null، نستخدم 0 أو قيمة افتراضية
                var companyId = request.CompanyId ?? 0;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId 
                );

                var items = pagedResult.Items.Select(x => new SponsorDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    SponsorNumber: x.SponsorNumber,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
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