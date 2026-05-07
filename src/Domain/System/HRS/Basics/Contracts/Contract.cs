using Domain.Common;
using Domain.System.HRS.Basics.ContractsTypes;
using Domain.System.HRS.Basics.GradesAndClasses;
using Domain.System.HRS.Employees;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.Contracts
{
    public class Contract : LegacyEntity, ICompanyScoped
    {
        public int Number { get; private set; }
        public int ContractTypeId { get; private set; }
        public int EmployeeClassId { get; private set; }
        public int EmployeeId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public int? ProfessionId { get; private set; }
        public int? PositionId { get; private set; }
        public int? GradeStepId { get; private set; }
        public int? CurrencyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? ContractPeriod { get; private set; }
        public int? UpdatedUserId { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public ContractsType? ContractType { get; private set; }
        public EmployeeClass? EmployeeClass { get; private set; }
        public Employee? Employee { get; private set; }
        public Profession? Profession { get; private set; }
        public Position? Position { get; private set; }
        public GradeStep? GradeStep { get; private set; }
        public Currency? Currency { get; private set; }

        private readonly List<ContractTransaction> _transactions = new();
        public IReadOnlyCollection<ContractTransaction> Transactions => _transactions.AsReadOnly();

        private Contract() { }

        public Contract(
            int number,
            int contractTypeId,
            int employeeClassId,
            int employeeId,
            DateTime startDate,
             int? professionId = null,
            int? positionId = null,
            int? gradeStepId = null,
            int? currencyId = null,
            DateTime? endDate = null,
            string? remarks = null,
             int? contractPeriod = null)
        {
            Number = number;
            ContractTypeId = contractTypeId;
            EmployeeClassId = employeeClassId;
            EmployeeId = employeeId;
            StartDate = startDate;
             ProfessionId = professionId;
            PositionId = positionId;
            GradeStepId = gradeStepId;
            CurrencyId = currencyId;
            EndDate = endDate;
            Remarks = remarks;
             ContractPeriod = contractPeriod;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            int? contractTypeId = null,
            int? employeeClassId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? professionId = null,
            int? positionId = null,
            int? gradeStepId = null,
            int? currencyId = null,
            string? remarks = null,
            int? contractPeriod = null)
        {
            if (contractTypeId.HasValue) ContractTypeId = contractTypeId.Value;
            if (employeeClassId.HasValue) EmployeeClassId = employeeClassId.Value;
            if (startDate.HasValue) StartDate = startDate.Value;
            if (endDate.HasValue) EndDate = endDate.Value;
            if (professionId.HasValue) ProfessionId = professionId.Value;
            if (positionId.HasValue) PositionId = positionId.Value;
            if (gradeStepId.HasValue) GradeStepId = gradeStepId.Value;
            if (currencyId.HasValue) CurrencyId = currencyId.Value;
            if (remarks != null) Remarks = remarks;
            if (contractPeriod.HasValue) ContractPeriod = contractPeriod.Value;
            UpdatedUserId = RegUserId;
            UpdateDate = DateTime.Now;
        }

        public void AddTransaction(ContractTransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void RemoveTransaction(ContractTransaction transaction)
        {
            _transactions.Remove(transaction);
        }

        public void ClearTransactions()
        {
            _transactions.Clear();
        }

        public void Cancel( )
        {
            CancelDate = DateTime.Now;
           
        }

        public bool IsActive() => !CancelDate.HasValue;
        public bool IsCurrent => StartDate <= DateTime.Now && (!EndDate.HasValue || EndDate.Value >= DateTime.Now);
    }
}