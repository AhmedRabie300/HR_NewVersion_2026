using Application.Common.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Position.Validators
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionDto>
    {
        public CreatePositionValidator(ILocalizationService localizer, IContextService contextService)
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

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage(localizer.GetMessage("ParentPositionRequired", lang));

            RuleFor(x => x.PositionLevelId)
                .GreaterThan(0).When(x => x.PositionLevelId.HasValue)
                .WithMessage(localizer.GetMessage("PositionLevelRequired", lang));

            RuleFor(x => x.EmployeesNo)
                .GreaterThan(0).When(x => x.EmployeesNo.HasValue)
                .WithMessage(localizer.GetMessage("EmployeesNoPositive", lang));

        
            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}