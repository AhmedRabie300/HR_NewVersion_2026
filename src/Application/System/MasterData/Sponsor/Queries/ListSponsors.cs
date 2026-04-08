// Application/System/MasterData/Sponsor/Queries/ListSponsors.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class ListSponsors
    {
        public record Query : IRequest<List<SponsorDto>>;

        public class Handler : IRequestHandler<Query, List<SponsorDto>>
        {
            private readonly ISponsorRepository _repo;
            private readonly ILanguageService _languageService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(
                ISponsorRepository repo,
                ILanguageService languageService,
                IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _languageService = languageService;
                _httpContextAccessor = httpContextAccessor;
            }

            private int? GetCompanyIdFromContext()
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null && context.Items.TryGetValue("CompanyId", out var companyId))
                {
                    return companyId as int?;
                }
                return null;
            }

            private int GetRequiredCompanyId()
            {
                var companyId = GetCompanyIdFromContext();
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<List<SponsorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new SponsorDto(
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
            }
        }
    }
}