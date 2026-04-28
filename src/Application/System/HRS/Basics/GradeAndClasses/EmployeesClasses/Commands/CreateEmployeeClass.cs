using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators;
using Domain.System.HRS.Basics.GradesAndClasses;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
{
    public static class CreateEmployeeClass
    {
        public record Command(CreateEmployeeClassDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateEmployeeClassValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeClassRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;
                var regUserId = _currentUser.UserId ?? 0;

                var entity = new EmployeeClass(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    noOfDaysPerPeriod: request.Data.NoOfDaysPerPeriod,
                    workHoursPerDay: request.Data.WorkHoursPerDay,
                    noOfHoursPerWeek: request.Data.NoOfHoursPerWeek,
                    noOfHoursPerPeriod: request.Data.NoOfHoursPerPeriod,
                    overtimeFactor: request.Data.OvertimeFactor,
                    holidayFactor: request.Data.HolidayFactor,
                    firstDayOfWeek: request.Data.FirstDayOfWeek,
                    defultStartTime: request.Data.DefultStartTime,
                    defultEndTime: request.Data.DefultEndTime,
                    workingUnitsIsHours: request.Data.WorkingUnitsIsHours,
                    defaultProjectId: request.Data.DefaultProjectId,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regUserId: regUserId,
                    regComputerId: request.Data.RegComputerId,
                    nonPermiLatTransaction: request.Data.NonPermiLatTransaction,
                    perDailyDelaying: request.Data.PerDailyDelaying,
                    perMonthlyDelaying: request.Data.PerMonthlyDelaying,
                    nonProfitOverTimeH: request.Data.NonProfitOverTimeH,
                    eobFormula: request.Data.EOBFormula,
                    overtimeFormula: request.Data.OvertimeFormula,
                    holidayFormula: request.Data.HolidayFormula,
                    overtimeTransaction: request.Data.OvertimeTransaction,
                    hOvertimeTransaction: request.Data.HOvertimeTransaction,
                    policyCheckMachine: request.Data.PolicyCheckMachine,
                    hasAttendance: request.Data.HasAttendance,
                    punishementCalc: request.Data.PunishementCalc,
                    onNoExit: request.Data.OnNoExit,
                    deductionMethod: request.Data.DeductionMethod,
                    maxLoanAmtPCT: request.Data.MaxLoanAmtPCT,
                    minServiceMonth: request.Data.MinServiceMonth,
                    maxInstallementPCT: request.Data.MaxInstallementPCT,
                    eosCostingTrns: request.Data.EOSCostingTrns,
                    ticketsCostingTrns: request.Data.TicketsCostingTrns,
                    vacCostingTrns: request.Data.VacCostingTrns,
                    hiCostingTrns: request.Data.HICostingTrns,
                    travalTrans: request.Data.TravalTrans,
                    absentFormula: request.Data.AbsentFormula,
                    lateFormula: request.Data.LateFormula,
                    vacCostFormula: request.Data.VacCostFormula,
                    hasFingerPrint: request.Data.HasFingerPrint,
                    hasOvertimeList: request.Data.HasOvertimeList,
                    attendanceFromTimeSheet: request.Data.AttendanceFromTimeSheet,
                    hasFlexibleTime: request.Data.HasFlexibleTime,
                    hasFlexableFingerPrint: request.Data.HasFlexableFingerPrint,
                    advanceBalance: request.Data.AdvanceBalance,
                    vacationTrans: request.Data.VacationTrans,
                    vactionTransType: request.Data.VactionTransType,
                    transValue: request.Data.TransValue,
                    addBalanceInAddEmp: request.Data.AddBalanceInAddEmp,
                    accumulatedBalance: request.Data.AccumulatedBalance
                );

                // Add Delays
                if (request.Data.Delays != null)
                {
                    foreach (var delayDto in request.Data.Delays)
                    {
                        var delay = new EmployeeClassDelay(
                            classId: 0,
                            fromMin: delayDto.FromMin,
                            toMin: delayDto.ToMin,
                            punishPCT: delayDto.PunishPCT,
                            companyId: companyId,
                            remarks: delayDto.Remarks,
                            regComputerId: delayDto.RegComputerId
                        );
                        entity.AddDelay(delay);
                    }
                }

                // Add Vacations
                if (request.Data.Vacations != null)
                {
                    foreach (var vacationDto in request.Data.Vacations)
                    {
                        var vacation = new EmployeeClassVacation(
                            employeeClassId: 0,
                            vacationTypeId: vacationDto.VacationTypeId,
                            durationDays: vacationDto.DurationDays,
                            requiredWorkingMonths: vacationDto.RequiredWorkingMonths,
                            fromMonth: vacationDto.FromMonth,
                            toMonth: vacationDto.ToMonth,
                            companyId: companyId,
                            remarks: vacationDto.Remarks,
                            ticketsRnd: vacationDto.TicketsRnd,
                            dependantTicketRnd: vacationDto.DependantTicketRnd,
                            maxKeepDays: vacationDto.MaxKeepDays,
                            regComputerId: vacationDto.RegComputerId
                        );
                        entity.AddVacation(vacation);
                    }
                }

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}