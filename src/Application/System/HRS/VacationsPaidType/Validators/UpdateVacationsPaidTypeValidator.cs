using Application.Common.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationsPaidType.Validators
{
    public class UpdateVacationsPaidTypeValidator : AbstractValidator<UpdateVacationsPaidTypeDto>
    {
        public UpdateVacationsPaidTypeValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.Code)
                .MaximumLength(50).When(x => x.Code != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(50).When(x => x.EngName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).When(x => x.ArbName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateVacationsPaidTypeDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null;
        }
    }
}