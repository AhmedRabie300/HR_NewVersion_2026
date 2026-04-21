using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Queries
{
    public static class GetPagedTransactionsTypes
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null
        ) : IRequest<PagedResult<TransactionsTypeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<TransactionsTypeDto>>
        {
            private readonly ITransactionsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(ITransactionsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<TransactionsTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(x => new TransactionsTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    ShortEngName: x.ShortEngName,
                    ShortArbName: x.ShortArbName,
                    ShortArbName4S: x.ShortArbName4S,
                    TransactionGroupId: x.TransactionGroupId,
                    TransactionGroupName: lang == 2 ? x.TransactionGroup?.ArbName : x.TransactionGroup?.EngName,
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
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegUserId: x.RegUserId,
                    RegComputerId: x.RegComputerId,
                    HasInsuranceTiers: x.HasInsuranceTiers,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<TransactionsTypeDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}