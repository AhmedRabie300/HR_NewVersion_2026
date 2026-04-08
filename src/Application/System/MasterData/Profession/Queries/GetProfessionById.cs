// Application/System/MasterData/Profession/Queries/GetProfessionById.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Profession.Queries
{
    public static class GetProfessionById
    {
        public record Query(int Id) : IRequest<ProfessionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly ILanguageService _languageService;

            public Validator(ILocalizationService localizer, ILanguageService languageService)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _languageService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, ProfessionDto>
        {
            private readonly IProfessionRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IProfessionRepository repo,
                IHttpContextAccessor httpContextAccessor,
                ILanguageService languageService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
                _languageService = languageService;
                _localizer = localizer;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<ProfessionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Profession", lang),
                        request.Id));

                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Profession does not belong to your company");

                return new ProfessionDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}