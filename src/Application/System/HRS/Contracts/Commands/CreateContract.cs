using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using Application.System.HRS.Contracts.Validators;
using Application.System.MasterData.Abstractions;
using Domain.System.HRS.Basics.Contracts;
using FluentValidation;
using MediatR;
using Application.System.MasterData.Abstractions;

namespace Application.System.HRS.Contracts.Commands
{
    public static class CreateContract
    {
        public record Command(CreateContractDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(
                IValidationMessages msg,
                IContractRepository repo,
                IEmployeeRepository employeeRepo,
                IEmployeeClassRepository employeeClassRepo,
                IContractTypeRepository contractTypeRepo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateContractValidator(msg, repo, employeeRepo, employeeClassRepo, contractTypeRepo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly ICompanyRepository _companyRepo;

            public Handler(IContractRepository repo, ICurrentUser currentUser, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _currentUser = currentUser;
                _companyRepo = companyRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;
                var regUserId = _currentUser.UserId ?? 0;

                // Get contract number
                int contractNumber;
                if (request.Data.Number.HasValue && request.Data.Number.Value > 0)
                {
                    contractNumber = request.Data.Number.Value;
                }
                else
                {
                    contractNumber = await _repo.GetNextNumberAsync(companyId, cancellationToken);
                }

                var entity = new Domain.System.HRS.Basics.Contracts.Contract(
                    number: contractNumber,
                    contractTypeId: request.Data.ContractTypeId,
                    employeeClassId: request.Data.EmployeeClassId,
                    employeeId: request.Data.EmployeeId,
                    startDate: request.Data.StartDate,
                     professionId: request.Data.ProfessionId,
                    positionId: request.Data.PositionId,
                    gradeStepId: request.Data.GradeStepId,
                    currencyId: request.Data.CurrencyId,
                    endDate: request.Data.EndDate,
                    remarks: request.Data.Remarks,
                     contractPeriod: request.Data.ContractPeriod
                );

                // Add transactions if provided
                if (request.Data.Transactions != null)
                {
                    foreach (var transDto in request.Data.Transactions)
                    {
                        var transaction = new ContractTransaction(
                            contractId: 0, // Will be set after entity is added
                            transactionTypeId: transDto.TransactionTypeId,
                             amount: transDto.Amount,
                            active: transDto.Active,
                            intervalId: transDto.IntervalId,
                            paidAtVacation: transDto.PaidAtVacation,
                            onceAtPeriod: transDto.OnceAtPeriod,
                            remarks: transDto.Remarks,
                             activeDate: transDto.ActiveDate,
                            activeDateD: transDto.ActiveDateD
                        );
                        entity.AddTransaction(transaction);
                    }
                }

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}