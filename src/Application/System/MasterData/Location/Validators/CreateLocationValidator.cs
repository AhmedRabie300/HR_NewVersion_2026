using Application.Common.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Location.Validators
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
    {
        private readonly ILocalizationService _localization;
        private readonly int _lang;

        public CreateLocationValidator(ILocalizationService localization, int lang = 1)
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

            RuleFor(x => x.CostCenterCode1)
                .MaximumLength(50).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            // At least one identifier should be provided
            RuleFor(x => x)
                .Must(x => x.CompanyId.HasValue || x.BranchId.HasValue)
                .WithMessage(_localization.GetMessage("AtLeastOneIdentifier", _lang));
        }
    }
}