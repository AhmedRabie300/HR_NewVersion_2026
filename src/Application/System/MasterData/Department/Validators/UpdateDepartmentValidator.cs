// Application/System/MasterData/Department/Validators/UpdateDepartmentValidator.cs
using Application.System.MasterData.Department.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Department.Validators
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Valid department ID is required");

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage("Remarks must not exceed 2048 characters");

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage("Parent ID must be greater than 0");

            RuleFor(x => x.CostCenterCode)
                .MaximumLength(50).When(x => x.CostCenterCode != null)
                .WithMessage("Cost center code must not exceed 50 characters");

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("At least one field must be provided for update");
        }

        private bool HaveAtLeastOneField(UpdateDepartmentDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.ParentId.HasValue ||
                   dto.Remarks != null ||
                   dto.CostCenterCode != null;
        }
    }
}