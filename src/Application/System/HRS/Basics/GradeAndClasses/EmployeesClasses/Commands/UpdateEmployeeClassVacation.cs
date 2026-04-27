using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class UpdateEmployeeClassVacation
    {
        public record Command(UpdateEmployeeClassVacationDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateEmployeeClassVacationValidator(msg));

          
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IEmployeeClassRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetVacationByIdAsync(request.Data.Id);
             

                entity.Update(
                    durationDays: request.Data.DurationDays,
                    requiredWorkingMonths: request.Data.RequiredWorkingMonths,
                    fromMonth: request.Data.FromMonth,
                    toMonth: request.Data.ToMonth,
                    remarks: request.Data.Remarks,
                    ticketsRnd: request.Data.TicketsRnd,
                    dependantTicketRnd: request.Data.DependantTicketRnd,
                    maxKeepDays: request.Data.MaxKeepDays
                );

                await _repo.UpdateVacationAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}