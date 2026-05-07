using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using MediatR;

namespace Application.System.HRS.Contracts.Queries
{
    public static class GetValidContractsWithTransactions
    {
        public record Query(int? EmployeeId = null) : IRequest<List<ContractWithTransactionsDto>>;

        public class Handler : IRequestHandler<Query, List<ContractWithTransactionsDto>>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<ContractWithTransactionsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;
                var now = DateTime.Now;

                // Get all active contracts for the company
                var allContracts = await _repo.GetByCompanyIdAsync();

                // Filter valid contracts (active and not expired)
                var validContracts = allContracts
                    .Where(x => x.IsActive() && (!x.EndDate.HasValue || x.EndDate.Value >= now));

                // Filter by employee if specified
                if (request.EmployeeId.HasValue && request.EmployeeId.Value > 0)
                {
                    validContracts = validContracts.Where(x => x.EmployeeId == request.EmployeeId.Value);
                }

                return validContracts.Select(x => new ContractWithTransactionsDto(
                    Id: x.Id,
                    Number: x.Number,
                    EmployeeId: x.EmployeeId,
                    EmployeeName: lang == 2 ? x.Employee?.ArbName : x.Employee?.EngName,
                    ContractTypeId: x.ContractTypeId,
                    ContractTypeName: lang == 2 ? x.ContractType?.ArbName : x.ContractType?.EngName,
                    EmployeeClassId: x.EmployeeClassId,
                    EmployeeClassName: lang == 2 ? x.EmployeeClass?.ArbName : x.EmployeeClass?.EngName,
                    StartDate: x.StartDate,
                    EndDate: x.EndDate,
                    ProfessionId: x.ProfessionId,
                    ProfessionName: lang == 2 ? x.Profession?.ArbName : x.Profession?.EngName,
                    PositionId: x.PositionId,
                    PositionName: lang == 2 ? x.Position?.ArbName : x.Position?.EngName,
                    GradeStepId: x.GradeStepId,
                    GradeStepName: lang == 2 ? x.GradeStep?.ArbName : x.GradeStep?.EngName,
                    CurrencyId: x.CurrencyId,
                    CurrencyName: lang == 2 ? x.Currency?.ArbName : x.Currency?.EngName,
                    ContractPeriod: x.ContractPeriod,
                    Remarks: x.Remarks,
                    IsCurrent: x.IsCurrent,
                    IsActive: x.IsActive(),
                    Transactions: x.Transactions
                        .Where(t => t.IsActive())  // Only active transactions (CancelDate IS NULL)
                        .Select(t => new ContractTransactionDto(
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
                        )).ToList()
                )).ToList();
            }
        }
    }
}