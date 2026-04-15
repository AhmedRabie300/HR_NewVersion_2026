using Application.Common.Abstractions;
using Application.System.MasterData.City.Dtos;
using FluentValidation;

namespace Application.System.MasterData.City.Validators
{
    public class CreateCityValidator : AbstractValidator<CreateCityDto>
    {
        public CreateCityValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.TimeZone)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage(localizer.GetMessage("CountryRequired", lang));

            RuleFor(x => x.RegionId)
                .GreaterThan(0).When(x => x.RegionId.HasValue)
                .WithMessage(localizer.GetMessage("RegionRequired", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}