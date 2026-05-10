using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Currency.Commands
{
    public static class SoftDeleteCurrency
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ICurrencyRepository _repo;

            public Validator(IValidationMessages msg, ICurrencyRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInContractAsync(id))
                    .WithMessage(msg.Get("CurrencyUsedInContract"));

            }
        }


        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICurrencyRepository _repo;

            public Handler(ICurrencyRepository repo)
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