using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Queries
{
    public static class GetModulePermissionById
    {
        public record Query(int Id) : IRequest<ModulePermissionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, ModulePermissionDto>
        {
            private readonly IModulePermissionRepository _repo;

            public Handler(IModulePermissionRepository repo)
            {
                _repo = repo;
            }

            public async Task<ModulePermissionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var permission = await _repo.GetByIdAsync(request.Id);
                if (permission == null)
                    throw new Exception($"Permission with ID {request.Id} not found");

                return new ModulePermissionDto(
                    Id: permission.Id,
                    ModuleId: permission.ModuleId,
                    ModuleCode: permission.Module?.Code,
                    ModuleName: permission.Module?.EngName ?? permission.Module?.ArbName,
                    GroupId: permission.GroupId,
                    GroupCode: permission.Group?.Code,
                    GroupName: permission.Group?.EngName ?? permission.Group?.ArbName,
                    UserId: permission.UserId,
                    UserCode: permission.User?.Code,
                    UserName: permission.User?.EngName ?? permission.User?.ArbName,
                    CanView: permission.CanView,
                    RegDate: permission.RegDate,
                    CancelDate: permission.CancelDate,
                    IsActive: permission.IsActive()
                );
            }
        }
    }
}