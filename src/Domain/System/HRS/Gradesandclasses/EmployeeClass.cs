using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class EmployeeClass : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public byte? NoOfDaysPerPeriod { get; private set; }
        public float? WorkHoursPerDay { get; private set; }
        public short? NoOfHoursPerWeek { get; private set; }
        public short? NoOfHoursPerPeriod { get; private set; }
        public float? OvertimeFactor { get; private set; }
        public float? HolidayFactor { get; private set; }
        public byte? FirstDayOfWeek { get; private set; }
        public DateTime? DefultStartTime { get; private set; }
        public DateTime? DefultEndTime { get; private set; }
        public bool? WorkingUnitsIsHours { get; private set; }
        public int? DefaultProjectId { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? NonPermiLatTransaction { get; private set; }
        public int? PerDailyDelaying { get; private set; }
        public int? PerMonthlyDelaying { get; private set; }
        public int? NonProfitOverTimeH { get; private set; }
        public string? EOBFormula { get; private set; }
        public string? OvertimeFormula { get; private set; }
        public string? HolidayFormula { get; private set; }
        public int? OvertimeTransaction { get; private set; }
        public int? HOvertimeTransaction { get; private set; }
        public bool? PolicyCheckMachine { get; private set; }
        public bool? HasAttendance { get; private set; }
        public int? PunishementCalc { get; private set; }
        public int? OnNoExit { get; private set; }
        public int? DeductionMethod { get; private set; }
        public int? MaxLoanAmtPCT { get; private set; }
        public int? MinServiceMonth { get; private set; }
        public int? MaxInstallementPCT { get; private set; }
        public int? EOSCostingTrns { get; private set; }
        public int? TicketsCostingTrns { get; private set; }
        public int? VacCostingTrns { get; private set; }
        public int? HICostingTrns { get; private set; }
        public int? TravalTrans { get; private set; }
        public string? AbsentFormula { get; private set; }
        public string? LateFormula { get; private set; }
        public string? VacCostFormula { get; private set; }
        public bool? HasFingerPrint { get; private set; }
        public bool? HasOvertimeList { get; private set; }
        public bool? AttendanceFromTimeSheet { get; private set; }
        public bool? HasFlexibleTime { get; private set; }
        public bool? HasFlexableFingerPrint { get; private set; }
        public bool? AdvanceBalance { get; private set; }
        public bool? VacationTrans { get; private set; }
        public int? VactionTransType { get; private set; }
        public int? TransValue { get; private set; }
        public bool? AddBalanceInAddEmp { get; private set; }
        public bool? AccumulatedBalance { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public Project? DefaultProject { get; private set; }

        private readonly List<EmployeeClassDelay> _delays = new();
        private readonly List<EmployeeClassVacation> _vacations = new();

        public IReadOnlyCollection<EmployeeClassDelay> Delays => _delays.AsReadOnly();
        public IReadOnlyCollection<EmployeeClassVacation> Vacations => _vacations.AsReadOnly();

        private EmployeeClass() { }

        public EmployeeClass(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            byte? noOfDaysPerPeriod,
            float? workHoursPerDay,
            short? noOfHoursPerWeek,
            short? noOfHoursPerPeriod,
            float? overtimeFactor,
            float? holidayFactor,
            byte? firstDayOfWeek,
            DateTime? defultStartTime,
            DateTime? defultEndTime,
            bool? workingUnitsIsHours,
            int? defaultProjectId,
            int companyId,
            string? remarks,
            int regUserId,
            int? regComputerId,
            int? nonPermiLatTransaction,
            int? perDailyDelaying,
            int? perMonthlyDelaying,
            int? nonProfitOverTimeH,
            string? eobFormula,
            string? overtimeFormula,
            string? holidayFormula,
            int? overtimeTransaction,
            int? hOvertimeTransaction,
            bool? policyCheckMachine,
            bool? hasAttendance,
            int? punishementCalc,
            int? onNoExit,
            int? deductionMethod,
            int? maxLoanAmtPCT,
            int? minServiceMonth,
            int? maxInstallementPCT,
            int? eosCostingTrns,
            int? ticketsCostingTrns,
            int? vacCostingTrns,
            int? hiCostingTrns,
            int? travalTrans,
            string? absentFormula,
            string? lateFormula,
            string? vacCostFormula,
            bool? hasFingerPrint,
            bool? hasOvertimeList,
            bool? attendanceFromTimeSheet,
            bool? hasFlexibleTime,
            bool? hasFlexableFingerPrint,
            bool? advanceBalance,
            bool? vacationTrans,
            int? vactionTransType,
            int? transValue,
            bool? addBalanceInAddEmp,
            bool? accumulatedBalance)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            NoOfDaysPerPeriod = noOfDaysPerPeriod;
            WorkHoursPerDay = workHoursPerDay;
            NoOfHoursPerWeek = noOfHoursPerWeek;
            NoOfHoursPerPeriod = noOfHoursPerPeriod;
            OvertimeFactor = overtimeFactor;
            HolidayFactor = holidayFactor;
            FirstDayOfWeek = firstDayOfWeek;
            DefultStartTime = defultStartTime;
            DefultEndTime = defultEndTime;
            WorkingUnitsIsHours = workingUnitsIsHours;
            DefaultProjectId = defaultProjectId;
            CompanyId = companyId;
            Remarks = remarks;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            NonPermiLatTransaction = nonPermiLatTransaction;
            PerDailyDelaying = perDailyDelaying;
            PerMonthlyDelaying = perMonthlyDelaying;
            NonProfitOverTimeH = nonProfitOverTimeH;
            EOBFormula = eobFormula;
            OvertimeFormula = overtimeFormula;
            HolidayFormula = holidayFormula;
            OvertimeTransaction = overtimeTransaction;
            HOvertimeTransaction = hOvertimeTransaction;
            PolicyCheckMachine = policyCheckMachine;
            HasAttendance = hasAttendance;
            PunishementCalc = punishementCalc;
            OnNoExit = onNoExit;
            DeductionMethod = deductionMethod;
            MaxLoanAmtPCT = maxLoanAmtPCT;
            MinServiceMonth = minServiceMonth;
            MaxInstallementPCT = maxInstallementPCT;
            EOSCostingTrns = eosCostingTrns;
            TicketsCostingTrns = ticketsCostingTrns;
            VacCostingTrns = vacCostingTrns;
            HICostingTrns = hiCostingTrns;
            TravalTrans = travalTrans;
            AbsentFormula = absentFormula;
            LateFormula = lateFormula;
            VacCostFormula = vacCostFormula;
            HasFingerPrint = hasFingerPrint;
            HasOvertimeList = hasOvertimeList;
            AttendanceFromTimeSheet = attendanceFromTimeSheet;
            HasFlexibleTime = hasFlexibleTime;
            HasFlexableFingerPrint = hasFlexableFingerPrint;
            AdvanceBalance = advanceBalance;
            VacationTrans = vacationTrans;
            VactionTransType = vactionTransType;
            TransValue = transValue;
            AddBalanceInAddEmp = addBalanceInAddEmp;
            AccumulatedBalance = accumulatedBalance;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            byte? noOfDaysPerPeriod,
            float? workHoursPerDay,
            short? noOfHoursPerWeek,
            short? noOfHoursPerPeriod,
            float? overtimeFactor,
            float? holidayFactor,
            byte? firstDayOfWeek,
            DateTime? defultStartTime,
            DateTime? defultEndTime,
            bool? workingUnitsIsHours,
            int? defaultProjectId,
            string? remarks,
            int? nonPermiLatTransaction,
            int? perDailyDelaying,
            int? perMonthlyDelaying,
            int? nonProfitOverTimeH,
            string? eobFormula,
            string? overtimeFormula,
            string? holidayFormula,
            int? overtimeTransaction,
            int? hOvertimeTransaction,
            bool? policyCheckMachine,
            bool? hasAttendance,
            int? punishementCalc,
            int? onNoExit,
            int? deductionMethod,
            int? maxLoanAmtPCT,
            int? minServiceMonth,
            int? maxInstallementPCT,
            int? eosCostingTrns,
            int? ticketsCostingTrns,
            int? vacCostingTrns,
            int? hiCostingTrns,
            int? travalTrans,
            string? absentFormula,
            string? lateFormula,
            string? vacCostFormula,
            bool? hasFingerPrint,
            bool? hasOvertimeList,
            bool? attendanceFromTimeSheet,
            bool? hasFlexibleTime,
            bool? hasFlexableFingerPrint,
            bool? advanceBalance,
            bool? vacationTrans,
            int? vactionTransType,
            int? transValue,
            bool? addBalanceInAddEmp,
            bool? accumulatedBalance)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (noOfDaysPerPeriod.HasValue) NoOfDaysPerPeriod = noOfDaysPerPeriod.Value;
            if (workHoursPerDay.HasValue) WorkHoursPerDay = workHoursPerDay.Value;
            if (noOfHoursPerWeek.HasValue) NoOfHoursPerWeek = noOfHoursPerWeek.Value;
            if (noOfHoursPerPeriod.HasValue) NoOfHoursPerPeriod = noOfHoursPerPeriod.Value;
            if (overtimeFactor.HasValue) OvertimeFactor = overtimeFactor.Value;
            if (holidayFactor.HasValue) HolidayFactor = holidayFactor.Value;
            if (firstDayOfWeek.HasValue) FirstDayOfWeek = firstDayOfWeek.Value;
            if (defultStartTime.HasValue) DefultStartTime = defultStartTime.Value;
            if (defultEndTime.HasValue) DefultEndTime = defultEndTime.Value;
            if (workingUnitsIsHours.HasValue) WorkingUnitsIsHours = workingUnitsIsHours.Value;
            if (defaultProjectId.HasValue) DefaultProjectId = defaultProjectId.Value;
            if (remarks != null) Remarks = remarks;
            if (nonPermiLatTransaction.HasValue) NonPermiLatTransaction = nonPermiLatTransaction.Value;
            if (perDailyDelaying.HasValue) PerDailyDelaying = perDailyDelaying.Value;
            if (perMonthlyDelaying.HasValue) PerMonthlyDelaying = perMonthlyDelaying.Value;
            if (nonProfitOverTimeH.HasValue) NonProfitOverTimeH = nonProfitOverTimeH.Value;
            if (eobFormula != null) EOBFormula = eobFormula;
            if (overtimeFormula != null) OvertimeFormula = overtimeFormula;
            if (holidayFormula != null) HolidayFormula = holidayFormula;
            if (overtimeTransaction.HasValue) OvertimeTransaction = overtimeTransaction.Value;
            if (hOvertimeTransaction.HasValue) HOvertimeTransaction = hOvertimeTransaction.Value;
            if (policyCheckMachine.HasValue) PolicyCheckMachine = policyCheckMachine.Value;
            if (hasAttendance.HasValue) HasAttendance = hasAttendance.Value;
            if (punishementCalc.HasValue) PunishementCalc = punishementCalc.Value;
            if (onNoExit.HasValue) OnNoExit = onNoExit.Value;
            if (deductionMethod.HasValue) DeductionMethod = deductionMethod.Value;
            if (maxLoanAmtPCT.HasValue) MaxLoanAmtPCT = maxLoanAmtPCT.Value;
            if (minServiceMonth.HasValue) MinServiceMonth = minServiceMonth.Value;
            if (maxInstallementPCT.HasValue) MaxInstallementPCT = maxInstallementPCT.Value;
            if (eosCostingTrns.HasValue) EOSCostingTrns = eosCostingTrns.Value;
            if (ticketsCostingTrns.HasValue) TicketsCostingTrns = ticketsCostingTrns.Value;
            if (vacCostingTrns.HasValue) VacCostingTrns = vacCostingTrns.Value;
            if (hiCostingTrns.HasValue) HICostingTrns = hiCostingTrns.Value;
            if (travalTrans.HasValue) TravalTrans = travalTrans.Value;
            if (absentFormula != null) AbsentFormula = absentFormula;
            if (lateFormula != null) LateFormula = lateFormula;
            if (vacCostFormula != null) VacCostFormula = vacCostFormula;
            if (hasFingerPrint.HasValue) HasFingerPrint = hasFingerPrint.Value;
            if (hasOvertimeList.HasValue) HasOvertimeList = hasOvertimeList.Value;
            if (attendanceFromTimeSheet.HasValue) AttendanceFromTimeSheet = attendanceFromTimeSheet.Value;
            if (hasFlexibleTime.HasValue) HasFlexibleTime = hasFlexibleTime.Value;
            if (hasFlexableFingerPrint.HasValue) HasFlexableFingerPrint = hasFlexableFingerPrint.Value;
            if (advanceBalance.HasValue) AdvanceBalance = advanceBalance.Value;
            if (vacationTrans.HasValue) VacationTrans = vacationTrans.Value;
            if (vactionTransType.HasValue) VactionTransType = vactionTransType.Value;
            if (transValue.HasValue) TransValue = transValue.Value;
            if (addBalanceInAddEmp.HasValue) AddBalanceInAddEmp = addBalanceInAddEmp.Value;
            if (accumulatedBalance.HasValue) AccumulatedBalance = accumulatedBalance.Value;
        }

        public void AddDelay(EmployeeClassDelay delay)
        {
            _delays.Add(delay);
        }

        public void RemoveDelay(EmployeeClassDelay delay)
        {
            _delays.Remove(delay);
        }

        public void ClearDelays()
        {
            _delays.Clear();
        }

        public void AddVacation(EmployeeClassVacation vacation)
        {
            _vacations.Add(vacation);
        }

        public void RemoveVacation(EmployeeClassVacation vacation)
        {
            _vacations.Remove(vacation);
        }

        public void ClearVacations()
        {
            _vacations.Clear();
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId.Value;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}