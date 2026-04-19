using Domain.Common;

namespace Domain.System.MasterData
{
    public class Currency : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? EngSymbol { get; private set; }
        public string? ArbSymbol { get; private set; }
        public int DecimalFraction { get; private set; }
        public string? DecimalEngName { get; private set; }
        public string? DecimalArbName { get; private set; }
        public decimal? Amount { get; private set; }
        public int? NoDecimalPlaces { get; private set; }
        public int? CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        public Company? Company { get; private set; }

        private Currency() { }

        public Currency(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? engSymbol,
            string? arbSymbol,
            int decimalFraction,
            string? decimalEngName,
            string? decimalArbName,
            decimal? amount,
            int? noDecimalPlaces,
            string? remarks)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            EngSymbol = engSymbol;
            ArbSymbol = arbSymbol;
            DecimalFraction = decimalFraction;
            DecimalEngName = decimalEngName;
            DecimalArbName = decimalArbName;
            Amount = amount;
            NoDecimalPlaces = noDecimalPlaces;
            Remarks = remarks;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? engSymbol,
            string? arbSymbol,
            int? decimalFraction,
            string? decimalEngName,
            string? decimalArbName,
            decimal? amount,
            int? noDecimalPlaces,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (engSymbol != null) EngSymbol = engSymbol;
            if (arbSymbol != null) ArbSymbol = arbSymbol;
            if (decimalFraction.HasValue) DecimalFraction = decimalFraction.Value;
            if (decimalEngName != null) DecimalEngName = decimalEngName;
            if (decimalArbName != null) DecimalArbName = decimalArbName;
            if (amount.HasValue) Amount = amount.Value;
            if (noDecimalPlaces.HasValue) NoDecimalPlaces = noDecimalPlaces.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}