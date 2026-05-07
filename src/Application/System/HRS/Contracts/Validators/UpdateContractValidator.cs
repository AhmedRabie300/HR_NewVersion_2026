using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using FluentValidation;

namespace Application.System.HRS.Contracts.Validators
{
    public class UpdateContractValidator : AbstractValidator<UpdateContractDto>
    {
        private readonly IContractRepository _repo;

        public UpdateContractValidator(IValidationMessages msg, IContractRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.ContractTypeId)
                .GreaterThan(0).When(x => x.ContractTypeId.HasValue)
                .WithMessage(x => msg.Get("ContractTypeRequired"));

            RuleFor(x => x.EmployeeClassId)
                .GreaterThan(0).When(x => x.EmployeeClassId.HasValue)
                .WithMessage(x => msg.Get("EmployeeClassRequired"));

            RuleFor(x => x.StartDate)
                .NotEmpty().When(x => x.StartDate.HasValue)
                .WithMessage(x => msg.Get("StartDateRequired"));

            RuleFor(x => x)
                .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate.Value <= x.EndDate.Value)
                .WithMessage(x => msg.Get("StartDateLessThanEndDate"));

            RuleFor(x => x.ContractPeriod)
                .GreaterThan(0).When(x => x.ContractPeriod.HasValue)
                .WithMessage(x => msg.Get("ContractPeriodPositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateContractDto dto)
        {
            return dto.ContractTypeId.HasValue ||
                   dto.EmployeeClassId.HasValue ||
                   dto.StartDate.HasValue ||
                   dto.EndDate.HasValue ||
                   dto.ProfessionId.HasValue ||
                   dto.PositionId.HasValue ||
                   dto.GradeStepId.HasValue ||
                   dto.CurrencyId.HasValue ||
                   dto.Remarks != null ||
                   dto.ContractPeriod.HasValue;
        }
    }

    public class UpdateContractTransactionValidator : AbstractValidator<UpdateContractTransactionDto>
    {
        public UpdateContractTransactionValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.ContractId)
                .GreaterThan(0).WithMessage(x => msg.Get("ContractIdRequired"));

            RuleFor(x => x.TransactionTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("TransactionTypeRequired"));

            RuleFor(x => x.Amount)
                .GreaterThan(0).When(x => x.Amount.HasValue)
                .WithMessage(x => msg.Get("AmountMustBePositive"));

            RuleFor(x => x.PaidAtVacation)
                .InclusiveBetween((byte)0, (byte)100).When(x => x.PaidAtVacation.HasValue)
                .WithMessage(x => msg.Get("PaidAtVacationRange"));

            RuleFor(x => x.ActiveDateD)
                .MaximumLength(3).When(x => x.ActiveDateD != null)
                .WithMessage(x => msg.Format("MaxLength", 3));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateContractTransactionDto dto)
        {
            return dto.Amount.HasValue ||
                   dto.Active.HasValue ||
                   dto.IntervalId.HasValue ||
                   dto.PaidAtVacation.HasValue ||
                   dto.OnceAtPeriod.HasValue ||
                   dto.Remarks != null ||
                   dto.ActiveDate.HasValue ||
                   dto.ActiveDateD != null;
        }
    }
}