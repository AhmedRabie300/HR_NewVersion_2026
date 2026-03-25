using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.DocumentTypesGroup.Validators
{
    public class UpdateDocumentTypesGroupValidator : AbstractValidator<UpdateDocumentTypesGroupDto>
    {
        public UpdateDocumentTypesGroupValidator(ILocalizationService localizer, int lang)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateDocumentTypesGroupDto dto)
        {
            return dto.EngName != null || dto.ArbName != null || dto.Remarks != null;
        }
    }
}