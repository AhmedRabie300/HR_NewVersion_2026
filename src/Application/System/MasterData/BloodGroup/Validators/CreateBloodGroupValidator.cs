using Application.System.MasterData.BloodGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.BloodGroup.Validators
{
    public class CreateBloodGroupValidator : AbstractValidator<CreateBloodGroupDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly int _lang;

        public CreateBloodGroupValidator(ILocalizationService localizer, int lang = 1)
        {
            _localizer = localizer;
            _lang = lang;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localizer.GetMessage("CodeRequired", _lang))
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localizer.GetMessage("MaxLength", _lang), 2048));
        }
    }
}