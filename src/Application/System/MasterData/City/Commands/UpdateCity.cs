using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using Application.System.MasterData.City.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.City.Commands
{
    public static class UpdateCity
    {
        public record Command(UpdateCityDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateCityValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICityRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly ICountryRepository _countryRepo;
            public Handler(
                ICityRepository repo, IValidationMessages msg,
                ICountryRepository countryRepo)
            {
                _repo = repo;
                _msg = msg;
                _countryRepo = countryRepo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("City", request.Data.Id));

                if (request.Data.CountryId.HasValue)
                {
                    var country = await _countryRepo.GetByIdAsync(request.Data.CountryId.Value);
                    if (country == null)
                        throw new NotFoundException(_msg.NotFound("Country", request.Data.CountryId.Value));
                }

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.PhoneKey,
                    request.Data.RegionId,
                    request.Data.TimeZone,
                    request.Data.CountryId,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}