using FluentValidation;
using VenusHR.Application.Common.DTOs.Lookups;

namespace VenusHR.Application.Common.Validators
{
    public class CreateBloodGroupValidator : AbstractValidator<CreateBloodGroupDto>
    {
        public CreateBloodGroupValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(200).WithMessage("English name must not exceed 200 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(200).WithMessage("Arabic name must not exceed 200 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(200).WithMessage("Arabic name (4S) must not exceed 200 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("Remarks must not exceed 500 characters");
        }
    }

    public class UpdateBloodGroupValidator : AbstractValidator<UpdateBloodGroupDto>
    {
        public UpdateBloodGroupValidator()
        {
            RuleFor(x => x.ID)
                .GreaterThan(0).WithMessage("Valid ID is required");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(200).WithMessage("English name must not exceed 200 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(200).WithMessage("Arabic name must not exceed 200 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(200).WithMessage("Arabic name (4S) must not exceed 200 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("Remarks must not exceed 500 characters");
        }
    }
}