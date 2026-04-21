using Application.Abstractions;
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
    public static class CreateTransactionsType
    {
        public record Command(CreateTransactionsTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(
                IValidationMessages msg,
                ITransactionsTypeRepository repo,
                ITransactionsGroupRepository transactionsGroupRepo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateTransactionsTypeValidator(msg, repo, transactionsGroupRepo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ITransactionsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly ITransactionsGroupRepository _transactionsGroupRepo;

            public Handler(
                ITransactionsTypeRepository repo,
                ICurrentUser currentUser,
                ITransactionsGroupRepository transactionsGroupRepo)
            {
                _repo = repo;
                _currentUser = currentUser;
                _transactionsGroupRepo = transactionsGroupRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;
                var regUserId = _currentUser.UserId ?? 0;

                 var transactionGroup = await _transactionsGroupRepo.GetByIdAsync(request.Data.TransactionGroupId);
               
                var entity = new TransactionsType(
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
                    companyId: companyId,
                    regUserId: regUserId,
                    regComputerId: request.Data.RegComputerId,
                    remarks: request.Data.Remarks,
                    hasInsuranceTiers: request.Data.HasInsuranceTiers
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}