// Application/UARbac/UserGroups/Commands/AddUserToGroup.cs
using Application.UARbac.UserGroups.Dtos;
using Application.UARbac.Abstractions;
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
            public Validator()
            {
                RuleFor(x => x.Data.UserId)
                    .GreaterThan(0)
                    .WithMessage("User ID is required");

                RuleFor(x => x.Data.GroupId)
                    .GreaterThan(0)
                    .WithMessage("Group ID is required");
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IUserGroupRepository _userGroupRepo;
            private readonly IUserRepository _userRepo;
            private readonly IGroupRepository _groupRepo;

            public Handler(
                IUserGroupRepository userGroupRepo,
                IUserRepository userRepo,
                IGroupRepository groupRepo)
            {
                _userGroupRepo = userGroupRepo;
                _userRepo = userRepo;
                _groupRepo = groupRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if user exists
                var user = await _userRepo.GetByIdAsync(request.Data.UserId);
                if (user == null)
                    throw new Exception($"User with ID {request.Data.UserId} not found");

                // Check if group exists
                var group = await _groupRepo.GetByIdAsync(request.Data.GroupId);
                if (group == null)
                    throw new Exception($"Group with ID {request.Data.GroupId} not found");

                // Check if already in group
                var exists = await _userGroupRepo.IsUserInGroupAsync(
                    request.Data.UserId,
                    request.Data.GroupId);

                if (exists)
                    throw new Exception($"User already belongs to this group");
 

                // Create new UserGroup
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