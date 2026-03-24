using Application.System.MasterData.BloodGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.BloodGroup.Validators
{
    public class UpdateBloodGroupValidator : AbstractValidator<UpdateBloodGroupDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly int _lang;

        public UpdateBloodGroupValidator(ILocalizationService localizer, int lang = 1)
        {
            _localizer = localizer;
            _lang = lang;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", _lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localizer.GetMessage("AtLeastOneField", _lang));
        }

        private bool HaveAtLeastOneField(UpdateBloodGroupDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Remarks != null;
        }
    }
}