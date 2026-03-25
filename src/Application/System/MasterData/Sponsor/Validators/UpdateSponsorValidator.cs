using Application.System.MasterData.Sponsor.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Sponsor.Validators
{
    public class UpdateSponsorValidator : AbstractValidator<UpdateSponsorDto>
    {
        public UpdateSponsorValidator(ILocalizationService localizer, int lang)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.SponsorNumber)
                .GreaterThan(0).When(x => x.SponsorNumber.HasValue)
                .WithMessage(localizer.GetMessage("SponsorNumberMustBePositive", lang));

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).When(x => x.CompanyId.HasValue)
                .WithMessage(localizer.GetMessage("CompanyRequired", lang));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateSponsorDto dto)
        {
            return dto.EngName != null || dto.ArbName != null ||
                   dto.ArbName4S != null || dto.SponsorNumber.HasValue ||
                   dto.CompanyId.HasValue;
        }
    }
}