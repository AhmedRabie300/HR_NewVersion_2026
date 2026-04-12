using Application.System.MasterData.Currency.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Currency.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public CreateCurrencyValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _ContextService.GetCurrentLanguage()))
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.EngSymbol)
                .MaximumLength(10).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 10));

            RuleFor(x => x.ArbSymbol)
                .MaximumLength(10).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 10));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).WithMessage(x => _localizer.GetMessage("DecimalFractionRequired", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.DecimalEngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.DecimalArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.Amount)
                .GreaterThan(0).When(x => x.Amount.HasValue)
                .WithMessage(x => _localizer.GetMessage("AmountMustBePositive", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.NoDecimalPlaces)
                .GreaterThan(0).When(x => x.NoDecimalPlaces.HasValue)
                .WithMessage(x => _localizer.GetMessage("NoDecimalPlacesMustBePositive", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 2048));
        }
    }
}