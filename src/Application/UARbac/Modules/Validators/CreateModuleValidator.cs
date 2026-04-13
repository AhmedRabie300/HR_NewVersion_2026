// Application/UARbac/Modules/Validators/CreateModuleValidator.cs
using Application.Common.Abstractions;
using Application.UARbac.Modules.Dtos;
using FluentValidation;

namespace Application.UARbac.Modules.Validators
{
    public class CreateModuleValidator : AbstractValidator<CreateModuleDto>
    {
        private readonly IContextService _contextService;
        private readonly ILocalizationService _localizer;

        public CreateModuleValidator(IContextService contextService, ILocalizationService localizer)
        {
            _contextService = contextService;
            _localizer = localizer;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .NotEmpty()
                    .WithMessage(_localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50))
                .Matches("^[A-Za-z0-9_]+$")
                    .WithMessage(_localizer.GetMessage("CodePattern", lang));

            RuleFor(x => x.Prefix)
                .MaximumLength(30)
                    .When(x => !string.IsNullOrEmpty(x.Prefix))
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 30));

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.EngName))
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.ArbName))
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.ArbName4S))
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.FormId)
                .GreaterThan(0)
                    .When(x => x.FormId.HasValue)
                    .WithMessage(_localizer.GetMessage("FormIdGreaterThanZero", lang));

            RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0)
                    .When(x => x.Rank.HasValue)
                    .WithMessage(_localizer.GetMessage("RankMustBePositive", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048)
                    .When(x => !string.IsNullOrEmpty(x.Remarks))
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}