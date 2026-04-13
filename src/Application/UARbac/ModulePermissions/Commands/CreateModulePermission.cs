// Application/UARbac/ModulePermissions/Commands/CreateModulePermission.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.ModulePermissions.Validators;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Commands
{
    public static class CreateModulePermission
    {
        public record Command(CreateModulePermissionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateModulePermissionValidator(_contextService, _localizer));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IModulePermissionRepository _repo;
            private readonly IModuleRepository _moduleRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IUserRepository _userRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IModulePermissionRepository repo,
                IModuleRepository moduleRepo,
                IGroupRepository groupRepo,
                IUserRepository userRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _moduleRepo = moduleRepo;
                _groupRepo = groupRepo;
                _userRepo = userRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var module = await _moduleRepo.GetByIdAsync(request.Data.ModuleId);
                if (module == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Module", lang),
                        request.Data.ModuleId,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Module", lang), request.Data.ModuleId));

                if (request.Data.GroupId.HasValue)
                {
                    var group = await _groupRepo.GetByIdAsync(request.Data.GroupId.Value);
                    if (group == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Group", lang),
                            request.Data.GroupId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Group", lang), request.Data.GroupId.Value));

                    var existing = await _repo.GetByModuleAndGroupAsync(
                        request.Data.ModuleId, request.Data.GroupId.Value);

                    if (existing != null)
                        throw new ConflictException(
                            _localizer.GetMessage("ModulePermission", lang),
                            "ModuleId/GroupId",
                            $"{request.Data.ModuleId}/{request.Data.GroupId.Value}",
                            string.Format(_localizer.GetMessage("PermissionAlreadyExists", lang), _localizer.GetMessage("Module", lang), _localizer.GetMessage("Group", lang)));
                }

                if (request.Data.UserId.HasValue)
                {
                    var user = await _userRepo.GetByIdAsync(request.Data.UserId.Value);
                    if (user == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("User", lang),
                            request.Data.UserId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("User", lang), request.Data.UserId.Value));

                    var existing = await _repo.GetByModuleAndUserAsync(
                        request.Data.ModuleId, request.Data.UserId.Value);

                    if (existing != null)
                        throw new ConflictException(
                            _localizer.GetMessage("ModulePermission", lang),
                            "ModuleId/UserId",
                            $"{request.Data.ModuleId}/{request.Data.UserId.Value}",
                            string.Format(_localizer.GetMessage("PermissionAlreadyExists", lang), _localizer.GetMessage("Module", lang), _localizer.GetMessage("User", lang)));
                }

                var permission = new ModulePermission(
                    moduleId: request.Data.ModuleId,
                    groupId: request.Data.GroupId,
                    userId: request.Data.UserId,
                    canView: request.Data.CanView,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(permission);
                await _repo.SaveChangesAsync(cancellationToken);

                return permission.Id;
            }
        }
    }
}