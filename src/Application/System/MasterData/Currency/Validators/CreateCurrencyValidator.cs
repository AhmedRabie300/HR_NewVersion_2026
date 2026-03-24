using Application.System.MasterData.Currency.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Currency.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyDto>
    {
        public CreateCurrencyValidator(ILocalizationService localizer, int lang = 1)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).WithMessage(localizer.GetMessage("DecimalFractionRequired", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));
        }
    }
}