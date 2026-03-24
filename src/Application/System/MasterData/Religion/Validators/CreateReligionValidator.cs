using Application.System.MasterData.Religion.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Religion.Validators
{
    public class CreateReligionValidator : AbstractValidator<CreateReligionDto>
    {
        public CreateReligionValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Religion code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage("Remarks must not exceed 2048 characters");
        }
    }
}