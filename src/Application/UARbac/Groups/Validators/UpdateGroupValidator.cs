using Application.Common.Abstractions;
using Application.UARbac.Groups.Dtos;
using FluentValidation;

namespace Application.UARbac.Groups.Validators
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupDto>
    {
        public UpdateGroupValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x)
                .Must(x => x.EngName != null || x.ArbName != null)
                .WithMessage(msg.Get("AtLeastOneField"));
        }
    }
}
