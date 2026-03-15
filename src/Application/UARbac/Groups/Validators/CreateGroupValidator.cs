// Application/UARbac/Groups/Validators/CreateGroupValidator.cs
using Application.UARbac.Groups.Dtos;
using FluentValidation;

namespace Application.UARbac.Groups.Validators
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupDto>
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Group code is required and must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage("English name must not exceed 200 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage("Arabic name must not exceed 200 characters");
        }
    }
}