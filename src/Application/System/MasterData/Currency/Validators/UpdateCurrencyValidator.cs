using Application.System.MasterData.Currency.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Currency.Validators
{
    public class UpdateCurrencyValidator : AbstractValidator<UpdateCurrencyDto>
    {
        public UpdateCurrencyValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.EngSymbol)
                .MaximumLength(10).When(x => x.EngSymbol != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.ArbSymbol)
                .MaximumLength(10).When(x => x.ArbSymbol != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).When(x => x.DecimalFraction.HasValue)
                .WithMessage(x => msg.Get("DecimalFractionRequired"));

            RuleFor(x => x.DecimalEngName)
                .MaximumLength(100).When(x => x.DecimalEngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.DecimalArbName)
                .MaximumLength(100).When(x => x.DecimalArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Amount)
                .GreaterThan(0).When(x => x.Amount.HasValue)
                .WithMessage(x => msg.Get("AmountMustBePositive"));

            RuleFor(x => x.NoDecimalPlaces)
                .GreaterThan(0).When(x => x.NoDecimalPlaces.HasValue)
                .WithMessage(x => msg.Get("NoDecimalPlacesMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateCurrencyDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.EngSymbol != null ||
                   dto.ArbSymbol != null ||
                   dto.DecimalFraction.HasValue ||
                   dto.DecimalEngName != null ||
                   dto.DecimalArbName != null ||
                   dto.Amount.HasValue ||
                   dto.NoDecimalPlaces.HasValue ||
                   dto.Remarks != null;
        }
    }
}