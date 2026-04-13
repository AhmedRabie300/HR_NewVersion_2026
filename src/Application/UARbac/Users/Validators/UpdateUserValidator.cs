using Application.Common.Abstractions;
using Application.UARbac.Users.Dtos;
using FluentValidation;

namespace Application.UARbac.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        private readonly IContextService _contextService;
        private readonly ILocalizationService _localizer;

        public UpdateUserValidator(IContextService contextService, ILocalizationService localizer)
        {
            _contextService = contextService;
            _localizer = localizer;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x)
                .Must(x => x.EngName != null || x.ArbName != null || x.IsAdmin != null || x.DeviceToken != null)
                .WithMessage(_localizer.GetMessage("AtLeastOneField", lang));
        }
    }
}