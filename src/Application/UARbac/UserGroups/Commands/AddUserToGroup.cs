using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.UserGroups.Dtos;
using Application.UARbac.UserGroups.Validators;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.UserGroups.Commands
{
    public static class AddUserToGroup
    {
        public record Command(AddUserToGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new AddUserToGroupValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IUserGroupRepository _userGroupRepo;
            private readonly IUserRepository _userRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IValidationMessages _msg;

            public Handler(
                IUserGroupRepository userGroupRepo,
                IUserRepository userRepo,
                IGroupRepository groupRepo,
                IValidationMessages msg)
            {
                _userGroupRepo = userGroupRepo;
                _userRepo = userRepo;
                _groupRepo = groupRepo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepo.GetByIdAsync(request.Data.UserId);
                if (user == null)
                    throw new NotFoundException(_msg.NotFound("User", request.Data.UserId));

                var group = await _groupRepo.GetByIdAsync(request.Data.GroupId);
                if (group == null)
                    throw new NotFoundException(_msg.NotFound("Group", request.Data.GroupId));

                var exists = await _userGroupRepo.IsUserInGroupAsync(request.Data.UserId, request.Data.GroupId);
                if (exists)
                    throw new ConflictException(_msg.Get("UserAlreadyInGroup"));

                if (request.Data.IsPrimary)
                {
                    var currentPrimary = await _userGroupRepo.GetUserPrimaryGroupAsync(request.Data.UserId);
                    if (currentPrimary != null)
                    {
                        currentPrimary.SetPrimary(false);
                        await _userGroupRepo.UpdateAsync(currentPrimary);
                    }
                }

                var userGroup = new UserGroup(
                    request.Data.UserId,
                    request.Data.GroupId,
                    request.Data.IsPrimary
                );

                await _userGroupRepo.AddAsync(userGroup);
                await _userGroupRepo.SaveChangesAsync(cancellationToken);

                return userGroup.Id;
            }
        }
    }
}
