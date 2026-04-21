using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Validators;
using Domain.System.HRS.Basics.FiscalTransactions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Commands
{
    public static class UpdateTransactionsType
    {
        public record Command(UpdateTransactionsTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(
                IValidationMessages msg,
                ITransactionsTypeRepository repo,
                ITransactionsGroupRepository transactionsGroupRepo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateTransactionsTypeValidator(msg, repo, transactionsGroupRepo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ITransactionsTypeRepository _repo;
            private readonly IValidationMessages _msg;
            private readonly ITransactionsGroupRepository _transactionsGroupRepo;

            public Handler(
                ITransactionsTypeRepository repo,
                IValidationMessages msg,
                ITransactionsGroupRepository transactionsGroupRepo)
            {
                _repo = repo;
                _msg = msg;
                _transactionsGroupRepo = transactionsGroupRepo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("TransactionsType", request.Data.Id));

                // التحقق من وجود مجموعة المعاملات إذا تم تغييرها
                if (request.Data.TransactionGroupId.HasValue)
                {
                    var transactionGroup = await _transactionsGroupRepo.GetByIdAsync(request.Data.TransactionGroupId.Value);
                    if (transactionGroup == null)
                        throw new NotFoundException(_msg.NotFound("TransactionGroup", request.Data.TransactionGroupId.Value));
                }

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    shortEngName: request.Data.ShortEngName,
                    shortArbName: request.Data.ShortArbName,
                    shortArbName4S: request.Data.ShortArbName4S,
                    transactionGroupId: request.Data.TransactionGroupId,
                    sign: request.Data.Sign,
                    debitAccountCode: request.Data.DebitAccountCode,
                    creditAccountCode: request.Data.CreditAccountCode,
                    isPaid: request.Data.IsPaid,
                    formula: request.Data.Formula,
                    beginContractFormula: request.Data.BeginContractFormula,
                    endContractFormula: request.Data.EndContractFormula,
                    inputIsNumeric: request.Data.InputIsNumeric,
                    isEndOfService: request.Data.IsEndOfService,
                    isSalaryEOSExeclude: request.Data.IsSalaryEOSExeclude,
                    isProjectRelatedItem: request.Data.IsProjectRelatedItem,
                    isBasicSalary: request.Data.IsBasicSalary,
                    isDistributable: request.Data.IsDistributable,
                    isAllowPosting: request.Data.IsAllowPosting,
                    remarks: request.Data.Remarks,
                    hasInsuranceTiers: request.Data.HasInsuranceTiers
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}