using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Country.Commands
{
    public static class SoftDeleteCountry
    {
        public record Command(int Id) : IRequest<Unit>;
        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ICountryRepository _repo;

            public Validator(IValidationMessages msg, ICountryRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInCitiesAsync(id))
                    .WithMessage(msg.Get("CountryUsedInCities"));

            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICountryRepository _repo;

            public Handler(ICountryRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repo.SoftDeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}