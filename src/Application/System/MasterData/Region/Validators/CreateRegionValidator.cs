using Application.Common.Abstractions;
using Application.System.MasterData.Region.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Region.Validators
{
    public class CreateRegionValidator : AbstractValidator<CreateRegionDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public CreateRegionValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage(_localizer.GetMessage("CountryRequired", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}