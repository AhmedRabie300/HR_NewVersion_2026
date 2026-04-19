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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateModulePermissionValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IModulePermissionRepository _repo;
            private readonly IModuleRepository _moduleRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IUserRepository _userRepo;
            private readonly IValidationMessages _msg;

            public Handler(
                IModulePermissionRepository repo,
                IModuleRepository moduleRepo,
                IGroupRepository groupRepo,
                IUserRepository userRepo,
                IValidationMessages msg)
            {
                _repo = repo;
                _moduleRepo = moduleRepo;
                _groupRepo = groupRepo;
                _userRepo = userRepo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var module = await _moduleRepo.GetByIdAsync(request.Data.ModuleId);
                if (module == null)
                    throw new NotFoundException(_msg.NotFound("Module", request.Data.ModuleId));

                if (request.Data.GroupId.HasValue)
                {
                    var group = await _groupRepo.GetByIdAsync(request.Data.GroupId.Value);
                    if (group == null)
                        throw new NotFoundException(_msg.NotFound("Group", request.Data.GroupId.Value));

                    var existing = await _repo.GetByModuleAndGroupAsync(request.Data.ModuleId, request.Data.GroupId.Value);
                    if (existing != null)
                        throw new ConflictException(_msg.Get("PermissionAlreadyExists"));
                }

                if (request.Data.UserId.HasValue)
                {
                    var user = await _userRepo.GetByIdAsync(request.Data.UserId.Value);
                    if (user == null)
                        throw new NotFoundException(_msg.NotFound("User", request.Data.UserId.Value));

                    var existing = await _repo.GetByModuleAndUserAsync(request.Data.ModuleId, request.Data.UserId.Value);
                    if (existing != null)
                        throw new ConflictException(_msg.Get("PermissionAlreadyExists"));
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
