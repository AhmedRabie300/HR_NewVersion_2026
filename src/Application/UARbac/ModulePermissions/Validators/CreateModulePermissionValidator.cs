using Application.Common.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using FluentValidation;

namespace Application.UARbac.ModulePermissions.Validators
{
    public class CreateModulePermissionValidator : AbstractValidator<CreateModulePermissionDto>
    {
        public CreateModulePermissionValidator(IValidationMessages msg)
        {
            RuleFor(x => x.ModuleId)
                .GreaterThan(0)
                .WithMessage(msg.Get("ModuleIdRequired"));

            RuleFor(x => x)
                .Must(x => (x.GroupId.HasValue && !x.UserId.HasValue) || (!x.GroupId.HasValue && x.UserId.HasValue))
                .WithMessage(msg.Get("EitherGroupOrUser"));

            RuleFor(x => x.CanView)
                .Equal(true).When(x => x.CanView.HasValue)
                .WithMessage(msg.Get("CanViewMustBeTrue"));
        }
    }
}
