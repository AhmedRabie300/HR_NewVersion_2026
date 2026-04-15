using Application.Common.Abstractions;
using Application.System.MasterData.Education.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Education.Validators
{
    public class CreateEducationValidator : AbstractValidator<CreateEducationDto>
    {
        public CreateEducationValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Level)
                .InclusiveBetween(1, 10).When(x => x.Level.HasValue)
                .WithMessage(localizer.GetMessage("EducationLevelRange", lang));

            RuleFor(x => x.RequiredYears)
                .InclusiveBetween(0, 20).When(x => x.RequiredYears.HasValue)
                .WithMessage(localizer.GetMessage("RequiredYearsRange", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}