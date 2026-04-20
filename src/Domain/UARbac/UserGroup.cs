// Domain/UARbac/UserGroup.cs
using Domain.Common;

namespace Domain.UARbac
{
    public class UserGroup : LegacyEntity
    {
        public int UserId { get; private set; }
        public int GroupId { get; private set; }
          public int? RegUserId { get; private set; }

        // Navigation properties
        public Users? User { get; private set; }
        public Group? Group { get; private set; }

        private UserGroup() { } // For EF Core

        public UserGroup(int userId, int groupId, bool isPrimary = false, int? regUserId = null)
        {
            UserId = userId;
            GroupId = groupId;
          
            RegUserId = regUserId;
            RegDate = DateTime.Now;
        }

        public void SetPrimary(bool isPrimary)
        {
         }

        public void UpdateRegUser(int? regUserId)
        {
            if (regUserId.HasValue)
                RegUserId = regUserId;
        }
    }
}