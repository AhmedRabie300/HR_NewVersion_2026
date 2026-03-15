namespace Application.UARbac.Users.Dtos
{
    public sealed record CreateUserDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Password,
        bool? IsAdmin,
        bool? IsArabic,
        bool? CanChangePassword,
        bool? ResetSearchCriteria,
        bool? ResetReportCriteria,
        byte? SessionIdleTime,
        bool? EnforceAlphaNumericPwd,
        DateTime? PasswordExpiry,
        string? Remarks,
        int? RelEmployee,
        int? LevelId,
        string? DeviceToken,
        bool? IsActive,
        List<int>? GroupIds
    );
}