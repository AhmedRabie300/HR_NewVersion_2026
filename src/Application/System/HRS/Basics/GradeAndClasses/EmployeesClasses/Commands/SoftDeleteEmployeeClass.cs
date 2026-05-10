using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class SoftDeleteEmployeeClass
    {
        public record Command(int Id) : IRequest<Unit>;


        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IEmployeeClassRepository _repo;

            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
          .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"))
          .MustAsync(async (id, cancellation) => !await _repo.IsUsedInContractsAsync(id))
          .WithMessage(x => msg.Get("EmployeeClassUsedInActiveContracts"));
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEmployeeClassRepository _repo;

            public Handler(IEmployeeClassRepository repo)
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