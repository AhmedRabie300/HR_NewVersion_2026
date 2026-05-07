using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using Application.System.HRS.Contracts.Validators;
using Domain.System.HRS.Basics.Contracts;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Contracts.Commands
{
    public static class AddContractTransaction
    {
        public record Command(int ContractId, CreateContractTransactionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IContractRepository repo)
            {
                RuleFor(x => x.ContractId)
                    .GreaterThan(0).WithMessage(x => msg.Get("ContractIdRequired"))
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("Contract"), x.ContractId));

                RuleFor(x => x.Data)
                    .SetValidator(new CreateContractTransactionValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IContractRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var contract = await _repo.GetByIdAsync(request.ContractId);
                if (contract == null)
                    throw new NotFoundException(_msg.NotFound("Contract", request.ContractId));

                var companyId = _currentUser.CompanyId;

                var entity = new ContractTransaction(
                    contractId: request.ContractId,
                    transactionTypeId: request.Data.TransactionTypeId,
                     amount: request.Data.Amount,
                    active: request.Data.Active,
                    intervalId: request.Data.IntervalId,
                    paidAtVacation: request.Data.PaidAtVacation,
                    onceAtPeriod: request.Data.OnceAtPeriod,
                    remarks: request.Data.Remarks,
                     activeDate: request.Data.ActiveDate,
                    activeDateD: request.Data.ActiveDateD
                );

                await _repo.AddTransactionAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}