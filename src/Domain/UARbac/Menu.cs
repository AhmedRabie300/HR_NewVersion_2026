 
using Domain.Common;

namespace Domain.UARbac
{
    public class Menu : LegacyEntity
    {
        public string? Code { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? ParentId { get; private set; }
        public string? Shortcut { get; private set; }
        public int? Rank { get; private set; }
        public int? FormId { get; private set; }
        public int? ObjectId { get; private set; }
        public int? ViewFormId { get; private set; }
        public bool? IsHide { get; private set; }
        public string? Image { get; private set; }
        public int? ViewType { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Menu? Parent { get; private set; }
  

        private readonly List<Menu> _children = new();
        public IReadOnlyCollection<Menu> Children => _children.AsReadOnly();

        private Menu() { } // For EF Core

        public Menu(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? parentId,
            string? shortcut,
            int? rank,
            int? formId,
            int? objectId,
            int? viewFormId,
            bool? isHide,
            string? image,
            int? viewType,
            int? regUserId,
            int? regComputerId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            ParentId = parentId;
            Shortcut = shortcut;
            Rank = rank;
            FormId = formId;
            ObjectId = objectId;
            ViewFormId = viewFormId;
            IsHide = isHide;
            Image = image;
            ViewType = viewType;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            RegDate = DateTime.Now;
        }

        // Update methods
        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? shortcut,
            int? rank,
            string? image,
            int? viewType)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (shortcut != null) Shortcut = shortcut;
            if (rank.HasValue) Rank = rank;
            if (image != null) Image = image;
            if (viewType.HasValue) ViewType = viewType;
        }

        public void UpdateRelations(
            int? parentId,
            int? formId,
            int? objectId,
            int? viewFormId)
        {
            if (parentId.HasValue) ParentId = parentId;
            if (formId.HasValue) FormId = formId;
            if (objectId.HasValue) ObjectId = objectId;
            if (viewFormId.HasValue) ViewFormId = viewFormId;
        }

        public void UpdateVisibility(bool? isHide)
        {
            if (isHide.HasValue) IsHide = isHide;
        }

        public void AddChild(Menu child)
        {
            if (!_children.Any(x => x.Id == child.Id))
            {
                _children.Add(child);
            }
        }

        public void RemoveChild(int childId)
        {
            var child = _children.FirstOrDefault(x => x.Id == childId);
            if (child != null)
            {
                _children.Remove(child);
            }
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }
    }
}