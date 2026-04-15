using Application.Common.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Nationality.Validators
{
    public class CreateNationalityValidator : AbstractValidator<CreateNationalityDto>
    {
        public CreateNationalityValidator(ILocalizationService localizer, IContextService contextService)
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

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x.TravelRoute)
                .GreaterThan(0).When(x => x.TravelRoute.HasValue)
                .WithMessage(localizer.GetMessage("TravelRouteMustBePositive", lang));

            RuleFor(x => x.TravelClass)
                .GreaterThan(0).When(x => x.TravelClass.HasValue)
                .WithMessage(localizer.GetMessage("TravelClassMustBePositive", lang));

            RuleFor(x => x.TicketAmount)
                .GreaterThan(0).When(x => x.TicketAmount.HasValue)
                .WithMessage(localizer.GetMessage("TicketAmountMustBePositive", lang));
        }
    }
}