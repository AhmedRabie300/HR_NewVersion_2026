using Application.Common.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationsType.Validators
{
    public class CreateVacationsTypeValidator : AbstractValidator<CreateVacationsTypeDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public CreateVacationsTypeValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Sex)
                .MaximumLength(1).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 1));
                
            RuleFor(x => x.Religion)
                .MaximumLength(10).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.TimesNoInYear)
                .GreaterThan(0).When(x => x.TimesNoInYear.HasValue)
                .WithMessage(_localizer.GetMessage("TimesNoInYearGreaterThanZero", lang));

            RuleFor(x => x.AllowedDaysNo)
                .GreaterThan(0).When(x => x.AllowedDaysNo.HasValue)
                .WithMessage(_localizer.GetMessage("AllowedDaysNoGreaterThanZero", lang));

            RuleFor(x => x.Stage1Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage1Pct.HasValue)
                .WithMessage(_localizer.GetMessage("PercentageBetween0And100", lang));

            RuleFor(x => x.Stage2Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage2Pct.HasValue)
                .WithMessage(_localizer.GetMessage("PercentageBetween0And100", lang));

            RuleFor(x => x.Stage3Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage3Pct.HasValue)
                .WithMessage(_localizer.GetMessage("PercentageBetween0And100", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}