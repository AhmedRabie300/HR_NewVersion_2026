using Domain.Common;

namespace Domain.UARbac
{
    public class Users : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? Password { get; private set; }
        public bool? IsAdmin { get; private set; }
        public bool? IsArabic { get; private set; }
        public bool? CanChangePassword { get; private set; }
        public bool? ResetSearchCriteria { get; private set; }
        public bool? ResetReportCriteria { get; private set; }
        public byte? SessionIdleTime { get; private set; }
        public bool? EnforceAlphaNumericPwd { get; private set; }
        public DateTime? PasswordExpiry { get; private set; }
        public DateTime? PasswordChangedOn { get; private set; }
        public string? Remarks { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? RelEmployee { get; private set; }
        public int? LevelId { get; private set; }
        public string? DeviceToken { get; private set; }
        public bool? IsActive { get; private set; }

        public Users() { } // For EF Core

        public Users(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? password,
            bool? isAdmin = null,
            bool? isArabic = null,
            bool? canChangePassword = null,
            bool? resetSearchCriteria = null,
            bool? resetReportCriteria = null,
            byte? sessionIdleTime = null,
            bool? enforceAlphaNumericPwd = null,
            DateTime? passwordExpiry = null,
            string? remarks = null,
            int? relEmployee = null,
            int? levelId = null,
            string? deviceToken = null,
            bool? isActive = true)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            Password = password;
            IsAdmin = isAdmin;
            IsArabic = isArabic;
            CanChangePassword = canChangePassword;
            ResetSearchCriteria = resetSearchCriteria;
            ResetReportCriteria = resetReportCriteria;
            SessionIdleTime = sessionIdleTime;
            EnforceAlphaNumericPwd = enforceAlphaNumericPwd;
            PasswordExpiry = passwordExpiry;
            PasswordChangedOn = DateTime.UtcNow;
            Remarks = remarks;
            RelEmployee = relEmployee;
            LevelId = levelId;
            DeviceToken = deviceToken;
            IsActive = isActive;
            RegDate = DateTime.UtcNow;
        }

        // Update methods
        public void UpdatePersonalInfo(string? engName, string? arbName, string? arbName4S, int? relEmployee)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (relEmployee != null) RelEmployee = relEmployee;
        }

        public void SetPassword(string newPassword)
        {
            Password = newPassword;
            PasswordChangedOn = DateTime.UtcNow;
            PasswordExpiry = DateTime.UtcNow.AddDays(90);
        }

        public void UpdateSettings(bool? isArabic, bool? canChangePassword, byte? sessionIdleTime, bool? enforceAlphaNumericPwd)
        {
            if (isArabic != null) IsArabic = isArabic;
            if (canChangePassword != null) CanChangePassword = canChangePassword;
            if (sessionIdleTime != null) SessionIdleTime = sessionIdleTime;
            if (enforceAlphaNumericPwd != null) EnforceAlphaNumericPwd = enforceAlphaNumericPwd;
        }

        public void UpdatePermissions(bool? isAdmin, bool? resetSearchCriteria, bool? resetReportCriteria)
        {
            if (isAdmin != null) IsAdmin = isAdmin;
            if (resetSearchCriteria != null) ResetSearchCriteria = resetSearchCriteria;
            if (resetReportCriteria != null) ResetReportCriteria = resetReportCriteria;
        }

        public void UpdateRemarks(string? remarks)
        {
            if (remarks != null) Remarks = remarks;
        }

        public void UpdateDeviceToken(string? deviceToken)
        {
            if (deviceToken != null) DeviceToken = deviceToken;
        }

        public void UpdateLevel(int? levelId)
        {
            if (levelId != null) LevelId = levelId;
        }

        public void Activate()
        {
            IsActive = true;
            CancelDate = null;
        }

        public void Deactivate(DateTime? cancelDate = null)
        {
            IsActive = false;
            CancelDate = cancelDate ?? DateTime.UtcNow;
        }
    }
}