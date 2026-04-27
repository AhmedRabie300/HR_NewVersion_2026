using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands
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
            private readonly IValidationMessages _msg;

            public Handler(IEmployeeClassRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
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

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}