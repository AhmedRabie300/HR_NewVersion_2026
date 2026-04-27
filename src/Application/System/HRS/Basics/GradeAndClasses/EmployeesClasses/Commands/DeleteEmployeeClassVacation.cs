using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class DeleteEmployeeClassVacation
    {
        public record Command(int Id) : IRequest<bool>;

    

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IEmployeeClassRepository _repo;

            public Handler(IEmployeeClassRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.VacationExistsAsync(request.Id))
                    return false;

                await _repo.DeleteVacationAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}