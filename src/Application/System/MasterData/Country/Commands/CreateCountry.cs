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
            public Validator(IValidationMessages msg,ICountryRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateCountryValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICountryRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly ICurrencyRepository _currencyRepo;
            private readonly INationalityRepository _nationalityRepo;
            private readonly IRegionRepository _regionRepo;
            private readonly ICityRepository _cityRepo;
            public Handler(
                ICountryRepository repo, IValidationMessages msg,
                ICurrencyRepository currencyRepo,
                INationalityRepository nationalityRepo,
                IRegionRepository regionRepo,
                ICityRepository cityRepo)
            {
                _repo = repo;
                _msg = msg;
                _currencyRepo = currencyRepo;
                _nationalityRepo = nationalityRepo;
                _regionRepo = regionRepo;
                _cityRepo = cityRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
              

                if (request.Data.CurrencyId.HasValue)
                {
                    var currency = await _currencyRepo.GetByIdAsync(request.Data.CurrencyId.Value);
                    if (currency == null)
                        throw new NotFoundException(_msg.NotFound("Currency", request.Data.CurrencyId.Value));
                }

                if (request.Data.NationalityId.HasValue)
                {
                    var nationality = await _nationalityRepo.GetByIdAsync(request.Data.NationalityId.Value);
                    if (nationality == null)
                        throw new NotFoundException(_msg.NotFound("Nationality", request.Data.NationalityId.Value));
                }

                if (request.Data.RegionId.HasValue)
                {
                    var region = await _regionRepo.GetByIdAsync(request.Data.RegionId.Value);
                    if (region == null)
                        throw new NotFoundException(_msg.NotFound("Region", request.Data.RegionId.Value));
                }

                if (request.Data.CapitalId.HasValue)
                {
                    var capital = await _cityRepo.GetByIdAsync(request.Data.CapitalId.Value);
                    if (capital == null)
                        throw new NotFoundException(_msg.NotFound("City", request.Data.CapitalId.Value));
                }

                var entity = new Domain.System.MasterData.Country(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    currencyId: request.Data.CurrencyId,
                    nationalityId: request.Data.NationalityId,
                    phoneKey: request.Data.PhoneKey,
                    isMainCountries: request.Data.IsMainCountries,
                    remarks: request.Data.Remarks,
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