// Application/UARbac/Users/Dtos/GetUserDto.cs
namespace Application.UARbac.Users.Dtos
{
    public sealed record GetUserDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsAdmin,
        bool? IsArabic,
        bool? CanChangePassword,
        bool? ResetSearchCriteria,
        bool? ResetReportCriteria,
        byte? SessionIdleTime,
        bool? EnforceAlphaNumericPwd,
        DateTime? PasswordExpiry,
        DateTime? PasswordChangedOn,
        string? Remarks,
        int? RegComputerId,
        DateTime RegDate,
        DateTime? CancelDate,
        int? RelEmployee,
        int? LevelId,
        string? DeviceToken,
        bool? IsActive
    );
}