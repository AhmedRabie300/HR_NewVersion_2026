using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators;
using Domain.System.HRS.Basics.GradesAndClasses;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos;

using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Commands
{
    public static class UpdateEmployeeClass
    {
        public record Command(UpdateEmployeeClassDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeClassRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateEmployeeClassValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IEmployeeClassRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("EmployeeClass", request.Data.Id));

                entity.Update(
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
                   remarks: request.Data.Remarks,
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

                if (request.Data.Delays != null)
                {
                    foreach (var delayDto in request.Data.Delays)
                    {
                        if (delayDto.Id > 0)
                        {
                            var delay = entity.Delays.FirstOrDefault(d => d.Id == delayDto.Id);
                            if (delay != null)
                            {
                                delay.Update(
                                    fromMin: delayDto.FromMin,
                                    toMin: delayDto.ToMin,
                                    punishPCT: delayDto.PunishPCT,
                                    remarks: delayDto.Remarks
                                );
                            }
                        }
                        else
                        {
                            var newDelay = new EmployeeClassDelay(
                                classId: entity.Id,
                                fromMin: delayDto.FromMin,
                                toMin: delayDto.ToMin,
                                punishPCT: delayDto.PunishPCT,
                                remarks: delayDto.Remarks
                            );
                            entity.AddDelay(newDelay);
                        }
                    }
                }

                if (request.Data.Vacations != null)
                {
                    foreach (var vacationDto in request.Data.Vacations)
                    {
                        if (vacationDto.Id > 0)
                        {
                            var vacation = entity.Vacations.FirstOrDefault(v => v.Id == vacationDto.Id);
                            if (vacation != null)
                            {
                                vacation.Update(
                                    durationDays: vacationDto.DurationDays,
                                    requiredWorkingMonths: vacationDto.RequiredWorkingMonths,
                                    fromMonth: vacationDto.FromMonth,
                                    toMonth: vacationDto.ToMonth,
                                    remarks: vacationDto.Remarks,
                                    ticketsRnd: vacationDto.TicketsRnd,
                                    dependantTicketRnd: vacationDto.DependantTicketRnd,
                                    maxKeepDays: vacationDto.MaxKeepDays
                                );
                            }
                        }
                        else
                        {

                            var newVacation = new EmployeeClassVacation(
                                employeeClassId: entity.Id,
                                vacationTypeId: vacationDto.VacationTypeId,
                                durationDays: vacationDto.DurationDays,
                                requiredWorkingMonths: vacationDto.RequiredWorkingMonths,
                                fromMonth: vacationDto.FromMonth,
                                toMonth: vacationDto.ToMonth,
                                remarks: vacationDto.Remarks,
                                ticketsRnd: vacationDto.TicketsRnd,
                                dependantTicketRnd: vacationDto.DependantTicketRnd,
                                maxKeepDays: vacationDto.MaxKeepDays
                            );
                            entity.AddVacation(newVacation);
                        }
                    }
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}