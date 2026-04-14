using Application.Common.Abstractions;
using Application.System.MasterData.City.Dtos;
using FluentValidation;

namespace Application.System.MasterData.City.Validators
{
    public class UpdateCityValidator : AbstractValidator<UpdateCityDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public UpdateCityValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).When(x => x.PhoneKey != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.TimeZone)
                .MaximumLength(50).When(x => x.TimeZone != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage(_localizer.GetMessage("CountryRequired", lang));

            RuleFor(x => x.RegionId)
                .GreaterThan(0).When(x => x.RegionId.HasValue)
                .WithMessage(_localizer.GetMessage("RegionRequired", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateCityDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.PhoneKey != null ||
                   dto.RegionId.HasValue ||
                   dto.TimeZone != null ||
                   dto.CountryId.HasValue ||
                   dto.Remarks != null;
        }
    }
}