using Domain.Common;

namespace Domain.System.Search
{
    public class Sys_Objects : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public bool? IsFiscalYearClosable { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        private readonly List<Sys_Fields> _fields = new();
        public IReadOnlyCollection<Sys_Fields> Fields => _fields.AsReadOnly();

        private readonly List<Sys_Searchs> _searchDefinitions = new();
        public IReadOnlyCollection<Sys_Searchs> SearchDefinitions => _searchDefinitions.AsReadOnly();

        private Sys_Objects() { }

        public Sys_Objects(string code, string? engName, string? arbName, bool? isFiscalYearClosable)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            IsFiscalYearClosable = isFiscalYearClosable;
            RegDate = DateTime.UtcNow;
        }

        public void Update(string? engName, string? arbName, bool? isFiscalYearClosable)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (isFiscalYearClosable.HasValue) IsFiscalYearClosable = isFiscalYearClosable;
        }

        public void AddField(Sys_Fields field)
        {
            if (!_fields.Any(x => x.Id == field.Id))
                _fields.Add(field);
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}