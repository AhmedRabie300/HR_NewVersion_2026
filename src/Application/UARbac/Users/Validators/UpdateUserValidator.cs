using Application.Common.Abstractions;
using Application.UARbac.Users.Dtos;
using FluentValidation;

namespace Application.UARbac.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x)
                .Must(x => x.EngName != null || x.ArbName != null || x.IsAdmin != null || x.DeviceToken != null)
                .WithMessage(msg.Get("AtLeastOneField"));
        }
    }
}
