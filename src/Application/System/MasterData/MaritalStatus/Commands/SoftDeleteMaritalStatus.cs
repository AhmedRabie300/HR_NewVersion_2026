using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.MaritalStatus.Commands
{
    public static class SoftDeleteMaritalStatus
    {
        public record Command(int Id) : IRequest<Unit>;
        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IMaritalStatusRepository _repo;

            public Validator(IValidationMessages msg, IMaritalStatusRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInEmployeesAsync(id))
                    .WithMessage(msg.Get("MaritalStatusUsedInEmployees"));

            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IMaritalStatusRepository _repo;

            public Handler(IMaritalStatusRepository repo)
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