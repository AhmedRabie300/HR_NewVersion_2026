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
        public record Command(CreateCountryDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly ICompanyRepository _companyRepo;

            public Validator(
                IContextService contextService,
                ILocalizationService localizer,
                ICompanyRepository companyRepo)
            {
                _contextService = contextService;
                _localizer = localizer;
                _companyRepo = companyRepo;

                RuleFor(x => x.Data)
                    .CustomAsync(async (data, context, ct) =>
                    {
                        var companyId = _contextService.GetCurrentCompanyId();
                        var company = await _companyRepo.GetByIdAsync(companyId);
                        var lang = _contextService.GetCurrentLanguage();

                        if (company?.HasSequence != true)
                        {
                            if (string.IsNullOrWhiteSpace(data.Code))
                            {
                                context.AddFailure("Code", _localizer.GetMessage("CodeRequired", lang));
                            }
                            else if (data.Code.Length > 50)
                            {
                                context.AddFailure("Code", string.Format(_localizer.GetMessage("MaxLength", lang), 50));
                            }
                        }

                        var validator = new CreateCountryValidator(_localizer, _contextService);
                        var result = await validator.ValidateAsync(data, ct);
                        foreach (var error in result.Errors)
                        {
                            context.AddFailure(error);
                        }
                    });
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
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ICountryRepository repo,
                ICompanyRepository companyRepo,
                ICurrencyRepository currencyRepo,
                INationalityRepository nationalityRepo,
                IRegionRepository regionRepo,
                ICityRepository cityRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _currencyRepo = currencyRepo;
                _nationalityRepo = nationalityRepo;
                _regionRepo = regionRepo;
                _cityRepo = cityRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
           

                  
                                    

                string code;

                if (company?.HasSequence == true)
                {
                    var separator = company.Separator ?? "-";
                    var sequenceLength = company.SequenceLength ?? 5;
                    var maxCode = await _repo.GetMaxCodeAsync(companyId, cancellationToken);
                    int lastNumber = 0;

                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        if (maxCode.Contains(separator))
                        {
                            var lastPart = maxCode.Split(separator).Last();
                            int.TryParse(lastPart, out lastNumber);
                        }
                        else
                        {
                            int.TryParse(maxCode, out lastNumber);
                        }
                    }

                    var newNumber = lastNumber + 1;
                    var formattedNumber = newNumber.ToString($"D{sequenceLength}");
                    code = formattedNumber.ToString();
                }
                else
                {
                    code = request.Data.Code;

                    if (string.IsNullOrWhiteSpace(code))
                        throw new NotFoundException("CodeRequired", _localizer.GetMessage("CodeRequired", lang));

                    if (await _repo.CodeExistsAsync(code))
                        throw new ConflictException(string.Format(
                            _localizer.GetMessage("CodeExists", lang),
                            _localizer.GetMessage("Country", lang),
                            code));
                }

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
                    regUserId: request.Data.RegUserId,
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