using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class GetSponsorById
    {
        public record Query(int Id) : IRequest<SponsorDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly ILanguageService _languageService;

            public Validator(ILocalizationService localizer, ILanguageService languageService)
            {
                _localizer = localizer;
                _languageService = languageService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _languageService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, SponsorDto>
        {
            private readonly ISponsorRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;  // ← أضف هذا

            public Handler(
                ISponsorRepository repo,
                IHttpContextAccessor httpContextAccessor,
                ILanguageService languageService,
                ILocalizationService localizer)  // ← أضف في الـ Constructor
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
                _languageService = languageService;
                _localizer = localizer;
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

            public async Task<SponsorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Sponsor", lang),
                        request.Id));

                // التأكد أن الكفيل تابع للشركة الحالية
                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Sponsor does not belong to your company");

                return new SponsorDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    SponsorNumber: entity.SponsorNumber,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}