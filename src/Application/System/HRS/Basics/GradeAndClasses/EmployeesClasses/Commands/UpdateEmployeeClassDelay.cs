using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class UpdateEmployeeClassDelay
    {
        public record Command(UpdateEmployeeClassDelayDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateEmployeeClassDelayValidator(msg));
 
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
                var entity = await _repo.GetDelayByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("EmployeeClassDelay", request.Data.Id));

                entity.Update(
                    fromMin: request.Data.FromMin,
                    toMin: request.Data.ToMin,
                    punishPCT: request.Data.PunishPCT,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateDelayAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}