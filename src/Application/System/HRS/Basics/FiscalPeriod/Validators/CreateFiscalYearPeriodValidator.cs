using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;

namespace Application.System.HRS.Basics.FiscalPeriod.Validators
{
    public class CreateFiscalYearPeriodValidator : AbstractValidator<CreateFiscalYearPeriodDto>
    {
        private readonly IFiscalYearPeriodRepository _repo;
        private readonly IFiscalYearRepository _fiscalYearRepo;

        public CreateFiscalYearPeriodValidator(IValidationMessages msg, IFiscalYearPeriodRepository repo, IFiscalYearRepository fiscalYearRepo)
        {
            _repo = repo;
            _fiscalYearRepo = fiscalYearRepo;

            RuleFor(x => x.FiscalYearId)
                .GreaterThan(0).WithMessage(x => msg.Get("FiscalYearRequired"))
                .MustAsync(async (fiscalYearId, cancellation) => await _fiscalYearRepo.ExistsAsync(fiscalYearId))
                .WithMessage(x => msg.Format("NotFound", msg.Get("FiscalYear"), x.FiscalYearId));

            RuleFor(x => x.Code)
                .MaximumLength(30).WithMessage(x => msg.Format("MaxLength", 30))
                .MustAsync(async (dto, code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code.Trim());
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("FiscalYearPeriod"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate).When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage(x => msg.Get("FromDateLessThanToDate"));

            RuleFor(x => x.HFromDate)
                .MaximumLength(12).WithMessage(x => msg.Format("MaxLength", 12));

            RuleFor(x => x.HToDate)
                .MaximumLength(12).WithMessage(x => msg.Format("MaxLength", 12));

            RuleFor(x => x.PeriodType)
                .InclusiveBetween((byte)1, (byte)12).When(x => x.PeriodType.HasValue)
                .WithMessage(x => msg.Get("PeriodTypeRange"));

            RuleFor(x => x.PeriodRank)
                .GreaterThanOrEqualTo((byte)1).When(x => x.PeriodRank.HasValue)
                .WithMessage(x => msg.Get("PeriodRankPositive"));

            RuleFor(x => x.PrepareFromDate)
                .LessThanOrEqualTo(x => x.PrepareToDate).When(x => x.PrepareFromDate.HasValue && x.PrepareToDate.HasValue)
                .WithMessage(x => msg.Get("PrepareFromDateLessThanPrepareToDate"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));
        }
    }

    public class CreateFiscalYearPeriodModuleValidator : AbstractValidator<CreateFiscalYearPeriodModuleDto>
    {
        private readonly IFiscalYearPeriodRepository _repo;
        private readonly IModuleRepository _moduleRepo;

        public CreateFiscalYearPeriodModuleValidator(IValidationMessages msg, IFiscalYearPeriodRepository repo, IModuleRepository moduleRepo)
        {
            _repo = repo;
            _moduleRepo = moduleRepo;

            RuleFor(x => x.FiscalYearPeriodId)
                .GreaterThan(0).WithMessage(x => msg.Get("FiscalYearPeriodRequired"))
                .MustAsync(async (periodId, cancellation) => await _repo.ExistsAsync(periodId))
                .WithMessage(x => msg.Format("NotFound", msg.Get("FiscalYearPeriod"), x.FiscalYearPeriodId));

            RuleFor(x => x.ModuleId)
                .GreaterThan(0).WithMessage(x => msg.Get("ModuleRequired"))
                .MustAsync(async (moduleId, cancellation) => await _moduleRepo.ExistsAsync(moduleId))
                .WithMessage(x => msg.Format("NotFound", msg.Get("Module"), x.ModuleId));

            RuleFor(x => x.OpenDate)
                .LessThanOrEqualTo(x => x.CloseDate).When(x => x.OpenDate.HasValue && x.CloseDate.HasValue)
                .WithMessage(x => msg.Get("OpenDateLessThanCloseDate"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    var existing = await _repo.GetModulesByPeriodIdAsync(dto.FiscalYearPeriodId);
                    return !existing.Any(m => m.ModuleId == dto.ModuleId && m.CancelDate == null);
                })
                .WithMessage(x => msg.Get("ModuleAlreadyExistsInPeriod"));
        }
    }
}