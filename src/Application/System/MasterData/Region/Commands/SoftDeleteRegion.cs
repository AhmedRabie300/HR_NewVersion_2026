using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Region.Commands
{
    public static class SoftDeleteRegion
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IRegionRepository _repo;

            public Validator(IValidationMessages msg, IRegionRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInCountriesAsync(id))
                    .WithMessage(msg.Get("RegionUsedInCountries"));

            }
        }


        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IRegionRepository _repo;

            public Handler(IRegionRepository repo)
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