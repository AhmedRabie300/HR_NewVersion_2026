using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Country.Dtos;
using Application.System.MasterData.Country.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Country.Commands
{
    public static class UpdateCountry
    {
        public record Command(UpdateCountryDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, ICountryRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateCountryValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICountryRepository _repo;
            private readonly IValidationMessages _msg;
            private readonly ICurrencyRepository _currencyRepo;
            private readonly INationalityRepository _nationalityRepo;
            private readonly IRegionRepository _regionRepo;
            private readonly ICityRepository _cityRepo;

            public Handler(
                ICountryRepository repo,
                IValidationMessages msg,
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Country", request.Data.Id));

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