using Domain.Common;

namespace Domain.System.MasterData
{
    public class Position : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? ParentId { get; private set; }
        public int? PositionLevelId { get; private set; }

        public int? EvalRecruitmentId { get; private set; }
        public int? EvalEvaluationID { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? EmployeesNo { get; private set; }
        public bool? ApplyValidation { get; private set; }
        public string? PositionBudget { get; private set; }
        public int? AppraisalTypeGroupId { get; private set; }

        // Navigation properties
        public Position? ParentPosition { get; private set; }
        public ICollection<Position>? ChildPositions { get; private set; }
        // public PositionLevel? PositionLevel { get; private set; } // هتضاف بعدين

        private Position() { } // For EF Core

        public Position(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? parentId,
            int? positionLevelId,
            string? remarks,
            int? employeesNo,
            bool? applyValidation,
            string? positionBudget,
            int? appraisalTypeGroupId,
            int? regUserId,
            int? regComputerId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            ParentId = parentId;
            PositionLevelId = positionLevelId;
             Remarks = remarks;
            EmployeesNo = employeesNo;
            ApplyValidation = applyValidation;
            PositionBudget = positionBudget;
            AppraisalTypeGroupId = appraisalTypeGroupId;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        // Update methods
        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
        }

        public void UpdateRelations(
            int? parentId,
            int? positionLevelId,
            int? evalRecruitmentId,
            int? appraisalTypeGroupId)
        {
            if (parentId.HasValue) ParentId = parentId;
            if (positionLevelId.HasValue) PositionLevelId = positionLevelId;
             if (appraisalTypeGroupId.HasValue) AppraisalTypeGroupId = appraisalTypeGroupId;
        }

        public void UpdateSettings(
            int? employeesNo,
            bool? applyValidation,
            string? positionBudget)
        {
            if (employeesNo.HasValue) EmployeesNo = employeesNo;
            if (applyValidation.HasValue) ApplyValidation = applyValidation;
            if (positionBudget != null) PositionBudget = positionBudget;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }
        // Domain/System/MasterData/Position.cs
        // أضف هذه الدوال داخل الكلاس

        public void UpdateParent(int? parentId)
        {
            if (parentId.HasValue && parentId != Id)
                ParentId = parentId;
        }

        public void UpdatePositionLevel(int? positionLevelId)
        {
            if (positionLevelId.HasValue)
                PositionLevelId = positionLevelId;
        }

        public void UpdateEmployeesNo(int? employeesNo)
        {
            if (employeesNo.HasValue)
                EmployeesNo = employeesNo;
        }

        public void UpdatePositionBudget(string? positionBudget)
        {
            if (positionBudget != null)
                PositionBudget = positionBudget;
        }
        public bool IsActive() => !CancelDate.HasValue;
    }
}