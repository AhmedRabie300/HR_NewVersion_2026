using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;

namespace Application.System.HRS.Basics.FiscalPeriod.Validators
{
    public class UpdateFiscalYearPeriodValidator : AbstractValidator<UpdateFiscalYearPeriodDto>
    {
        private readonly IFiscalYearPeriodRepository _repo;

        public UpdateFiscalYearPeriodValidator(IValidationMessages msg, IFiscalYearPeriodRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.Code)
                .MaximumLength(30).When(x => x.Code != null)
                .WithMessage(x => msg.Format("MaxLength", 30))
                .MustAsync(async (dto, code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code.Trim(), dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("FiscalYearPeriod"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), dto.Id, cancellation);
                })
                .When(x => x.EngName != null)
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate).When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage(x => msg.Get("FromDateLessThanToDate"));

            RuleFor(x => x.HFromDate)
                .MaximumLength(12).When(x => x.HFromDate != null)
                .WithMessage(x => msg.Format("MaxLength", 12));

            RuleFor(x => x.HToDate)
                .MaximumLength(12).When(x => x.HToDate != null)
                .WithMessage(x => msg.Format("MaxLength", 12));

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
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateFiscalYearPeriodDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.FromDate.HasValue ||
                   dto.ToDate.HasValue ||
                   dto.Remarks != null ||
                   dto.HFromDate != null ||
                   dto.HToDate != null ||
                   dto.PeriodType.HasValue ||
                   dto.PeriodRank.HasValue ||
                   dto.PrepareFromDate.HasValue ||
                   dto.PrepareToDate.HasValue;
        }
    }

    public class UpdateFiscalYearPeriodModuleValidator : AbstractValidator<UpdateFiscalYearPeriodModuleDto>
    {
        private readonly IFiscalYearPeriodRepository _repo;
        private readonly IModuleRepository _moduleRepo;

        public UpdateFiscalYearPeriodModuleValidator(IValidationMessages msg, IFiscalYearPeriodRepository repo, IModuleRepository moduleRepo)
        {
            _repo = repo;
            _moduleRepo = moduleRepo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

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
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateFiscalYearPeriodModuleDto dto)
        {
            return dto.OpenDate.HasValue ||
                   dto.CloseDate.HasValue ||
                   dto.Remarks != null;
        }
    }
}