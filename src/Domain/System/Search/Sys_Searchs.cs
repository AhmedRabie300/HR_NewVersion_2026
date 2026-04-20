using Domain.Common;

namespace Domain.System.Search
{
    public class Sys_Searchs : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public int ObjectID { get; private set; }
        public int? RegUserID { get; private set; }
        public string? RegComputerID { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Sys_Objects? Object { get; private set; }
        private readonly List<Sys_SearchsColumns> _columns = new();
        public IReadOnlyCollection<Sys_SearchsColumns> Columns => _columns.AsReadOnly();

        private Sys_Searchs() { }

        public Sys_Searchs(
            string code,
            string? engName,
            string? arbName,
            int objectID,
            int? regUserID,
            string? regComputerID)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ObjectID = objectID;
            RegUserID = regUserID;
            RegComputerID = regComputerID;
            RegDate = DateTime.Now;
        }

        public void Update(string? engName, string? arbName)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
        }

        public void AddColumn(Sys_SearchsColumns column)
        {
            if (!_columns.Any(x => x.Id == column.Id))
                _columns.Add(column);
        }

        public void Cancel(int? regUserID = null)
        {
            CancelDate = DateTime.Now;
            if (regUserID.HasValue) RegUserID = regUserID;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}