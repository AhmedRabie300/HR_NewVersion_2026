using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Validators
{
    public class CreateGradeValidator : AbstractValidator<CreateGradeDto>
    {
        private readonly IGradeRepository _repo;

        public CreateGradeValidator(IValidationMessages msg, IGradeRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Grade"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FromSalary)
                .GreaterThanOrEqualTo(0).When(x => x.FromSalary.HasValue)
                .WithMessage(x => msg.Get("FromSalaryMustBePositive"));

            RuleFor(x => x.ToSalary)
                .GreaterThanOrEqualTo(0).When(x => x.ToSalary.HasValue)
                .WithMessage(x => msg.Get("ToSalaryMustBePositive"));

            RuleFor(x => x.RegularHours)
                .GreaterThanOrEqualTo(0).When(x => x.RegularHours.HasValue)
                .WithMessage(x => msg.Get("RegularHoursMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));

            RuleFor(x => x.Transactions)
                .NotNull().WithMessage(x => msg.Get("TransactionsRequired"));
        }
    }

    public class CreateGradeTransactionValidator : AbstractValidator<CreateGradeTransactionDto>
    {
        private readonly IGradeRepository _repo;

        public CreateGradeTransactionValidator(IValidationMessages msg, IGradeRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.TransactionTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("TransactionTypeRequired"));

            RuleFor(x => x.MinValue)
                .GreaterThanOrEqualTo(0).When(x => x.MinValue.HasValue)
                .WithMessage(x => msg.Get("MinValueMustBePositive"));

            RuleFor(x => x.MaxValue)
                .GreaterThanOrEqualTo(0).When(x => x.MaxValue.HasValue)
                .WithMessage(x => msg.Get("MaxValueMustBePositive"));

            RuleFor(x => x.PaidAtVacation)
                .GreaterThanOrEqualTo(0).When(x => x.PaidAtVacation.HasValue)
                .WithMessage(x => msg.Get("PaidAtVacationMustBePositive"));

            RuleFor(x => x.NumberOfTickets)
                .GreaterThanOrEqualTo(0).When(x => x.NumberOfTickets.HasValue)
                .WithMessage(x => msg.Get("NumberOfTicketsMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}