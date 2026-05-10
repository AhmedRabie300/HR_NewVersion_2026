using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Department.Commands
{
    public static class SoftDeleteDepartment
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IDepartmentRepository _repo;

            public Validator(IValidationMessages msg, IDepartmentRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInEmployeesAsync(id))
                    .WithMessage(msg.Get("DepartmentUsedInEmployees"));

            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDepartmentRepository _repo;

            public Handler(IDepartmentRepository repo)
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