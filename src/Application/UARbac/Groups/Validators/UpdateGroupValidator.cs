// Application/UARbac/Groups/Validators/UpdateGroupValidator.cs
using Application.UARbac.Groups.Dtos;
using FluentValidation;

namespace Application.UARbac.Groups.Validators
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupDto>
    {
        public UpdateGroupValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Valid Group ID is required");

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage("English name must not exceed 200 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage("Arabic name must not exceed 200 characters");

            RuleFor(x => x)
                .Must(x => x.EngName != null || x.ArbName != null)
                .WithMessage("At least one field must be provided to update");
        }
    }
}