using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Bank.Commands
{
    public static class SoftDeleteBank
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IBankRepository _repo;

            public Validator(IValidationMessages msg, IBankRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInEmployeesAsync(id))
                    .WithMessage(msg.Get("BankUsedInEmployees"));
                 
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBankRepository _repo;

            public Handler(IBankRepository repo)
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