using Application.Common.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Position.Validators
{
    public class UpdatePositionValidator : AbstractValidator<UpdatePositionDto>
    {
        private readonly ILocalizationService _localization;
        private readonly int _lang;

        public UpdatePositionValidator(ILocalizationService localization, int lang = 1)
        {
            _localization = localization;
            _lang = lang;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_localization.GetMessage("IdGreaterThanZero", _lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 2048));

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
                .MaximumLength(5).When(x => x.PositionBudget != null)
                .WithMessage("Position budget must not exceed 5 characters");

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localization.GetMessage("AtLeastOneField", _lang));
        }

        private bool HaveAtLeastOneField(UpdatePositionDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.ParentId.HasValue ||
                   dto.PositionLevelId.HasValue ||
                   dto.EvalEvaluationId.HasValue ||
                   dto.EvalRecruitmentId.HasValue ||
                   dto.Remarks != null ||
                   dto.EmployeesNo.HasValue ||
                   dto.ApplyValidation.HasValue ||
                   dto.PositionBudget != null ||
                   dto.AppraisalTypeGroupId.HasValue;
        }
    }
}