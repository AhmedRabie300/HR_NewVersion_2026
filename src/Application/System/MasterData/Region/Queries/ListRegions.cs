using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Region.Dtos;
using MediatR;

namespace Application.System.MasterData.Region.Queries
{
    public static class ListRegions
    {
        public record Query : IRequest<List<RegionDto>>;

        public class Handler : IRequestHandler<Query, List<RegionDto>>
        {
            private readonly IRegionRepository _repo;
            private readonly IContextService _contextService;

            public Handler(IRegionRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<List<RegionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var items = await _repo.GetAllAsync();

                return items.Select(x => new RegionDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CountryId: x.CountryId,
                    CountryName: lang == 2 ? x.Country?.EngName : x.Country?.ArbName,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}