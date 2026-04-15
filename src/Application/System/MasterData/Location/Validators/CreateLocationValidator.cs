using Application.Common.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Location.Validators
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
    {
        public CreateLocationValidator(ILocalizationService localizer, IContextService contextService)
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

            RuleFor(x => x.CityId)
                .GreaterThan(0).When(x => x.CityId.HasValue)
                .WithMessage(localizer.GetMessage("CityRequired", lang));

            RuleFor(x => x.BranchId)
                .GreaterThan(0).When(x => x.BranchId.HasValue)
                .WithMessage(localizer.GetMessage("BranchRequired", lang));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).When(x => x.DepartmentId.HasValue)
                .WithMessage(localizer.GetMessage("DepartmentRequired", lang));

            RuleFor(x => x.CostCenterCode1)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}