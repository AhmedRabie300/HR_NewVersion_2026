using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Country.Dtos;
using Application.System.MasterData.Country.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Country.Commands
{
    public static class CreateCountry
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateCountryDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();

               
                RuleFor(x => x.Data)
                    .SetValidator(new CreateCountryValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICountryRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICurrencyRepository _currencyRepo;
            private readonly INationalityRepository _nationalityRepo;
            private readonly IRegionRepository _regionRepo;
            private readonly ICityRepository _cityRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ICountryRepository repo,
                ICompanyRepository companyRepo,
                ICurrencyRepository currencyRepo,
                INationalityRepository nationalityRepo,
                IRegionRepository regionRepo,
                ICityRepository cityRepo,
                ICodeGenerationService codeGenerationService,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _currencyRepo = currencyRepo;
                _nationalityRepo = nationalityRepo;
                _regionRepo = regionRepo;
                _cityRepo = cityRepo;
                _codeGenerationService = codeGenerationService;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
          
                if (request.Data.CurrencyId.HasValue)
                {
                    var currency = await _currencyRepo.GetByIdAsync(request.Data.CurrencyId.Value);
                    if (currency == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Currency", lang),
                            request.Data.CurrencyId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Currency", lang), request.Data.CurrencyId.Value));
                }

                if (request.Data.NationalityId.HasValue)
                {
                    var nationality = await _nationalityRepo.GetByIdAsync(request.Data.NationalityId.Value);
                    if (nationality == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Nationality", lang),
                            request.Data.NationalityId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Nationality", lang), request.Data.NationalityId.Value));
                }

                if (request.Data.RegionId.HasValue)
                {
                    var region = await _regionRepo.GetByIdAsync(request.Data.RegionId.Value);
                    if (region == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Region", lang),
                            request.Data.RegionId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Region", lang), request.Data.RegionId.Value));
                }

                if (request.Data.CapitalId.HasValue)
                {
                    var capital = await _cityRepo.GetByIdAsync(request.Data.CapitalId.Value);
                    if (capital == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("City", lang),
                            request.Data.CapitalId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("City", lang), request.Data.CapitalId.Value));
                }

                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    (companyId, ct) => _repo.GetMaxCodeAsync(companyId, ct),
                    (code, ct) => _repo.CodeExistsAsync(code),
                    cancellationToken
                );

                var entity = new Domain.System.MasterData.Country(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    currencyId: request.Data.CurrencyId,
                    nationalityId: request.Data.NationalityId,
                    phoneKey: request.Data.PhoneKey,
                    isMainCountries: request.Data.IsMainCountries,
                    remarks: request.Data.Remarks,
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.RegComputerId,
                    regionId: request.Data.RegionId,
                    isoAlpha2: request.Data.ISOAlpha2,
                    isoAlpha3: request.Data.ISOAlpha3,
                    languages: request.Data.Languages,
                    continent: request.Data.Continent,
                    capitalId: request.Data.CapitalId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}