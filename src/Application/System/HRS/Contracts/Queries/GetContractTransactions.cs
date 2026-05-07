using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using MediatR;

namespace Application.System.HRS.Contracts.Queries
{
    public static class GetContractTransactions
    {
        public record Query(int ContractId) : IRequest<List<ContractTransactionDto>>;

        public class Handler : IRequestHandler<Query, List<ContractTransactionDto>>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<ContractTransactionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var items = await _repo.GetTransactionsByContractIdAsync(request.ContractId);

                return items.Select(t => new ContractTransactionDto(
                    Id: t.Id,
                    ContractId: t.ContractId,
                    TransactionTypeId: t.TransactionTypeId,
                    TransactionTypeName: lang == 2 ? t.TransactionType?.ArbName : t.TransactionType?.EngName,
                    Amount: t.Amount,
                    Active: t.Active,
                    IntervalId: t.IntervalId,
                    IntervalName: lang == 2 ? t.Interval?.ArbName : t.Interval?.EngName,
                    PaidAtVacation: t.PaidAtVacation,
                    OnceAtPeriod: t.OnceAtPeriod,
                    Remarks: t.Remarks,
                    ActiveDate: t.ActiveDate,
                    ActiveDateD: t.ActiveDateD,
                    RegDate: t.RegDate,
                    CancelDate: t.CancelDate,
                    IsActive: t.IsActive()
                )).ToList();
            }
        }
    }
}