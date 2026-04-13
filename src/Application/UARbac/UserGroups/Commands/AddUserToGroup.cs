using Application.Common;
using Application.Common.Abstractions;
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
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                var lang = _contextService.GetCurrentLanguage();

                RuleFor(x => x.Data.UserId)
                    .GreaterThan(0)
                    .WithMessage(_localizer.GetMessage("UserIdRequired", lang));

                RuleFor(x => x.Data.GroupId)
                    .GreaterThan(0)
                    .WithMessage(_localizer.GetMessage("GroupIdRequired", lang));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IUserGroupRepository _userGroupRepo;
            private readonly IUserRepository _userRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IUserGroupRepository userGroupRepo,
                IUserRepository userRepo,
                IGroupRepository groupRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _userGroupRepo = userGroupRepo;
                _userRepo = userRepo;
                _groupRepo = groupRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var user = await _userRepo.GetByIdAsync(request.Data.UserId);
                if (user == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("User", lang),
                        request.Data.UserId,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("User", lang), request.Data.UserId));

                var group = await _groupRepo.GetByIdAsync(request.Data.GroupId);
                if (group == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Group", lang),
                        request.Data.GroupId,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Group", lang), request.Data.GroupId));

                var exists = await _userGroupRepo.IsUserInGroupAsync(
                    request.Data.UserId,
                    request.Data.GroupId);

                if (exists)
                    throw new ConflictException(
                        _localizer.GetMessage("UserGroup", lang),
                        "UserId/GroupId",
                        $"{request.Data.UserId}/{request.Data.GroupId}",
                        string.Format(_localizer.GetMessage("UserAlreadyInGroup", lang), _localizer.GetMessage("User", lang), _localizer.GetMessage("Group", lang)));

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