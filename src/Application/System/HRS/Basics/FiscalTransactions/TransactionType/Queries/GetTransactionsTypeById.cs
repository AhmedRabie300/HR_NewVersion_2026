using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Queries
{
    public static class GetTransactionsTypeById
    {
        public record Query(int Id) : IRequest<TransactionsTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;

          
            }
        }

        public class Handler : IRequestHandler<Query, TransactionsTypeDto>
        {
            private readonly ITransactionsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(ITransactionsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<TransactionsTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
 

                return new TransactionsTypeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    ShortEngName: entity.ShortEngName,
                    ShortArbName: entity.ShortArbName,
                    ShortArbName4S: entity.ShortArbName4S,
                    TransactionGroupId: entity.TransactionGroupId,
                    TransactionGroupName: lang == 2 ? entity.TransactionGroup?.ArbName : entity.TransactionGroup?.EngName,
                    Sign: entity.Sign,
                    DebitAccountCode: entity.DebitAccountCode,
                    CreditAccountCode: entity.CreditAccountCode,
                    IsPaid: entity.IsPaid,
                    Formula: entity.Formula,
                    BeginContractFormula: entity.BeginContractFormula,
                    EndContractFormula: entity.EndContractFormula,
                    InputIsNumeric: entity.InputIsNumeric,
                    IsEndOfService: entity.IsEndOfService,
                    IsSalaryEOSExeclude: entity.IsSalaryEOSExeclude,
                    IsProjectRelatedItem: entity.IsProjectRelatedItem,
                    IsBasicSalary: entity.IsBasicSalary,
                    IsDistributable: entity.IsDistributable,
                    IsAllowPosting: entity.IsAllowPosting,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegUserId: entity.RegUserId,
                    RegComputerId: entity.RegComputerId,
                    HasInsuranceTiers: entity.HasInsuranceTiers,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}