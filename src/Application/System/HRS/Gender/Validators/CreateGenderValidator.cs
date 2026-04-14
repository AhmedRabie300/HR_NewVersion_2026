using Application.Common.Abstractions;
using Application.System.HRS.Gender.Dtos;
using FluentValidation;

namespace Application.System.HRS.Gender.Validators
{
    public class CreateGenderValidator : AbstractValidator<CreateGenderDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public CreateGenderValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));
        }
    }
}