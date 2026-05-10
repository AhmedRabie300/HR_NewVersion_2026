using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Commands
{
    public static class SoftDeleteGradeStep
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IGradeStepRepository _repo;

            public Validator(IValidationMessages msg, IGradeStepRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
          .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"))
          .MustAsync(async (id, cancellation) => !await _repo.IsUsedInEmployeeContractAsync(id))
          .WithMessage(x => msg.Get("GradeStepUsedInEmployeeContract"));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGradeStepRepository _repo;

            public Handler(IGradeStepRepository repo)
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