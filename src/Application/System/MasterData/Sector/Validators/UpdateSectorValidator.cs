// Application/System/MasterData/Sector/Validators/UpdateSectorValidator.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Sector.Validators
{
    public class UpdateSectorValidator : AbstractValidator<UpdateSectorDto>
    {
        private readonly ILocalizationService _localization;
        private readonly int _lang;

        public UpdateSectorValidator(ILocalizationService localization, int lang = 1)
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

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localization.GetMessage("AtLeastOneField", _lang));
        }

        private bool HaveAtLeastOneField(UpdateSectorDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.ParentId.HasValue ||
                   dto.Remarks != null;
        }
    }
}