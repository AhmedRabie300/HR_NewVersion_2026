using Application.UARbac.ModulePermissions.Dtos;
using FluentValidation;

namespace Application.UARbac.ModulePermissions.Validators
{
    public class CreateModulePermissionValidator : AbstractValidator<CreateModulePermissionDto>
    {
        public CreateModulePermissionValidator()
        {
            RuleFor(x => x.ModuleId)
                .GreaterThan(0).WithMessage("Module ID is required");

            RuleFor(x => x)
                .Must(x => (x.GroupId.HasValue && !x.UserId.HasValue) ||
                          (!x.GroupId.HasValue && x.UserId.HasValue))
                .WithMessage("Either GroupId or UserId must be provided, but not both");

            RuleFor(x => x.CanView)
                .Equal(true).When(x => x.CanView.HasValue)
                .WithMessage("CanView must be true if provided");
        }
    }
}