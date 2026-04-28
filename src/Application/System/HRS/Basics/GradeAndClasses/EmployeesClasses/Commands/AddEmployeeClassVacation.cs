using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators;
using Domain.System.HRS.Basics.GradesAndClasses;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class AddEmployeeClassVacation
    {
        public record Command(int EmployeeClassId, CreateEmployeeClassVacationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
             

                RuleFor(x => x.Data)
                    .SetValidator(new CreateEmployeeClassVacationValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IEmployeeClassRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var employeeClass = await _repo.GetByIdAsync(request.EmployeeClassId);
                if (employeeClass == null)
                    throw new NotFoundException(_msg.NotFound("EmployeeClass", request.EmployeeClassId));

                var companyId = _currentUser.CompanyId;

                var entity = new EmployeeClassVacation(
                    employeeClassId: request.EmployeeClassId,
                    vacationTypeId: request.Data.VacationTypeId,
                    durationDays: request.Data.DurationDays,
                    requiredWorkingMonths: request.Data.RequiredWorkingMonths,
                    fromMonth: request.Data.FromMonth,
                    toMonth: request.Data.ToMonth,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    ticketsRnd: request.Data.TicketsRnd,
                    dependantTicketRnd: request.Data.DependantTicketRnd,
                    maxKeepDays: request.Data.MaxKeepDays,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddVacationAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}