using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.FiscalTransactions
{
    public class TransactionsType : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? ShortEngName { get; private set; }
        public string? ShortArbName { get; private set; }
        public string? ShortArbName4S { get; private set; }
        public int TransactionGroupId { get; private set; }
        public short Sign { get; private set; }
        public string? DebitAccountCode { get; private set; }
        public string? CreditAccountCode { get; private set; }
        public bool? IsPaid { get; private set; }
        public string? Formula { get; private set; }
        public string? BeginContractFormula { get; private set; }
        public string? EndContractFormula { get; private set; }
        public bool? InputIsNumeric { get; private set; }
        public bool? IsEndOfService { get; private set; }
        public bool? IsSalaryEOSExeclude { get; private set; }
        public bool? IsProjectRelatedItem { get; private set; }
        public bool? IsBasicSalary { get; private set; }
        public bool? IsDistributable { get; private set; }
        public bool? IsAllowPosting { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public bool? HasInsuranceTiers { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public TransactionsGroup? TransactionGroup { get; private set; }

        private TransactionsType() { }

        public TransactionsType(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? shortEngName,
            string? shortArbName,
            string? shortArbName4S,
            int transactionGroupId,
            short sign,
            string? debitAccountCode,
            string? creditAccountCode,
            bool? isPaid,
            string? formula,
            string? beginContractFormula,
            string? endContractFormula,
            bool? inputIsNumeric,
            bool? isEndOfService,
            bool? isSalaryEOSExeclude,
            bool? isProjectRelatedItem,
            bool? isBasicSalary,
            bool? isDistributable,
            bool? isAllowPosting,
            int companyId,
            int regUserId,
            int? regComputerId,
            string? remarks,
            bool? hasInsuranceTiers)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            ShortEngName = shortEngName;
            ShortArbName = shortArbName;
            ShortArbName4S = shortArbName4S;
            TransactionGroupId = transactionGroupId;
            Sign = sign;
            DebitAccountCode = debitAccountCode;
            CreditAccountCode = creditAccountCode;
            IsPaid = isPaid;
            Formula = formula;
            BeginContractFormula = beginContractFormula;
            EndContractFormula = endContractFormula;
            InputIsNumeric = inputIsNumeric;
            IsEndOfService = isEndOfService;
            IsSalaryEOSExeclude = isSalaryEOSExeclude;
            IsProjectRelatedItem = isProjectRelatedItem;
            IsBasicSalary = isBasicSalary;
            IsDistributable = isDistributable;
            IsAllowPosting = isAllowPosting;
            CompanyId = companyId;
            Remarks = remarks;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            HasInsuranceTiers = hasInsuranceTiers;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? shortEngName,
            string? shortArbName,
            string? shortArbName4S,
            int? transactionGroupId,
            short? sign,
            string? debitAccountCode,
            string? creditAccountCode,
            bool? isPaid,
            string? formula,
            string? beginContractFormula,
            string? endContractFormula,
            bool? inputIsNumeric,
            bool? isEndOfService,
            bool? isSalaryEOSExeclude,
            bool? isProjectRelatedItem,
            bool? isBasicSalary,
            bool? isDistributable,
            bool? isAllowPosting,
            string? remarks,
            bool? hasInsuranceTiers)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (shortEngName != null) ShortEngName = shortEngName;
            if (shortArbName != null) ShortArbName = shortArbName;
            if (shortArbName4S != null) ShortArbName4S = shortArbName4S;
            if (transactionGroupId.HasValue) TransactionGroupId = transactionGroupId.Value;
            if (sign.HasValue) Sign = sign.Value;
            if (debitAccountCode != null) DebitAccountCode = debitAccountCode;
            if (creditAccountCode != null) CreditAccountCode = creditAccountCode;
            if (isPaid.HasValue) IsPaid = isPaid;
            if (formula != null) Formula = formula;
            if (beginContractFormula != null) BeginContractFormula = beginContractFormula;
            if (endContractFormula != null) EndContractFormula = endContractFormula;
            if (inputIsNumeric.HasValue) InputIsNumeric = inputIsNumeric;
            if (isEndOfService.HasValue) IsEndOfService = isEndOfService;
            if (isSalaryEOSExeclude.HasValue) IsSalaryEOSExeclude = isSalaryEOSExeclude;
            if (isProjectRelatedItem.HasValue) IsProjectRelatedItem = isProjectRelatedItem;
            if (isBasicSalary.HasValue) IsBasicSalary = isBasicSalary;
            if (isDistributable.HasValue) IsDistributable = isDistributable;
            if (isAllowPosting.HasValue) IsAllowPosting = isAllowPosting;
            if (remarks != null) Remarks = remarks;
            if (hasInsuranceTiers.HasValue) HasInsuranceTiers = hasInsuranceTiers;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId.Value;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}