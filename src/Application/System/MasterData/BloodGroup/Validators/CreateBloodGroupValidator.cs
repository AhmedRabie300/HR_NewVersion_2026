using Application.Common.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using FluentValidation;

namespace Application.System.MasterData.BloodGroup.Validators
{
    public class CreateBloodGroupValidator : AbstractValidator<CreateBloodGroupDto>
    {
        public CreateBloodGroupValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

             RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(localizer.GetMessage("EngNameRequired", lang))
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(localizer.GetMessage("ArbNameRequired", lang))
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}