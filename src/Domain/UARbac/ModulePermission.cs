 using Domain.Common;

namespace Domain.UARbac
{
    public class ModulePermission : LegacyEntity
    {
         public int ModuleId { get; private set; }
        public int? GroupId { get; private set; }
        public int? UserId { get; private set; }

         public bool? CanView { get; private set; }

         public int? RegUserId { get; private set; }
        public string? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

         public Module? Module { get; private set; }
        public Group? Group { get; private set; }
        public Users? User { get; private set; }

        private ModulePermission() { }  

        public ModulePermission(
            int moduleId,
            int? groupId,
            int? userId,
            bool? canView,
            int? regUserId,
            string? regComputerId)
        {
             if (!groupId.HasValue && !userId.HasValue)
                throw new ArgumentException("Either GroupId or UserId must be provided");

            if (groupId.HasValue && userId.HasValue)
                throw new ArgumentException("Cannot provide both GroupId and UserId");

            ModuleId = moduleId;
            GroupId = groupId;
            UserId = userId;
            CanView = canView;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

         public void UpdatePermission(bool? canView)
        {
            if (canView.HasValue)
                CanView = canView;
        }

         public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue)
                RegUserId = regUserId;
        }

         public void Restore()
        {
            CancelDate = null;
        }

         public bool IsActive() => !CancelDate.HasValue;

         public bool IsForGroup() => GroupId.HasValue;

         public bool IsForUser() => UserId.HasValue;
    }
}