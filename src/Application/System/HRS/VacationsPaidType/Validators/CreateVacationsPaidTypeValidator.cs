using Application.Common.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationsPaidType.Validators
{
    public class CreateVacationsPaidTypeValidator : AbstractValidator<CreateVacationsPaidTypeDto>
    {
        public CreateVacationsPaidTypeValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));
        }
    }
}