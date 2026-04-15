using Application.Common.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using FluentValidation;

namespace Application.System.MasterData.MaritalStatus.Validators
{
    public class UpdateMaritalStatusValidator : AbstractValidator<UpdateMaritalStatusDto>
    {
        public UpdateMaritalStatusValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

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

        private bool HaveAtLeastOneField(UpdateMaritalStatusDto dto)
        {
            return  
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Remarks != null;
        }
    }
}