using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Queries
{
    public static class ListTransactionsTypes
    {
        public record Query : IRequest<List<TransactionsTypeDto>>;

        public class Handler : IRequestHandler<Query, List<TransactionsTypeDto>>
        {
            private readonly ITransactionsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(ITransactionsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<TransactionsTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var items = await _repo.GetByCompanyIdAsync(companyId);

                return items.Select(x => new TransactionsTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    ShortEngName: x.ShortEngName,
                    ShortArbName: x.ShortArbName,
                    ShortArbName4S: x.ShortArbName4S,
                    TransactionGroupId: x.TransactionGroupId,
                    Sign: x.Sign,
                    DebitAccountCode: x.DebitAccountCode,
                    CreditAccountCode: x.CreditAccountCode,
                    IsPaid: x.IsPaid,
                    Formula: x.Formula,
                    BeginContractFormula: x.BeginContractFormula,
                    EndContractFormula: x.EndContractFormula,
                    InputIsNumeric: x.InputIsNumeric,
                    IsEndOfService: x.IsEndOfService,
                    IsSalaryEOSExeclude: x.IsSalaryEOSExeclude,
                    IsProjectRelatedItem: x.IsProjectRelatedItem,
                    IsBasicSalary: x.IsBasicSalary,
                    IsDistributable: x.IsDistributable,
                    IsAllowPosting: x.IsAllowPosting,
                    CompanyId: x.CompanyId,
                    Remarks: x.Remarks,
                    RegUserId: x.RegUserId,
                    RegComputerId: x.RegComputerId,
                    HasInsuranceTiers: x.HasInsuranceTiers,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}