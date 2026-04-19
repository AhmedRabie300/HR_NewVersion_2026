using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using MediatR;

namespace Application.System.MasterData.City.Queries
{
    public static class ListCities
    {
        public record Query : IRequest<List<CityDto>>;

        public class Handler : IRequestHandler<Query, List<CityDto>>
        {
            private readonly ICityRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(ICityRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<List<CityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = 1;

                var items = await _repo.GetAllAsync();

                return items.Select(x => new CityDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    PhoneKey: x.PhoneKey,
                    RegionId: x.RegionId,
                    RegionName: lang == 2 ? x.Region?.ArbName : x.Region?.EngName,
                    TimeZone: x.TimeZone,
                    CountryId: x.CountryId,
                    CountryName: lang == 2 ? x.Country?.ArbName : x.Country?.EngName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}