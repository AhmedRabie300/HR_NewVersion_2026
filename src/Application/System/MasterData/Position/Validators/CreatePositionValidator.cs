using Application.Common.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Position.Validators
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public CreatePositionValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _ContextService.GetCurrentLanguage()))
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.PositionLevelId)
                .GreaterThan(0).When(x => x.PositionLevelId.HasValue)
                .WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.EmployeesNo)
                .GreaterThan(0).When(x => x.EmployeesNo.HasValue)
                .WithMessage(x => _localizer.GetMessage("EmployeesNoMustBePositive", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.PositionBudget)
                .MaximumLength(5).When(x => x.PositionBudget != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 5));
        }
    }
}