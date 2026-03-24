using Application.System.MasterData.Profession.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Profession.Validators
{
    public class UpdateProfessionValidator : AbstractValidator<UpdateProfessionDto>
    {
        public UpdateProfessionValidator(ILocalizationService localizer, int lang = 1)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateProfessionDto dto)
        {
            return dto.EngName != null || dto.ArbName != null || dto.ArbName4S != null || dto.Remarks != null;
        }
    }
}