using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Validators
{
    public class CreateIntervalValidator : AbstractValidator<CreateIntervalDto>
    {
        private readonly IIntervalRepository _repo;

        public CreateIntervalValidator(IValidationMessages msg, IIntervalRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Interval"), x.Code));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage(x => msg.Get("NumberMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}