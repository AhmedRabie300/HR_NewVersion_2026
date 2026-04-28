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
    public static class AddEmployeeClassDelay
    {
        public record Command(int ClassId, CreateEmployeeClassDelayDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
             

                RuleFor(x => x.Data)
                    .SetValidator(new CreateEmployeeClassDelayValidator(msg));
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
                var employeeClass = await _repo.GetByIdAsync(request.ClassId);
                if (employeeClass == null)
                    throw new NotFoundException(_msg.NotFound("EmployeeClass", request.ClassId));

                var companyId = _currentUser.CompanyId;

                var entity = new EmployeeClassDelay(
                    classId: request.ClassId,
                    fromMin: request.Data.FromMin,
                    toMin: request.Data.ToMin,
                    punishPCT: request.Data.PunishPCT,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddDelayAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}