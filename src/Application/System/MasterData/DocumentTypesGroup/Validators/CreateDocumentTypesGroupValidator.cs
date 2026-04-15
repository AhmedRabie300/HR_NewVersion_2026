using Application.Common.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using FluentValidation;

namespace Application.System.MasterData.DocumentTypesGroup.Validators
{
    public class CreateDocumentTypesGroupValidator : AbstractValidator<CreateDocumentTypesGroupDto>
    {
        public CreateDocumentTypesGroupValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            // ✅ إضافة التحقق من Code
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}