using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Queries
{
    public static class GetPagedEmployeeClasses
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null
        ) : IRequest<PagedResult<EmployeeClassDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<EmployeeClassDto>>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeClassRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<EmployeeClassDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new EmployeeClassDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    NoOfDaysPerPeriod: x.NoOfDaysPerPeriod,
                    WorkHoursPerDay: x.WorkHoursPerDay,
                    NoOfHoursPerWeek: x.NoOfHoursPerWeek,
                    NoOfHoursPerPeriod: x.NoOfHoursPerPeriod,
                    OvertimeFactor: x.OvertimeFactor,
                    HolidayFactor: x.HolidayFactor,
                    FirstDayOfWeek: x.FirstDayOfWeek,
                    DefultStartTime: x.DefultStartTime,
                    DefultEndTime: x.DefultEndTime,
                    WorkingUnitsIsHours: x.WorkingUnitsIsHours,
                    DefaultProjectId: x.DefaultProjectId,
                    DefaultProjectName: lang == 2 ? x.DefaultProject?.ArbName : x.DefaultProject?.EngName,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegUserId: x.RegUserId,
                    RegComputerId: x.RegComputerId,
                    CancelDate: x.CancelDate,
                    NonPermiLatTransaction: x.NonPermiLatTransaction,
                    PerDailyDelaying: x.PerDailyDelaying,
                    PerMonthlyDelaying: x.PerMonthlyDelaying,
                    NonProfitOverTimeH: x.NonProfitOverTimeH,
                    EOBFormula: x.EOBFormula,
                    OvertimeFormula: x.OvertimeFormula,
                    HolidayFormula: x.HolidayFormula,
                    OvertimeTransaction: x.OvertimeTransaction,
                    HOvertimeTransaction: x.HOvertimeTransaction,
                    PolicyCheckMachine: x.PolicyCheckMachine,
                    HasAttendance: x.HasAttendance,
                    PunishementCalc: x.PunishementCalc,
                    OnNoExit: x.OnNoExit,
                    DeductionMethod: x.DeductionMethod,
                    MaxLoanAmtPCT: x.MaxLoanAmtPCT,
                    MinServiceMonth: x.MinServiceMonth,
                    MaxInstallementPCT: x.MaxInstallementPCT,
                    EOSCostingTrns: x.EOSCostingTrns,
                    TicketsCostingTrns: x.TicketsCostingTrns,
                    VacCostingTrns: x.VacCostingTrns,
                    HICostingTrns: x.HICostingTrns,
                    TravalTrans: x.TravalTrans,
                    AbsentFormula: x.AbsentFormula,
                    LateFormula: x.LateFormula,
                    VacCostFormula: x.VacCostFormula,
                    HasFingerPrint: x.HasFingerPrint,
                    HasOvertimeList: x.HasOvertimeList,
                    AttendanceFromTimeSheet: x.AttendanceFromTimeSheet,
                    HasFlexibleTime: x.HasFlexibleTime,
                    HasFlexableFingerPrint: x.HasFlexableFingerPrint,
                    AdvanceBalance: x.AdvanceBalance,
                    VacationTrans: x.VacationTrans,
                    VactionTransType: x.VactionTransType,
                    TransValue: x.TransValue,
                    AddBalanceInAddEmp: x.AddBalanceInAddEmp,
                    AccumulatedBalance: x.AccumulatedBalance,
                    RegDate: x.RegDate,
                    IsActive: x.IsActive(),
                    Delays: x.Delays.Select(d => new EmployeeClassDelayDto(
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
                    Vacations: x.Vacations.Select(v => new EmployeeClassVacationDto(
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
                )).ToList();

                return new PagedResult<EmployeeClassDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}