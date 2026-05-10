using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using FluentValidation;
using Application.System.MasterData.Abstractions;
namespace Application.System.HRS.Contracts.Validators
{
    public class CreateContractValidator : AbstractValidator<CreateContractDto>
    {
        private readonly IContractRepository _repo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IEmployeeClassRepository _employeeClassRepo;
        private readonly IContractTypeRepository _contractTypeRepo;

        public CreateContractValidator(
            IValidationMessages msg,
            IContractRepository repo,
            IEmployeeRepository employeeRepo,
            IEmployeeClassRepository employeeClassRepo,
            IContractTypeRepository contractTypeRepo)
        {
            _repo = repo;
            _employeeRepo = employeeRepo;
            _employeeClassRepo = employeeClassRepo;
            _contractTypeRepo = contractTypeRepo;

            // Contract Number (optional - will be generated)
            RuleFor(x => x.Number)
                .GreaterThan(0).When(x => x.Number.HasValue)
                .WithMessage(x => msg.Get("NumberMustBePositive"))
                .MustAsync(async (dto, number, cancellation) =>
                {
                    if (!number.HasValue) return true;
                    return !await _repo.NumberExistsAsync(number.Value);
                })
                .When(x => x.Number.HasValue)
                .WithMessage(x => msg.Format("NumberExists", x.Number));

            // Contract Type
            RuleFor(x => x.ContractTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("ContractTypeRequired"))
                .MustAsync(async (id, cancellation) => await _contractTypeRepo.ExistsAsync(id))
                .WithMessage(x => msg.Format("NotFound", msg.Get("ContractType"), x.ContractTypeId));

            // Employee Class
            RuleFor(x => x.EmployeeClassId)
                .GreaterThan(0).WithMessage(x => msg.Get("EmployeeClassRequired"))
                .MustAsync(async (id, cancellation) => await _employeeClassRepo.ExistsAsync(id))
                .WithMessage(x => msg.Format("NotFound", msg.Get("EmployeeClass"), x.EmployeeClassId));

            // Employee
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage(x => msg.Get("EmployeeRequired"))
                .MustAsync(async (id, cancellation) => await _employeeRepo.ExistsAsync(id))
                .WithMessage(x => msg.Format("NotFound", msg.Get("Employee"), x.EmployeeId));

            // Dates
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(x => msg.Get("StartDateRequired"));

            RuleFor(x => x)
                .Must(x => !x.EndDate.HasValue || x.StartDate <= x.EndDate.Value)
                .WithMessage(x => msg.Get("StartDateLessThanEndDate"));

            // Contract Period
            RuleFor(x => x.ContractPeriod)
                .GreaterThan(0).When(x => x.ContractPeriod.HasValue)
                .WithMessage(x => msg.Get("ContractPeriodPositive"));

            // Remarks
            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            // Transactions validation (if provided)
            RuleForEach(x => x.Transactions)
                .SetValidator(new CreateContractTransactionValidator(msg));
        }
    }

    public class CreateContractTransactionValidator : AbstractValidator<CreateContractTransactionDto>
    {
        public CreateContractTransactionValidator(IValidationMessages msg)
        {
            RuleFor(x => x.TransactionTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("TransactionTypeRequired"));

            RuleFor(x => x.Amount)
                .GreaterThan(0).When(x => x.Amount.HasValue)
                .WithMessage(x => msg.Get("AmountMustBePositive"));

            RuleFor(x => x.PaidAtVacation)
                .InclusiveBetween((byte)0, (byte)100).When(x => x.PaidAtVacation.HasValue)
                .WithMessage(x => msg.Get("PaidAtVacationRange"));

            RuleFor(x => x.ActiveDateD)
                .MaximumLength(3).WithMessage(x => msg.Format("MaxLength", 3));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}