using Application.System.MasterData.Nationality.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Nationality.Validators
{
    public class UpdateNationalityValidator : AbstractValidator<UpdateNationalityDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public UpdateNationalityValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.TravelRoute)
                .GreaterThan(0).When(x => x.TravelRoute.HasValue)
                .WithMessage(x => _localizer.GetMessage("TravelRouteMustBePositive", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.TravelClass)
                .GreaterThan(0).When(x => x.TravelClass.HasValue)
                .WithMessage(x => _localizer.GetMessage("TravelClassMustBePositive", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.TicketAmount)
                .GreaterThan(0).When(x => x.TicketAmount.HasValue)
                .WithMessage(x => _localizer.GetMessage("TicketAmountMustBePositive", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => _localizer.GetMessage("AtLeastOneField", _ContextService.GetCurrentLanguage()));
        }

        private bool HaveAtLeastOneField(UpdateNationalityDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.IsMainNationality.HasValue ||
                   dto.TravelRoute.HasValue ||
                   dto.TravelClass.HasValue ||
                   dto.Remarks != null ||
                   dto.TicketAmount.HasValue;
        }
    }
}