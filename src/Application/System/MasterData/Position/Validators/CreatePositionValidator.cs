using Application.Common.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Position.Validators
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionDto>
    {
        private readonly ILocalizationService _localization;
        private readonly int _lang;

        public CreatePositionValidator(ILocalizationService localization, int lang = 1)
        {
            _localization = localization;
            _lang = lang;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localization.GetMessage("CodeRequired", _lang))
                .MaximumLength(50).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 2048));

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage(_localization.GetMessage("IdGreaterThanZero", _lang));

            RuleFor(x => x.PositionLevelId)
                .GreaterThan(0).When(x => x.PositionLevelId.HasValue)
                .WithMessage(_localization.GetMessage("IdGreaterThanZero", _lang));

            RuleFor(x => x.EmployeesNo)
                .GreaterThan(0).When(x => x.EmployeesNo.HasValue)
                .WithMessage("Number of employees must be greater than 0");

            RuleFor(x => x.PositionBudget)
                .MaximumLength(5).WithMessage("Position budget must not exceed 5 characters");
        }
    }
}