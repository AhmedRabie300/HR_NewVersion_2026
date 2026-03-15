// Application/UARbac/Menus/Validators/CreateMenuValidator.cs
using Application.UARbac.Menus.Dtos;
using FluentValidation;

namespace Application.UARbac.Menus.Validators
{
    public class CreateMenuValidator : AbstractValidator<CreateMenuDto>
    {
        public CreateMenuValidator()
        {
            RuleFor(x => x.Code)
                .MaximumLength(50)
                .WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage("English name must not exceed 200 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage("Arabic name must not exceed 200 characters");

            RuleFor(x => x.Shortcut)
                .MaximumLength(50)
                .WithMessage("Shortcut must not exceed 50 characters");

            RuleFor(x => x.Image)
                .MaximumLength(500)
                .WithMessage("Image path must not exceed 500 characters");
        }
    }
}