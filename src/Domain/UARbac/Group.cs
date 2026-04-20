using Domain.Common;

namespace Domain.UARbac
{
    public class Group : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? ArbName { get; private set; }
        public string? EngName { get; private set; }
        public int? RegUserId { get; private set; }
        public DateTime? CancelDate { get; private set; }

         private readonly List<UserGroup> _groupUsers = new();
        public IReadOnlyCollection<UserGroup> GroupUsers => _groupUsers.AsReadOnly();

        private Group() { }  

        public Group(string code, string? engName, string? arbName, int? regUserId = null)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            RegUserId = regUserId;
            RegDate = DateTime.Now;
        }

        public void Update(string? engName, string? arbName)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

         public void AddUser(int userId, bool isPrimary = false)
        {
            if (!_groupUsers.Any(x => x.UserId == userId))
            {
                _groupUsers.Add(new UserGroup(userId, Id, isPrimary));
            }
        }

        public void RemoveUser(int userId)
        {
            var userGroup = _groupUsers.FirstOrDefault(x => x.UserId == userId);
            if (userGroup != null)
            {
                _groupUsers.Remove(userGroup);
            }
        }

        public bool HasUser(int userId)
        {
            return _groupUsers.Any(x => x.UserId == userId);
        }

        public int UsersCount => _groupUsers.Count;
    }
}