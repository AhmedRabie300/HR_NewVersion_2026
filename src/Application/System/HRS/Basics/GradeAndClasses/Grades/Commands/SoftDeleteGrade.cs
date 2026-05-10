using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Commands
{
    public static class SoftDeleteGrade
    {
        public record Command(int Id ) : IRequest<Unit>;


        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IGradeRepository _repo;

            public Validator(IValidationMessages msg, IGradeRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
          .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"))
          .MustAsync(async (id, cancellation) => !await _repo.IsUsedInGradeStepAsync(id))
          .WithMessage(x => msg.Get("GradeUsedInGradeSteps"));
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGradeRepository _repo;

            public Handler(IGradeRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repo.SoftDeleteAsync(request.Id );
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}