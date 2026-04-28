using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators
{
    public class UpdateEmployeeClassValidator : AbstractValidator<UpdateEmployeeClassDto>
    {
        private readonly IEmployeeClassRepository _repo;

        public UpdateEmployeeClassValidator(IValidationMessages msg, IEmployeeClassRepository repo)
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
                    return !await _repo.CodeExistsAsync(code.Trim(), dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("EmployeeClass"), x.Code));

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

            RuleFor(x => x.NoOfDaysPerPeriod)
                .Must(x => !x.HasValue || (x.Value >= 1 && x.Value <= 255))
                .WithMessage(x => msg.Get("NoOfDaysPerPeriodRange"));

            RuleFor(x => x.WorkHoursPerDay)
                .GreaterThan(0).When(x => x.WorkHoursPerDay.HasValue)
                .WithMessage(x => msg.Get("WorkHoursPerDayPositive"));

           
            RuleFor(x => x.FirstDayOfWeek)
                .Must(x => !x.HasValue || (x.Value >= 0 && x.Value <= 6))
                .WithMessage(x => msg.Get("FirstDayOfWeekRange"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.EOBFormula)
                .MaximumLength(2048).When(x => x.EOBFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.OvertimeFormula)
                .MaximumLength(2048).When(x => x.OvertimeFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.HolidayFormula)
                .MaximumLength(2048).When(x => x.HolidayFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.AbsentFormula)
                .MaximumLength(2048).When(x => x.AbsentFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.LateFormula)
                .MaximumLength(2048).When(x => x.LateFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.VacCostFormula)
                .MaximumLength(2048).When(x => x.VacCostFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateEmployeeClassDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.NoOfDaysPerPeriod.HasValue ||
                   dto.WorkHoursPerDay.HasValue ||
                   dto.NoOfHoursPerWeek.HasValue ||
                   dto.NoOfHoursPerPeriod.HasValue ||
                   dto.OvertimeFactor.HasValue ||
                   dto.HolidayFactor.HasValue ||
                   dto.FirstDayOfWeek.HasValue ||
                   dto.DefultStartTime.HasValue ||
                   dto.DefultEndTime.HasValue ||
                   dto.WorkingUnitsIsHours.HasValue ||
                   dto.DefaultProjectId.HasValue ||
                   dto.Remarks != null ||
                   dto.NonPermiLatTransaction.HasValue ||
                   dto.PerDailyDelaying.HasValue ||
                   dto.PerMonthlyDelaying.HasValue ||
                   dto.NonProfitOverTimeH.HasValue ||
                   dto.EOBFormula != null ||
                   dto.OvertimeFormula != null ||
                   dto.HolidayFormula != null ||
                   dto.OvertimeTransaction.HasValue ||
                   dto.HOvertimeTransaction.HasValue ||
                   dto.PolicyCheckMachine.HasValue ||
                   dto.HasAttendance.HasValue ||
                   dto.PunishementCalc.HasValue ||
                   dto.OnNoExit.HasValue ||
                   dto.DeductionMethod.HasValue ||
                   dto.MaxLoanAmtPCT.HasValue ||
                   dto.MinServiceMonth.HasValue ||
                   dto.MaxInstallementPCT.HasValue ||
                   dto.EOSCostingTrns.HasValue ||
                   dto.TicketsCostingTrns.HasValue ||
                   dto.VacCostingTrns.HasValue ||
                   dto.HICostingTrns.HasValue ||
                   dto.TravalTrans.HasValue ||
                   dto.AbsentFormula != null ||
                   dto.LateFormula != null ||
                   dto.VacCostFormula != null ||
                   dto.HasFingerPrint.HasValue ||
                   dto.HasOvertimeList.HasValue ||
                   dto.AttendanceFromTimeSheet.HasValue ||
                   dto.HasFlexibleTime.HasValue ||
                   dto.HasFlexableFingerPrint.HasValue ||
                   dto.AdvanceBalance.HasValue ||
                   dto.VacationTrans.HasValue ||
                   dto.VactionTransType.HasValue ||
                   dto.TransValue.HasValue ||
                   dto.AddBalanceInAddEmp.HasValue ||
                   dto.AccumulatedBalance.HasValue;
        }
    }

    public class UpdateEmployeeClassDelayValidator : AbstractValidator<UpdateEmployeeClassDelayDto>
    {
        public UpdateEmployeeClassDelayValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage(x => msg.Get("ClassIdRequired"));

            RuleFor(x => x.FromMin)
                .GreaterThanOrEqualTo(0).When(x => x.FromMin.HasValue)
                .WithMessage(x => msg.Get("FromMinPositive"));

            RuleFor(x => x.ToMin)
                .GreaterThanOrEqualTo(0).When(x => x.ToMin.HasValue)
                .WithMessage(x => msg.Get("ToMinPositive"));

            RuleFor(x => x.PunishPCT)
                .Must(x => !x.HasValue || (x.Value >= 0 && x.Value <= 100))
                .WithMessage(x => msg.Get("PunishPCTRange"));

            RuleFor(x => x)
                .Must(x => !x.FromMin.HasValue || !x.ToMin.HasValue || x.FromMin <= x.ToMin)
                .When(x => x.FromMin.HasValue && x.ToMin.HasValue)
                .WithMessage(x => msg.Get("FromMinLessThanToMin"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateEmployeeClassDelayDto dto)
        {
            return dto.FromMin.HasValue ||
                   dto.ToMin.HasValue ||
                   dto.PunishPCT.HasValue ||
                   dto.Remarks != null;
        }
    }

    public class UpdateEmployeeClassVacationValidator : AbstractValidator<UpdateEmployeeClassVacationDto>
    {
        public UpdateEmployeeClassVacationValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EmployeeClassId)
                .GreaterThan(0).WithMessage(x => msg.Get("EmployeeClassIdRequired"));

            RuleFor(x => x.VacationTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("VacationTypeIdRequired"));

            RuleFor(x => x.DurationDays)
                .GreaterThan(0).When(x => x.DurationDays.HasValue)
                .WithMessage(x => msg.Get("DurationDaysPositive"));

            RuleFor(x => x.RequiredWorkingMonths)
                .GreaterThan(0).When(x => x.RequiredWorkingMonths.HasValue)
                .WithMessage(x => msg.Get("RequiredWorkingMonthsPositive"));

            RuleFor(x => x.FromMonth)
                .GreaterThanOrEqualTo(0).When(x => x.FromMonth.HasValue)
                .WithMessage(x => msg.Get("FromMonthPositive"));

            RuleFor(x => x.ToMonth)
                .GreaterThanOrEqualTo(0).When(x => x.ToMonth.HasValue)
                .WithMessage(x => msg.Get("ToMonthPositive"));

            RuleFor(x => x)
                .Must(x => !x.FromMonth.HasValue || !x.ToMonth.HasValue || x.FromMonth <= x.ToMonth)
                .WithMessage(x => msg.Get("FromMonthLessThanToMonth"));

            RuleFor(x => x.TicketsRnd)
                .GreaterThanOrEqualTo(0).When(x => x.TicketsRnd.HasValue)
                .WithMessage(x => msg.Get("TicketsRndPositive"));

            RuleFor(x => x.DependantTicketRnd)
                .GreaterThanOrEqualTo(0).When(x => x.DependantTicketRnd.HasValue)
                .WithMessage(x => msg.Get("DependantTicketRndPositive"));

            RuleFor(x => x.MaxKeepDays)
                .GreaterThanOrEqualTo(0).When(x => x.MaxKeepDays.HasValue)
                .WithMessage(x => msg.Get("MaxKeepDaysPositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateEmployeeClassVacationDto dto)
        {
            return dto.DurationDays.HasValue ||
                   dto.RequiredWorkingMonths.HasValue ||
                   dto.FromMonth.HasValue ||
                   dto.ToMonth.HasValue ||
                   dto.Remarks != null ||
                   dto.TicketsRnd.HasValue ||
                   dto.DependantTicketRnd.HasValue ||
                   dto.MaxKeepDays.HasValue;
        }
    }
}