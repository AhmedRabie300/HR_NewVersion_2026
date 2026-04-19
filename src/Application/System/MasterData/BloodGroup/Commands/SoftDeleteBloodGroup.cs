using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class SoftDeleteBloodGroup
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBloodGroupRepository _repo;

            public Handler(IBloodGroupRepository repo)
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