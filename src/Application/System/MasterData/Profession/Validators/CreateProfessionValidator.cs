using Application.Common.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Profession.Validators
{
    public class CreateProfessionValidator : AbstractValidator<CreateProfessionDto>
    {
        public CreateProfessionValidator(ILocalizationService localizer, IContextService contextService)
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

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}