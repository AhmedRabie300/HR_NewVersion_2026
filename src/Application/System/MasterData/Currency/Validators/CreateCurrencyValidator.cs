using Application.Common.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Currency.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyDto>
    {
        public CreateCurrencyValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.EngSymbol)
                .MaximumLength(10).WithMessage(msg.Format("MaxLength", 10));

            RuleFor(x => x.ArbSymbol)
                .MaximumLength(10).WithMessage(msg.Format("MaxLength", 10));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).WithMessage(msg.Get("DecimalFractionRequired"));

            RuleFor(x => x.DecimalEngName)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.DecimalArbName)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.NoDecimalPlaces)
                .InclusiveBetween(0, 4).When(x => x.NoDecimalPlaces.HasValue)
                .WithMessage(msg.Get("DecimalPlacesRange"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}