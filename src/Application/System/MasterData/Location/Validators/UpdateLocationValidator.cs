// Application/System/MasterData/Location/Validators/UpdateLocationValidator.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Location.Validators
{
    public class UpdateLocationValidator : AbstractValidator<UpdateLocationDto>
    {
        private readonly ILocalizationService _localization;
        private readonly int _lang;

        public UpdateLocationValidator(ILocalizationService localization, int lang = 1)
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

            RuleFor(x => x.CostCenterCode1)
                .MaximumLength(50).When(x => x.CostCenterCode1 != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).When(x => x.CostCenterCode2 != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).When(x => x.CostCenterCode3 != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).When(x => x.CostCenterCode4 != null)
                .WithMessage(string.Format(_localization.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localization.GetMessage("AtLeastOneField", _lang));
        }

        private bool HaveAtLeastOneField(UpdateLocationDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.CityId.HasValue ||
                   dto.BranchId.HasValue ||
                   dto.StoreId.HasValue ||
                   dto.InventoryCostLedgerId.HasValue ||
                   dto.InventoryAdjustmentLedgerId.HasValue ||
                   dto.DepartmentId.HasValue ||
                   dto.Remarks != null ||
                   dto.CostCenterCode1 != null ||
                   dto.CostCenterCode2 != null ||
                   dto.CostCenterCode3 != null ||
                   dto.CostCenterCode4 != null;
        }
    }
}