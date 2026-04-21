using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Validators
{
    public class CreateEndOfServiceValidator : AbstractValidator<CreateEndOfServiceDto>
    {
        private readonly IEndOfServiceRepository _repo;

        public CreateEndOfServiceValidator(IValidationMessages msg, IEndOfServiceRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("EndOfService"), x.Code));

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

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.Rules)
                .NotNull().WithMessage(x => msg.Get("RulesRequired"))
                .Must(rules => rules != null && rules.Any())
                .WithMessage(x => msg.Get("AtLeastOneRuleRequired"));

            RuleForEach(x => x.Rules)
                .SetValidator(new CreateEndOfServiceRuleValidator(msg));
        }
    }

    public class CreateEndOfServiceRuleValidator : AbstractValidator<CreateEndOfServiceRuleDto>
    {
        public CreateEndOfServiceRuleValidator(IValidationMessages msg)
        {
            RuleFor(x => x.FromWorkingMonths)
                .GreaterThanOrEqualTo(0).When(x => x.FromWorkingMonths.HasValue)
                .WithMessage(x => msg.Get("FromWorkingMonthsMustBePositive"));

            RuleFor(x => x.ToWorkingMonths)
                .GreaterThanOrEqualTo(0).When(x => x.ToWorkingMonths.HasValue)
                .WithMessage(x => msg.Get("ToWorkingMonthsMustBePositive"));

            RuleFor(x => x.AmountPercent)
                .GreaterThanOrEqualTo(0).When(x => x.AmountPercent.HasValue)
                .LessThanOrEqualTo(100).When(x => x.AmountPercent.HasValue)
                .WithMessage(x => msg.Get("AmountPercentRange"));

            RuleFor(x => x.Formula)
                .MaximumLength(512).WithMessage(x => msg.Format("MaxLength", 512));

            RuleFor(x => x.ExtraDedFormula)
                .MaximumLength(512).WithMessage(x => msg.Format("MaxLength", 512));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(x => x.AmountPercent.HasValue || !string.IsNullOrWhiteSpace(x.Formula))
                .WithMessage(x => msg.Get("AmountPercentOrFormulaRequired"));
        }
    }
}