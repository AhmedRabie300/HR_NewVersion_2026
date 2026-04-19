using Application.Common.Abstractions;
using Application.UARbac.Groups.Dtos;
using FluentValidation;

namespace Application.UARbac.Groups.Validators
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupDto>
    {
        public CreateGroupValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(200).WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.ArbName)
                .MaximumLength(200).WithMessage(msg.Format("MaxLength", 200));
        }
    }
}
