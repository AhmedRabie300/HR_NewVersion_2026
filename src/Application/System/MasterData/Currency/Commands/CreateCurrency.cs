using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using Application.System.MasterData.Currency.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Currency.Commands
{
    public static class CreateCurrency
    {
        public record Command(CreateCurrencyDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateCurrencyValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICurrencyRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(ICurrencyRepository repo, ICompanyRepository companyRepo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                if (request.Data.CompanyId.HasValue)
                {
                    var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId.Value);
                    if (company == null)
                        throw new NotFoundException("Create Currency",string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Company", lang),
                            request.Data.CompanyId));
                }

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Currency", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Currency(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.EngSymbol,
                    request.Data.ArbSymbol,
                    request.Data.DecimalFraction,
                    request.Data.DecimalEngName,
                    request.Data.DecimalArbName,
                    request.Data.Amount,
                    request.Data.NoDecimalPlaces,
                    request.Data.CompanyId,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}