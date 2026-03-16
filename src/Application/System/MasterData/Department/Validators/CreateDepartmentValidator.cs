// Application/System/MasterData/Department/Validators/CreateDepartmentValidator.cs
using Application.System.MasterData.Department.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Department.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Department code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Valid company ID is required");

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage("Remarks must not exceed 2048 characters");

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage("Parent ID must be greater than 0");

            RuleFor(x => x.CostCenterCode)
                .MaximumLength(50).WithMessage("Cost center code must not exceed 50 characters");
        }
    }
}