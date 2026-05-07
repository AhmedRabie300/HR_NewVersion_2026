using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Contracts.Queries
{
    public static class GetContractByNumber
    {
        public record Query(int Number) : IRequest<ContractDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Number)
                    .GreaterThan(0).WithMessage("Contract number must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, ContractDto>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<ContractDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByNumberAsync(request.Number);
              
                return new ContractDto(
                    Id: entity.Id,
                    Number: entity.Number,
                    ContractTypeId: entity.ContractTypeId,
                    ContractTypeName: lang == 2 ? entity.ContractType?.ArbName : entity.ContractType?.EngName,
                    EmployeeClassId: entity.EmployeeClassId,
                    EmployeeClassName: lang == 2 ? entity.EmployeeClass?.ArbName : entity.EmployeeClass?.EngName,
                    EmployeeId: entity.EmployeeId,
                    EmployeeName: lang == 2 ? entity.Employee?.ArbName : entity.Employee?.EngName,
                    StartDate: entity.StartDate,
                    EndDate: entity.EndDate,
                    ProfessionId: entity.ProfessionId,
                    ProfessionName: lang == 2 ? entity.Profession?.ArbName : entity.Profession?.EngName,
                    PositionId: entity.PositionId,
                    PositionName: lang == 2 ? entity.Position?.ArbName : entity.Position?.EngName,
                    GradeStepId: entity.GradeStepId,
                    GradeStepName: lang == 2 ? entity.GradeStep?.ArbName : entity.GradeStep?.EngName,
                    CurrencyId: entity.CurrencyId,
                    CurrencyName: lang == 2 ? entity.Currency?.ArbName : entity.Currency?.EngName,
                    Remarks: entity.Remarks,
                    ContractPeriod: entity.ContractPeriod,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive(),
                    IsCurrent: entity.IsCurrent,
                    Transactions: entity.Transactions.Select(t => new ContractTransactionDto(
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
                );
            }
        }
    }
}