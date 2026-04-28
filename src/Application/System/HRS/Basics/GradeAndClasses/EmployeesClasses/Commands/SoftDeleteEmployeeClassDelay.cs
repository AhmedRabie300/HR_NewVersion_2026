using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class SoftDeleteEmployeeClassDelay
    {
        public record Command(int Id, int? RegUserId = null) : IRequest<Unit>;

    
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEmployeeClassRepository _repo;

            public Handler(IEmployeeClassRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repo.SoftDeleteDelayAsync(request.Id, request.RegUserId);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}