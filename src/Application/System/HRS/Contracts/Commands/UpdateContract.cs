using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using Application.System.HRS.Contracts.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Contracts.Commands
{
    public static class UpdateContract
    {
        public record Command(UpdateContractDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IContractRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateContractValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IContractRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IContractRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Contract", request.Data.Id));

                entity.Update(
                    contractTypeId: request.Data.ContractTypeId,
                    employeeClassId: request.Data.EmployeeClassId,
                    startDate: request.Data.StartDate,
                    endDate: request.Data.EndDate,
                    professionId: request.Data.ProfessionId,
                    positionId: request.Data.PositionId,
                    gradeStepId: request.Data.GradeStepId,
                    currencyId: request.Data.CurrencyId,
                    remarks: request.Data.Remarks,
                    contractPeriod: request.Data.ContractPeriod
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}