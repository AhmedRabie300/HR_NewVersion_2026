using Domain.Common;
using Domain.System.HRS.Employees;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.EmployeesItems
{
    public class EmployeeItem : LegacyEntity, ICompanyScoped
    {
        public int EmployeeId { get; private set; }
        public int ItemId { get; private set; }
        public DateTime? ReceivedDate { get; private set; }
        public DateTime? ReturnedDate { get; private set; }
        public string? ReceivingItemStatus { get; private set; }
        public string? ReturningItemStatus { get; private set; }
        public bool? IsFromAssets { get; private set; }
        public bool? IsConfirmed { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public Employee? Employee { get; private set; }
        public Item? Item { get; private set; }
        public Company? Company { get; private set; }

        private EmployeeItem() { }

        public EmployeeItem(
            int employeeId,
            int itemId,
             DateTime? receivedDate = null,
            DateTime? returnedDate = null,
            string? receivingItemStatus = null,
            string? returningItemStatus = null,
            bool? isFromAssets = null,
            bool? isConfirmed = null,
            string? remarks = null )
        {
            EmployeeId = employeeId;
            ItemId = itemId;
             ReceivedDate = receivedDate;
            ReturnedDate = returnedDate;
            ReceivingItemStatus = receivingItemStatus;
            ReturningItemStatus = returningItemStatus;
            IsFromAssets = isFromAssets;
            IsConfirmed = isConfirmed;
            Remarks = remarks;
             RegDate = DateTime.UtcNow;
        }

        public void Update(
            DateTime? receivedDate = null,
            DateTime? returnedDate = null,
            string? receivingItemStatus = null,
            string? returningItemStatus = null,
            bool? isFromAssets = null,
            bool? isConfirmed = null,
            string? remarks = null)
        {
            if (receivedDate.HasValue) ReceivedDate = receivedDate.Value;
            if (returnedDate.HasValue) ReturnedDate = returnedDate.Value;
            if (receivingItemStatus != null) ReceivingItemStatus = receivingItemStatus;
            if (returningItemStatus != null) ReturningItemStatus = returningItemStatus;
            if (isFromAssets.HasValue) IsFromAssets = isFromAssets.Value;
            if (isConfirmed.HasValue) IsConfirmed = isConfirmed.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void Confirm()
        {
            IsConfirmed = true;
        }

        public void Return(DateTime? returnedDate = null, string? returningItemStatus = null)
        {
            ReturnedDate = returnedDate ?? DateTime.Now;
            if (returningItemStatus != null) ReturningItemStatus = returningItemStatus;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
        public bool IsReturned => ReturnedDate.HasValue;
        public bool IsReceived => ReceivedDate.HasValue && !ReturnedDate.HasValue;
    }
}