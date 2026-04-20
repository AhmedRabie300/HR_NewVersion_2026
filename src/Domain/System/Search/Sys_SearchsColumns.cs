using Domain.Common;

namespace Domain.System.Search
{
    public class Sys_SearchsColumns : LegacyEntity
    {
        public int SearchID { get; private set; }
        public int FieldID { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public bool? IsCriteria { get; private set; }
        public bool? IsView { get; private set; }
        public short? InputLength { get; private set; }
        public bool? IsArabic { get; private set; }
        public byte? Alignment { get; private set; }
        public int? Rank { get; private set; }
        public int? SubSearchID { get; private set; }
        public int? RegUserID { get; private set; }
        public string? RegComputerID { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? RankCriteria { get; private set; }
        public int? RankView { get; private set; }
        public bool? IsTarget { get; private set; }

        // Navigation properties
        public Sys_Searchs? SearchDefinition { get; private set; }
        public Sys_Fields? Field { get; private set; }
        public Sys_Searchs? SubSearch { get; private set; }

        private Sys_SearchsColumns() { }

        public Sys_SearchsColumns(
            int searchID,
            int fieldID,
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isCriteria,
            bool? isView,
            short? inputLength,
            bool? isArabic,
            byte? alignment,
            int? rank,
            int? subSearchID,
            int? regUserID,
            string? regComputerID,
            int? rankCriteria,
            int? rankView,
            bool? isTarget)
        {
            SearchID = searchID;
            FieldID = fieldID;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            IsCriteria = isCriteria;
            IsView = isView;
            InputLength = inputLength;
            IsArabic = isArabic;
            Alignment = alignment;
            Rank = rank;
            SubSearchID = subSearchID;
            RegUserID = regUserID;
            RegComputerID = regComputerID;
            RankCriteria = rankCriteria;
            RankView = rankView;
            IsTarget = isTarget;
            RegDate = DateTime.Now;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isCriteria,
            bool? isView,
            short? inputLength,
            bool? isArabic,
            byte? alignment,
            int? rank,
            int? subSearchID,
            int? rankCriteria,
            int? rankView,
            bool? isTarget)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (isCriteria.HasValue) IsCriteria = isCriteria;
            if (isView.HasValue) IsView = isView;
            if (inputLength.HasValue) InputLength = inputLength;
            if (isArabic.HasValue) IsArabic = isArabic;
            if (alignment.HasValue) Alignment = alignment;
            if (rank.HasValue) Rank = rank;
            if (subSearchID.HasValue) SubSearchID = subSearchID;
            if (rankCriteria.HasValue) RankCriteria = rankCriteria;
            if (rankView.HasValue) RankView = rankView;
            if (isTarget.HasValue) IsTarget = isTarget;
        }

        public void Cancel(int? regUserID = null)
        {
            CancelDate = DateTime.Now;
            if (regUserID.HasValue) RegUserID = regUserID;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}