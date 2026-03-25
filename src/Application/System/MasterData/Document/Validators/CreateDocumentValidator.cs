using Application.System.MasterData.Document.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Document.Validators
{
    public class CreateDocumentValidator : AbstractValidator<CreateDocumentDto>
    {
        public CreateDocumentValidator(ILocalizationService localizer, int lang)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x.DocumentTypesGroupId)
                .GreaterThan(0).When(x => x.DocumentTypesGroupId.HasValue)
                .WithMessage(localizer.GetMessage("DocumentTypesGroupRequired", lang));
        }
    }
}