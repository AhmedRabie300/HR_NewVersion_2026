using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Country.Dtos;
using Application.System.MasterData.Country.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Country.Commands
{
    public static class UpdateCountry
    {
        public record Command(UpdateCountryDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateCountryValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICountryRepository _repo;
            private readonly ICurrencyRepository _currencyRepo;
            private readonly INationalityRepository _nationalityRepo;
            private readonly IRegionRepository _regionRepo;
            private readonly ICityRepository _cityRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ICountryRepository repo,
                ICurrencyRepository currencyRepo,
                INationalityRepository nationalityRepo,
                IRegionRepository regionRepo,
                ICityRepository cityRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _currencyRepo = currencyRepo;
                _nationalityRepo = nationalityRepo;
                _regionRepo = regionRepo;
                _cityRepo = cityRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Country", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Country", lang), request.Data.Id));

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

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.CurrencyId,
                    request.Data.NationalityId,
                    request.Data.PhoneKey,
                    request.Data.IsMainCountries,
                    request.Data.Remarks,
                    request.Data.RegionId,
                    request.Data.ISOAlpha2,
                    request.Data.ISOAlpha3,
                    request.Data.Languages,
                    request.Data.Continent,
                    request.Data.CapitalId
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}