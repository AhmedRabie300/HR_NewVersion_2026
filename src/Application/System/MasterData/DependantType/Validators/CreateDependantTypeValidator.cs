using Application.System.MasterData.DependantType.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.DependantType.Validators
{
    public class CreateDependantTypeValidator : AbstractValidator<CreateDependantTypeDto>
    {
        public CreateDependantTypeValidator(ILocalizationService localizer, int lang = 1)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage(localizer.GetMessage("CompanyRequired", lang));

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