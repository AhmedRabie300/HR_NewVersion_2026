using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sponsor.Commands
{
    public static class CreateSponsor
    {
        public record Command(CreateSponsorDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                _languageService = languageService;
                _localizer = localizer;

                // ✅ استخدام SetValidator مباشرة
                RuleFor(x => x.Data)
                    .SetValidator(new CreateSponsorValidator(_localizer, _languageService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ISponsorRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(ISponsorRepository repo, ICompanyRepository companyRepo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                // التحقق من وجود الشركة
                if (request.Data.CompanyId.HasValue)
                {
                    var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId.Value);
                    if (company == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Company", lang),
                            request.Data.CompanyId));
                }

                // التحقق من عدم تكرار الكود
                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new Exception(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Sponsor", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Sponsor(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.SponsorNumber,
                    request.Data.RegUserId,
                    request.Data.RegComputerId,
                    request.Data.CompanyId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}