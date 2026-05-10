using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Profession.Commands
{
    public static class SoftDeleteProfession
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IProfessionRepository _repo;

            public Validator(IValidationMessages msg, IProfessionRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInEmployeesContractsAsync(id))
                    .WithMessage(msg.Get("ProfessionUsedInContracts"));

            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IProfessionRepository _repo;

            public Handler(IProfessionRepository repo)
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