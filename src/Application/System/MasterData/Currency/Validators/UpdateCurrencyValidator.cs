using Application.System.MasterData.Currency.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Currency.Validators
{
    public class UpdateCurrencyValidator : AbstractValidator<UpdateCurrencyDto>
    {
        public UpdateCurrencyValidator(ILocalizationService localizer, int lang = 1)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).When(x => x.DecimalFraction.HasValue)
                .WithMessage(localizer.GetMessage("DecimalFractionRequired", lang));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateCurrencyDto dto)
        {
            return dto.EngName != null || dto.ArbName != null || dto.ArbName4S != null ||
                   dto.EngSymbol != null || dto.ArbSymbol != null || dto.DecimalFraction.HasValue ||
                   dto.DecimalEngName != null || dto.DecimalArbName != null || dto.Amount.HasValue ||
                   dto.NoDecimalPlaces.HasValue || dto.Remarks != null;
        }
    }
}