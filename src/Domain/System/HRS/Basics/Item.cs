using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics
{
    public class Item : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public DateTime? PurchaseDate { get; private set; }
        public decimal? PurchasePrice { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public string? LicenseNumber { get; private set; }
        public int CompanyId { get; private set; }
        public bool? IsFromAssets { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation property
        public Company? Company { get; private set; }

        private Item() { }

        public Item(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            DateTime? purchaseDate,
            decimal? purchasePrice,
            DateTime? expiryDate,
            string? licenseNumber,
            int companyId,
            bool? isFromAssets,
            string? remarks,
            int? regComputerId = null)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
            ExpiryDate = expiryDate;
            LicenseNumber = licenseNumber;
            CompanyId = companyId;
            IsFromAssets = isFromAssets;
            Remarks = remarks;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            DateTime? purchaseDate,
            decimal? purchasePrice,
            DateTime? expiryDate,
            string? licenseNumber,
            bool? isFromAssets,
            string? remarks)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (purchaseDate.HasValue) PurchaseDate = purchaseDate.Value;
            if (purchasePrice.HasValue) PurchasePrice = purchasePrice.Value;
            if (expiryDate.HasValue) ExpiryDate = expiryDate.Value;
            if (licenseNumber != null) LicenseNumber = licenseNumber;
            if (isFromAssets.HasValue) IsFromAssets = isFromAssets.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}