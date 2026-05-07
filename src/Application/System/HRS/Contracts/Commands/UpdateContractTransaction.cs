using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using Application.System.HRS.Contracts.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Contracts.Commands
{
    public static class UpdateContractTransaction
    {
        public record Command(UpdateContractTransactionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IContractRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateContractTransactionValidator(msg));

                RuleFor(x => x.Data.ContractId)
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("Contract"), x.Data.ContractId));
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
                var entity = await _repo.GetTransactionByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("ContractTransaction", request.Data.Id));

                entity.Update(
                    amount: request.Data.Amount,
                    active: request.Data.Active,
                    intervalId: request.Data.IntervalId,
                    paidAtVacation: request.Data.PaidAtVacation,
                    onceAtPeriod: request.Data.OnceAtPeriod,
                    remarks: request.Data.Remarks,
                    activeDate: request.Data.ActiveDate,
                    activeDateD: request.Data.ActiveDateD
                );

                await _repo.UpdateTransactionAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}