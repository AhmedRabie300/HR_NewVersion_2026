using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Queries
{
    public static class GetEmployeeClassById
    {
        public record Query(int Id) : IRequest<EmployeeClassDto>;


        public class Handler : IRequestHandler<Query, EmployeeClassDto>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeClassRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<EmployeeClassDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
           

                return new EmployeeClassDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    NoOfDaysPerPeriod: entity.NoOfDaysPerPeriod,
                    WorkHoursPerDay: entity.WorkHoursPerDay,
                    NoOfHoursPerWeek: entity.NoOfHoursPerWeek,
                    NoOfHoursPerPeriod: entity.NoOfHoursPerPeriod,
                    OvertimeFactor: entity.OvertimeFactor,
                    HolidayFactor: entity.HolidayFactor,
                    FirstDayOfWeek: entity.FirstDayOfWeek,
                    DefultStartTime: entity.DefultStartTime,
                    DefultEndTime: entity.DefultEndTime,
                    WorkingUnitsIsHours: entity.WorkingUnitsIsHours,
                    DefaultProjectId: entity.DefaultProjectId,
                    DefaultProjectName: lang == 2 ? entity.DefaultProject?.ArbName : entity.DefaultProject?.EngName,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegUserId: entity.RegUserId,
                    RegComputerId: entity.RegComputerId,
                    CancelDate: entity.CancelDate,
                    NonPermiLatTransaction: entity.NonPermiLatTransaction,
                    PerDailyDelaying: entity.PerDailyDelaying,
                    PerMonthlyDelaying: entity.PerMonthlyDelaying,
                    NonProfitOverTimeH: entity.NonProfitOverTimeH,
                    EOBFormula: entity.EOBFormula,
                    OvertimeFormula: entity.OvertimeFormula,
                    HolidayFormula: entity.HolidayFormula,
                    OvertimeTransaction: entity.OvertimeTransaction,
                    HOvertimeTransaction: entity.HOvertimeTransaction,
                    PolicyCheckMachine: entity.PolicyCheckMachine,
                    HasAttendance: entity.HasAttendance,
                    PunishementCalc: entity.PunishementCalc,
                    OnNoExit: entity.OnNoExit,
                    DeductionMethod: entity.DeductionMethod,
                    MaxLoanAmtPCT: entity.MaxLoanAmtPCT,
                    MinServiceMonth: entity.MinServiceMonth,
                    MaxInstallementPCT: entity.MaxInstallementPCT,
                    EOSCostingTrns: entity.EOSCostingTrns,
                    TicketsCostingTrns: entity.TicketsCostingTrns,
                    VacCostingTrns: entity.VacCostingTrns,
                    HICostingTrns: entity.HICostingTrns,
                    TravalTrans: entity.TravalTrans,
                    AbsentFormula: entity.AbsentFormula,
                    LateFormula: entity.LateFormula,
                    VacCostFormula: entity.VacCostFormula,
                    HasFingerPrint: entity.HasFingerPrint,
                    HasOvertimeList: entity.HasOvertimeList,
                    AttendanceFromTimeSheet: entity.AttendanceFromTimeSheet,
                    HasFlexibleTime: entity.HasFlexibleTime,
                    HasFlexableFingerPrint: entity.HasFlexableFingerPrint,
                    AdvanceBalance: entity.AdvanceBalance,
                    VacationTrans: entity.VacationTrans,
                    VactionTransType: entity.VactionTransType,
                    TransValue: entity.TransValue,
                    AddBalanceInAddEmp: entity.AddBalanceInAddEmp,
                    AccumulatedBalance: entity.AccumulatedBalance,
                    RegDate: entity.RegDate,
                    IsActive: entity.IsActive(),
                    Delays: entity.Delays.Select(d => new EmployeeClassDelayDto(
                        Id: d.Id,
                        ClassId: d.ClassId,
                        FromMin: d.FromMin,
                        ToMin: d.ToMin,
                        PunishPCT: d.PunishPCT,
                        Remarks: d.Remarks,
                        RegDate: d.RegDate,
                        CancelDate: d.CancelDate,
                        IsActive: d.IsActive()
                    )).ToList(),
                    Vacations: entity.Vacations.Select(v => new EmployeeClassVacationDto(
                        Id: v.Id,
                        EmployeeClassId: v.EmployeeClassId,
                        VacationTypeId: v.VacationTypeId,
                        VacationTypeName: lang == 2 ? v.VacationType?.ArbName : v.VacationType?.EngName,
                        DurationDays: v.DurationDays,
                        RequiredWorkingMonths: v.RequiredWorkingMonths,
                        FromMonth: v.FromMonth,
                        ToMonth: v.ToMonth,
                        Remarks: v.Remarks,
                        TicketsRnd: v.TicketsRnd,
                        DependantTicketRnd: v.DependantTicketRnd,
                        MaxKeepDays: v.MaxKeepDays,
                        RegDate: v.RegDate,
                        CancelDate: v.CancelDate,
                        IsActive: v.IsActive()
                    )).ToList()
                );
            }
        }
    }
}