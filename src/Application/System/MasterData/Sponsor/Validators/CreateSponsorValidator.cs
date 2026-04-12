using Application.System.MasterData.Sponsor.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Sponsor.Validators
{
    public class CreateSponsorValidator : AbstractValidator<CreateSponsorDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public CreateSponsorValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _ContextService.GetCurrentLanguage()))
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.SponsorNumber)
                .GreaterThan(0).When(x => x.SponsorNumber.HasValue)
                .WithMessage(x => _localizer.GetMessage("SponsorNumberMustBePositive", _ContextService.GetCurrentLanguage()));

           
        }
    }
}