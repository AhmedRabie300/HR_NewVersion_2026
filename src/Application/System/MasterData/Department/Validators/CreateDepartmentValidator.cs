using Application.System.MasterData.Department.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Department.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public CreateDepartmentValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _ContextService.GetCurrentLanguage()))
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 50));

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage(x => _localizer.GetMessage("CompanyRequired", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage(x => _localizer.GetMessage("ParentDepartmentRequired", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.CostCenterCode)
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 50));
        }
    }
}