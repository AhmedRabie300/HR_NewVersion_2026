using Domain.Common;

namespace Domain.System.Search
{
    public class Sys_Fields : LegacyEntity
    {
        public int ObjectID { get; private set; }
        public string FieldName { get; private set; } = null!;
        public short? FieldType { get; private set; }
        public short? FieldLength { get; private set; }
        public int? SysColumns_OrderID { get; private set; }
        public int? ViewObjectID { get; private set; }
        public int? ViewEngFieldID { get; private set; }
        public int? ViewArbFieldID { get; private set; }
        public int? RegUserId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Sys_Objects? Object { get; private set; }
        private readonly List<Sys_SearchsColumns> _searchColumns = new();
        public IReadOnlyCollection<Sys_SearchsColumns> SearchColumns => _searchColumns.AsReadOnly();

        private Sys_Fields() { }

        public Sys_Fields(
            int objectID,
            string fieldName,
            short? fieldType,
            short? fieldLength,
            int? sysColumns_OrderID,
            int? viewObjectID,
            int? viewEngFieldID,
            int? viewArbFieldID,
            int? regUserId)
        {
            ObjectID = objectID;
            FieldName = fieldName;
            FieldType = fieldType;
            FieldLength = fieldLength;
            SysColumns_OrderID = sysColumns_OrderID;
            ViewObjectID = viewObjectID;
            ViewEngFieldID = viewEngFieldID;
            ViewArbFieldID = viewArbFieldID;
            RegDate = DateTime.Now;
            RegUserId = regUserId;
        }

        public void Update(
            string? fieldName,
            short? fieldType,
            short? fieldLength,
            int? sysColumns_OrderID,
            int? viewObjectID,
            int? viewEngFieldID,
            int? viewArbFieldID)
        {
            if (fieldName != null) FieldName = fieldName;
            if (fieldType.HasValue) FieldType = fieldType;
            if (fieldLength.HasValue) FieldLength = fieldLength;
            if (sysColumns_OrderID.HasValue) SysColumns_OrderID = sysColumns_OrderID;
            if (viewObjectID.HasValue) ViewObjectID = viewObjectID;
            if (viewEngFieldID.HasValue) ViewEngFieldID = viewEngFieldID;
            if (viewArbFieldID.HasValue) ViewArbFieldID = viewArbFieldID;
        }

     

        public bool IsActive() => !CancelDate.HasValue;
    }
}