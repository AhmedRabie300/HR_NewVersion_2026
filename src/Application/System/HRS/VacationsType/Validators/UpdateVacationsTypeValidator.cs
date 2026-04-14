using Application.Common.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationsType.Validators
{
    public class UpdateVacationsTypeValidator : AbstractValidator<UpdateVacationsTypeDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public UpdateVacationsTypeValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.Code)
                .MaximumLength(50).When(x => x.Code != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Sex)
                .MaximumLength(1).When(x => x.Sex != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 1))
                .Must(x => x == null || x == "M" || x == "F" || x == "B")
                .WithMessage(_localizer.GetMessage("SexMustBeMOrFOrB", lang));

            RuleFor(x => x.Religion)
                .MaximumLength(10).When(x => x.Religion != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.TimesNoInYear)
                .GreaterThan(0).When(x => x.TimesNoInYear.HasValue)
                .WithMessage(_localizer.GetMessage("TimesNoInYearGreaterThanZero", lang));

            RuleFor(x => x.AllowedDaysNo)
                .GreaterThan(0).When(x => x.AllowedDaysNo.HasValue)
                .WithMessage(_localizer.GetMessage("AllowedDaysNoGreaterThanZero", lang));

            RuleFor(x => x.Stage1Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage1Pct.HasValue)
                .WithMessage(_localizer.GetMessage("PercentageBetween0And100", lang));

            RuleFor(x => x.Stage2Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage2Pct.HasValue)
                .WithMessage(_localizer.GetMessage("PercentageBetween0And100", lang));

            RuleFor(x => x.Stage3Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage3Pct.HasValue)
                .WithMessage(_localizer.GetMessage("PercentageBetween0And100", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateVacationsTypeDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.IsPaid.HasValue ||
                   dto.Sex != null ||
                   dto.IsAnnual.HasValue ||
                   dto.IsSickVacation.HasValue ||
                   dto.IsFromAnnual.HasValue ||
                   dto.ForSalaryTransaction.HasValue ||
                   dto.Remarks != null ||
                   dto.OBalanceTransactionId.HasValue ||
                   dto.OverDueVacationId.HasValue ||
                   dto.Stage1Pct.HasValue ||
                   dto.Stage2Pct.HasValue ||
                   dto.Stage3Pct.HasValue ||
                   dto.ForDeductionTransaction.HasValue ||
                   dto.AffectEos.HasValue ||
                   dto.VactionTypeCaculation.HasValue ||
                   dto.ExceededDaysType.HasValue ||
                   dto.HasPayment.HasValue ||
                   dto.RoundAnnualVacBalance.HasValue ||
                   dto.Religion != null ||
                   dto.IsOfficial.HasValue ||
                   dto.OverlapWithAnotherVac.HasValue ||
                   dto.ConsiderAllowedDays.HasValue ||
                   dto.TimesNoInYear.HasValue ||
                   dto.AllowedDaysNo.HasValue ||
                   dto.ExcludedFromSsRequests.HasValue;
        }
    }
}