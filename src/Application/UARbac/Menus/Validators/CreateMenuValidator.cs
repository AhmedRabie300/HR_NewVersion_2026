using Application.Common.Abstractions;
using Application.UARbac.Menus.Dtos;
using FluentValidation;

namespace Application.UARbac.Menus.Validators
{
    public class CreateMenuValidator : AbstractValidator<CreateMenuDto>
    {
        public CreateMenuValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .MaximumLength(50)
                .WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.Shortcut)
                .MaximumLength(50)
                .WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.Image)
                .MaximumLength(500)
                .WithMessage(msg.Format("MaxLength", 500));
        }
    }
}
