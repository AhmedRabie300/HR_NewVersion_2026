using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Validators
{
    public class UpdateEndOfServiceRuleValidator : AbstractValidator<UpdateEndOfServiceRuleDto>
    {
        public UpdateEndOfServiceRuleValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EndOfServiceId)
                .GreaterThan(0).WithMessage(x => msg.Get("EndOfServiceIdRequired"));

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
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateEndOfServiceRuleDto dto)
        {
            return dto.FromWorkingMonths.HasValue ||
                   dto.ToWorkingMonths.HasValue ||
                   dto.AmountPercent.HasValue ||
                   !string.IsNullOrWhiteSpace(dto.Formula) ||
                   !string.IsNullOrWhiteSpace(dto.ExtraDedFormula) ||
                   !string.IsNullOrWhiteSpace(dto.Remarks);
        }
    }
}