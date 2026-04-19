using Application.Common.Abstractions;
using Application.UARbac.UserGroups.Dtos;
using FluentValidation;

namespace Application.UARbac.UserGroups.Validators
{
    public class AddUserToGroupValidator : AbstractValidator<AddUserToGroupDto>
    {
        public AddUserToGroupValidator(IValidationMessages msg)
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage(msg.Get("UserIdRequired"));

            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage(msg.Get("GroupIdRequired"));
        }
    }
}
