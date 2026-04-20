 using Domain.Common;
 
namespace Domain.UARbac
{
    public class Form : LegacyEntity
    {
        public string? Code { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? EngDescription { get; private set; }
        public string? ArbDescription { get; private set; }
        public int? Rank { get; private set; }
        public int ModuleId { get; private set; }
        public int? SearchFormId { get; private set; }
        public int? Height { get; private set; }
        public int? Width { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public string? Layout { get; private set; }
        public string? LinkTarget { get; private set; }
        public string? LinkUrl { get; private set; }
        public string? ImageUrl { get; private set; }
        public int? MainId { get; private set; }

        // Navigation properties
     //   public Module? Module { get; private set; }
        public Form? SearchForm { get; private set; }
        public Form? MainForm { get; private set; }

        private readonly List<FormPermission> _formPermissions = new();
        public IReadOnlyCollection<FormPermission> FormPermissions => _formPermissions.AsReadOnly();

        private Form() { } // For EF Core

        public Form(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? engDescription,
            string? arbDescription,
            int? rank,
            int moduleId,
            int? searchFormId,
            int? height,
            int? width,
            string? remarks,
            int? regUserId,
            int? regComputerId,
            string? layout,
            string? linkTarget,
            string? linkUrl,
            string? imageUrl,
            int? mainId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            EngDescription = engDescription;
            ArbDescription = arbDescription;
            Rank = rank;
            ModuleId = moduleId;
            SearchFormId = searchFormId;
            Height = height;
            Width = width;
            Remarks = remarks;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            Layout = layout;
            LinkTarget = linkTarget;
            LinkUrl = linkUrl;
            ImageUrl = imageUrl;
            MainId = mainId;
            RegDate = DateTime.Now;
        }

        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? engDescription,
            string? arbDescription,
            int? rank,
            int? height,
            int? width,
            string? remarks,
            string? layout,
            string? linkTarget,
            string? linkUrl,
            string? imageUrl)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (engDescription != null) EngDescription = engDescription;
            if (arbDescription != null) ArbDescription = arbDescription;
            if (rank.HasValue) Rank = rank;
            if (height.HasValue) Height = height;
            if (width.HasValue) Width = width;
            if (remarks != null) Remarks = remarks;
            if (layout != null) Layout = layout;
            if (linkTarget != null) LinkTarget = linkTarget;
            if (linkUrl != null) LinkUrl = linkUrl;
            if (imageUrl != null) ImageUrl = imageUrl;
        }

        public void UpdateRelations(
            int? searchFormId,
            int? mainId)
        {
            if (searchFormId.HasValue) SearchFormId = searchFormId;
            if (mainId.HasValue) MainId = mainId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }
    }
}