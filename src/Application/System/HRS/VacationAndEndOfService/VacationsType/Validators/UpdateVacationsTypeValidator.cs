using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationAndEndOfService.VacationsType.Validators
{
    public class UpdateVacationsTypeValidator : AbstractValidator<UpdateVacationsTypeDto>
    {
        private readonly IVacationsTypeRepository _repo;

        public UpdateVacationsTypeValidator(IValidationMessages msg, IVacationsTypeRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

             RuleFor(x => x.Code)
                .MaximumLength(50).When(x => x.Code != null)
                .WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (dto, code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code, dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("VacationsType"), x.Code));

             RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName, dto.Id, cancellation);
                })
                .When(x => x.EngName != null)
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

             RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName, dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Sex)
                .MaximumLength(1).When(x => x.Sex != null)
                .WithMessage(x => msg.Format("MaxLength", 1))
                .Must(x => x == null || x == "M" || x == "F" || x == "B")
                .WithMessage(x => msg.Get("SexMustBeMOrFOrB"));

            RuleFor(x => x.Religion)
                .MaximumLength(10).When(x => x.Religion != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.TimesNoInYear)
                .GreaterThan(0).When(x => x.TimesNoInYear.HasValue)
                .WithMessage(x => msg.Get("TimesNoInYearGreaterThanZero"));

            RuleFor(x => x.AllowedDaysNo)
                .GreaterThan(0).When(x => x.AllowedDaysNo.HasValue)
                .WithMessage(x => msg.Get("AllowedDaysNoGreaterThanZero"));

            RuleFor(x => x.Stage1Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage1Pct.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            RuleFor(x => x.Stage2Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage2Pct.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            RuleFor(x => x.Stage3Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage3Pct.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
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