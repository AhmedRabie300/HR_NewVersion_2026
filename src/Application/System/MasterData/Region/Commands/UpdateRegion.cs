using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Region.Dtos;
using Application.System.MasterData.Region.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Region.Commands
{
    public static class UpdateRegion
    {
        public record Command(UpdateRegionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateRegionValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IRegionRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly ICountryRepository _countryRepo;
            public Handler(
                IRegionRepository repo, IValidationMessages msg,
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
                    throw new NotFoundException(_msg.NotFound("Region", request.Data.Id));

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
                    request.Data.CountryId,
                    request.Data.CompanyId,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}