namespace Application.UARbac.UserGroups.Dtos
{
    public sealed record AddUserToGroupDto(
        int UserId,      // ID المستخدم
        int GroupId,     // ID المجموعة
        bool IsPrimary = false  // هل هي المجموعة الأساسية؟
    );
}