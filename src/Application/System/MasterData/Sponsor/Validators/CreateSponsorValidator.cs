using Application.System.MasterData.Sponsor.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Sponsor.Validators
{
    public class CreateSponsorValidator : AbstractValidator<CreateSponsorDto>
    {
        public CreateSponsorValidator(ILocalizationService localizer, int lang)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.SponsorNumber)
                .GreaterThan(0).When(x => x.SponsorNumber.HasValue)
                .WithMessage(localizer.GetMessage("SponsorNumberMustBePositive", lang));

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).When(x => x.CompanyId.HasValue)
                .WithMessage(localizer.GetMessage("CompanyRequired", lang));
        }
    }
}