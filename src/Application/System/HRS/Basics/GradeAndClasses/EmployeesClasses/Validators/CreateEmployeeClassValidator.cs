using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators
{
    public class CreateEmployeeClassValidator : AbstractValidator<CreateEmployeeClassDto>
    {
        private readonly IEmployeeClassRepository _repo;

        public CreateEmployeeClassValidator(IValidationMessages msg, IEmployeeClassRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) => !await _repo.CodeExistsAsync(code))
                .WithMessage(x => msg.Format("CodeExists", msg.Get("EmployeeClass"), x.Code));

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
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.EOBFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.OvertimeFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.HolidayFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.AbsentFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.LateFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.VacCostFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));

            RuleForEach(x => x.Delays)
                .SetValidator(new CreateEmployeeClassDelayValidator(msg));

            RuleForEach(x => x.Vacations)
                .SetValidator(new CreateEmployeeClassVacationValidator(msg));
        }
    }

    public class CreateEmployeeClassDelayValidator : AbstractValidator<CreateEmployeeClassDelayDto>
    {
        public CreateEmployeeClassDelayValidator(IValidationMessages msg)
        {
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
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.ClassID)
                     .GreaterThan(0).WithMessage(x => msg.Get("ClassIdRequired"));
                   


        }
    }

    public class CreateEmployeeClassVacationValidator : AbstractValidator<CreateEmployeeClassVacationDto>
    {
        public CreateEmployeeClassVacationValidator(IValidationMessages msg)
        {
            RuleFor(x => x.EmployeeClassId)
                 .GreaterThan(0).WithMessage(x => msg.Get("EmployeeClassIdRequired"));


            RuleFor(x => x.VacationTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("VacationTypeRequired"));

            RuleFor(x => x.DurationDays)
                .GreaterThan(0).WithMessage(x => msg.Get("DurationDaysPositive"));

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
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}