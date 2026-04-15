using Application.Common.Abstractions;
using Application.System.HRS.Gender.Dtos;
using FluentValidation;

namespace Application.System.HRS.Gender.Validators
{
    public class CreateGenderValidator : AbstractValidator<CreateGenderDto>
    {
        public CreateGenderValidator(ILocalizationService localizer, IContextService contextService)
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