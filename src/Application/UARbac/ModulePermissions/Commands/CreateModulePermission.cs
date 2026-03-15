using Application.UARbac.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.ModulePermissions.Validators;
using Application.UARbac.Modules.Validators;
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
            public Validator()
            {
                RuleFor(x => x.Data)
                  .SetValidator(new CreateModulePermissionValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IModulePermissionRepository _repo;
            private readonly IModuleRepository _moduleRepo;
            private readonly IGroupRepository _groupRepo;
            private readonly IUserRepository _userRepo;

            public Handler(
                IModulePermissionRepository repo,
                IModuleRepository moduleRepo,
                IGroupRepository groupRepo,
                IUserRepository userRepo)
            {
                _repo = repo;
                _moduleRepo = moduleRepo;
                _groupRepo = groupRepo;
                _userRepo = userRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                 var module = await _moduleRepo.GetByIdAsync(request.Data.ModuleId);
                if (module == null)
                    throw new Exception($"Module with ID {request.Data.ModuleId} not found");

                 if (request.Data.GroupId.HasValue)
                {
                    var group = await _groupRepo.GetByIdAsync(request.Data.GroupId.Value);
                    if (group == null)
                        throw new Exception($"Group with ID {request.Data.GroupId} not found");

                    var existing = await _repo.GetByModuleAndGroupAsync(
                        request.Data.ModuleId, request.Data.GroupId.Value);
                    if (existing != null)
                        throw new Exception("Permission already exists for this module and group");
                }

                 if (request.Data.UserId.HasValue)
                {
                    var user = await _userRepo.GetByIdAsync(request.Data.UserId.Value);
                    if (user == null)
                        throw new Exception($"User with ID {request.Data.UserId} not found");

                    var existing = await _repo.GetByModuleAndUserAsync(
                        request.Data.ModuleId, request.Data.UserId.Value);
                    if (existing != null)
                        throw new Exception("Permission already exists for this module and user");
                }

                var permission = new ModulePermission(
                    moduleId: request.Data.ModuleId,
                    groupId: request.Data.GroupId,
                    userId: request.Data.UserId,
                    canView: request.Data.CanView,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(permission);
                await _repo.SaveChangesAsync(cancellationToken);

                return permission.Id;
            }
        }
    }
}