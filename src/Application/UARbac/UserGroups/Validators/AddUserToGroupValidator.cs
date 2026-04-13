using Application.UARbac.UserGroups.Dtos;
using FluentValidation;

namespace Application.UARbac.UserGroups.Validators
{
    public class AddUserToGroupValidator : AbstractValidator<AddUserToGroupDto>
    {
        public AddUserToGroupValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Valid User ID is required");

            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("Valid Group ID is required");
        }
    }
}