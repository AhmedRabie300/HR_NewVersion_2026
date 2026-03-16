using Application.System.MasterData.Branch.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Branch.Validators
{
    public class CreateBranchValidator : AbstractValidator<CreateBranchDto>
    {
        public CreateBranchValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Branch code is required")
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

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage("Country ID must be greater than 0");

            RuleFor(x => x.CityId)
                .GreaterThan(0).When(x => x.CityId.HasValue)
                .WithMessage("City ID must be greater than 0");

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage("Prepare day must be between 1 and 31");
        }
    }
}