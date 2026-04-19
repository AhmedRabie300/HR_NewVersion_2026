using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Country.Dtos;
using MediatR;

namespace Application.System.MasterData.Country.Queries
{
    public static class ListCountries
    {
        public record Query : IRequest<List<CountryDto>>;

        public class Handler : IRequestHandler<Query, List<CountryDto>>
        {
            private readonly ICountryRepository _repo;
            private readonly IContextService _contextService;

            public Handler(ICountryRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<List<CountryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = 1;

                var items = await _repo.GetAllAsync();

                return items.Select(x => new CountryDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CurrencyId: x.CurrencyId,
                    CurrencyName: lang == 2 ? x.Currency?.ArbName : x.Currency?.EngName,
                    NationalityId: x.NationalityId,
                    NationalityName: lang == 2 ? x.Nationality?.ArbName : x.Nationality?.EngName,
                    PhoneKey: x.PhoneKey,
                    IsMainCountries: x.IsMainCountries,
                    Remarks: x.Remarks,
                    RegionId: x.RegionId,
                    RegionName: lang == 2 ? x.Region?.ArbName : x.Region?.EngName,
                    ISOAlpha2: x.ISOAlpha2,
                    ISOAlpha3: x.ISOAlpha3,
                    Languages: x.Languages,
                    Continent: x.Continent,
                    CapitalId: x.CapitalId,
                    CapitalName: lang == 2 ? x.Capital?.ArbName : x.Capital?.EngName,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}