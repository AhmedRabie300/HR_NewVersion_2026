using Application.Common.Models;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Country.Dtos;
using MediatR;

namespace Application.System.MasterData.Country.Queries
{
    public static class GetPagedCountries
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<CountryDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<CountryDto>>
        {
            private readonly ICountryRepository _repo;
            private readonly IContextService _contextService;

            public Handler(ICountryRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<PagedResult<CountryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = 1;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new CountryDto(
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

                return new PagedResult<CountryDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}