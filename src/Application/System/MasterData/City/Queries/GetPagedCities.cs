using Application.Common.Models;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using MediatR;

namespace Application.System.MasterData.City.Queries
{
    public static class GetPagedCities
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<CityDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<CityDto>>
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

            public async Task<PagedResult<CityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new CityDto(
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

                return new PagedResult<CityDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}